using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CgmInfo.Commands.Enums;
using CgmInfoGui.ViewModels.Nodes.Sources;

namespace CgmInfoGui.ViewModels.Nodes
{
    public class ImageNode : NodeBase, INotifyPropertyChanged
    {
        private readonly ITileSource _tile;

        public ImageNode(ITileSource tileSource)
        {
            _tile = tileSource ?? throw new ArgumentNullException(nameof(tileSource));
        }

        private bool _loading;
        private ImageSource _image;
        public ImageSource Image
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
            private set
            {
                if (_image == value)
                    return;
                _image = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Image)));
            }
        }
        private Exception _loadError;
        public Exception LoadError
        {
            get { return _loadError; }
            private set
            {
                if (_loadError == value)
                    return;
                _loadError = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LoadError)));
            }
        }

        private Task<ImageSource> LoadImage()
        {
            if (_tile.CompressionType == 7)
            {
                // JPEG has a straight JFIF chunk in there, we can just throw it at the decoder and be done with it.
                var ms = new MemoryStream(_tile.CompressedCells);
                var jpegDecoder = new JpegBitmapDecoder(ms, BitmapCreateOptions.None, BitmapCacheOption.None);
                return Task.FromResult(jpegDecoder.Frames[0].GetAsFrozen() as ImageSource);
            }
            else if (_tile.CompressionType == 9)
            {
                // PNG is a little special: the SDR contains bitstreams, one per chunk;
                // we'll have to piece them together (and create a header), otherwise the decoder won't be able to read it.
                var ms = new MemoryStream();

                // PNG header
                ms.Write(new byte[] { 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a }, 0, 8);
                // other headers from the SDR (usually at least IHDR)
                foreach (var parameter in _tile.Parameters.Elements)
                {
                    if (parameter.Type == DataTypeIndex.BitStream && parameter.Values.FirstOrDefault() is byte[] data)
                        ms.Write(data, 0, data.Length);
                }
                // actual payload, IDAT/IEND from the TILE
                byte[] compressedCells = _tile.CompressedCells;
                ms.Write(compressedCells, 0, compressedCells.Length);
                ms.Position = 0;

                var pngDecoder = new PngBitmapDecoder(ms, BitmapCreateOptions.None, BitmapCacheOption.None);
                return Task.FromResult(pngDecoder.Frames[0].GetAsFrozen() as ImageSource);
            }
            else if ((_tile.CompressionType >= 2 && _tile.CompressionType <= 5) || _tile.CompressionType == 7)
            {
                // we need to create a TIFF header here, otherwise the decoder won't be able to read it.
                // TIFF will do all the CCITT compressions for us, as well as LZW and uncompressed (Bitmap).
                var generator = new TiffGenerator(_tile);
                var ms = generator.CreateStream();

                var tiffDecoder = new TiffBitmapDecoder(ms, BitmapCreateOptions.None, BitmapCacheOption.None);
                return Task.FromResult(tiffDecoder.Frames[0].GetAsFrozen() as ImageSource);
            }

            throw new NotImplementedException($"Decoding of compression type {_tile.CompressionType} is not implemented yet.");
        }

        public override string DisplayName => "(Image)";

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
