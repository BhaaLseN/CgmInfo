using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CgmInfo.Commands.Enums;
using CgmInfoGui.ViewModels.Nodes.Sources;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tiff;

namespace CgmInfoGui.ViewModels.Nodes;

public class ImageNode : NodeBase
{
    private readonly ITileSource _tile;

    public ImageNode(ITileSource tileSource)
    {
        _tile = tileSource ?? throw new ArgumentNullException(nameof(tileSource));
    }

    private bool _loading;
    private IImage? _image;
    public IImage? Image
    {
        get
        {
            if (_image == null && !_loading)
            {
                _loading = true;
                Task.Run(LoadImage).ContinueWith(async i =>
                {
                    try
                    {
                        Image = await i;
                    }
                    catch (Exception ex)
                    {
                        LoadError = ex;
                    }
                    finally
                    {
                        _loading = false;
                    }
                });
            }
            return _image;
        }
        private set { SetProperty(ref _image, value); }
    }
    private Exception? _loadError;
    public Exception? LoadError
    {
        get { return _loadError; }
        private set { SetProperty(ref _loadError, value); }
    }

    private IImage LoadImage()
    {
        if (_tile.CompressionType == 7)
        {
            // JPEG has a straight JFIF chunk in there, we can just throw it at the decoder and be done with it.
            var ms = new MemoryStream(_tile.CompressedCells);
            return new Bitmap(ms);
        }
        else if (_tile.CompressionType == 9)
        {
            // PNG is a little special: the SDR contains bitstreams, one per chunk;
            // we'll have to piece them together (and create a header), otherwise the decoder won't be able to read it.
            var ms = new MemoryStream();

            // PNG header
            ms.Write([0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a]);
            // other headers from the SDR (usually at least IHDR)
            foreach (var parameter in _tile.Parameters.Elements)
            {
                if (parameter.Type == DataTypeIndex.BitStream && parameter.Values.FirstOrDefault() is byte[] data)
                    ms.Write(data, 0, data.Length);
            }
            // actual payload, IDAT/IEND from the TILE
            byte[] compressedCells = _tile.CompressedCells;
            ms.Write(compressedCells, 0, compressedCells.Length);
            // FIXME: test if there is actually an IEND chunk. some generators omit it.
            ms.Position = 0;

            return new Bitmap(ms);
        }
        else if (_tile.CompressionType is (>= 2 and <= 5) or 7)
        {
            // we need to create a TIFF header here, otherwise the decoder won't be able to read it.
            // TIFF will do all the CCITT compressions for us, as well as LZW and uncompressed (Bitmap).
            using var generator = new TiffGenerator(_tile);
            using var ms = generator.CreateStream();

            // Avalonia (or rather: Skia) has no support for TIFF. Convert to PNG first.
            using var img = TiffDecoder.Instance.Decode(new SixLabors.ImageSharp.Formats.DecoderOptions(), ms) ?? throw new InvalidOperationException("Couldn't decode TIFF stream.");
            var pngMS = new MemoryStream();
            img.Save(pngMS, new PngEncoder());

            pngMS.Position = 0;

            return new Bitmap(pngMS);
        }

        throw new NotImplementedException($"Decoding of compression type {_tile.CompressionType} is not implemented yet.");
    }

    public override string DisplayName => "(Image)";
}
