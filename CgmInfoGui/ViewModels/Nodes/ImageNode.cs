using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CgmInfo.Commands.Enums;
using CgmInfoGui.ViewModels.Nodes.Sources;

namespace CgmInfoGui.ViewModels.Nodes
{
    public abstract class ImageNodeBase : NodeBase, INotifyPropertyChanged
    {
        protected ImageNodeBase() { }

        private bool _loading;
        private ImageSource _image;
        public ImageSource Image
        {
            get
            {
                if (NeedsLoading)
                {
                    TryLoadImage();
                }
                return _image;
            }
            private set { SetField(ref _image, value); }
        }

        internal Task TryLoadImage()
        {
            _loading = true;
            return Task.Run(LoadImage).ContinueWith(UpdateImage);
        }

        public bool NeedsLoading => _image == null && !_loading;

        private Exception _loadError;
        public Exception LoadError
        {
            get { return _loadError; }
            private set { SetField(ref _loadError, value); }
        }

        private async Task UpdateImage(Task<ImageSource> imageSource)
        {
            try
            {
                Image = await imageSource;
            }
            catch (Exception ex)
            {
                LoadError = ex;
            }
            finally
            {
                _loading = false;
                // this is necessary since TryLoadImage is run as fire&forget, and may return an old value
                // before the property is updated (while raising the event, still returning null)
                OnPropertyChanged(nameof(Image));
            }
        }
        protected abstract Task<ImageSource> LoadImage();
        public override string DisplayName => "(Image)";

        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetField<T>(ref T field, T newValue)
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
                return false;

            field = newValue;
            return true;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class ImageNode : ImageNodeBase
    {
        private readonly ITileSource _tile;

        public ImageNode(ITileSource tileSource)
        {
            _tile = tileSource ?? throw new ArgumentNullException(nameof(tileSource));
        }

        protected override Task<ImageSource> LoadImage()
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
    }

    public class CombinedImageNode : ImageNodeBase
    {
        private readonly int _pathDirectionTileCount;
        private readonly int _lineDirectionTileCount;
        private readonly double _tileWidth;
        private readonly double _tileHeight;
        private readonly double _dpiX;
        private readonly double _dpiY;
        private readonly IEnumerable<NodeBase> _collectionContainingTheImages;

        public CombinedImageNode(int pathDirectionTileCount, int lineDirectionTileCount, double tileWidth, double tileHeight, double dpiX, double dpiY, IEnumerable<NodeBase> collectionContainingTheImages)
        {
            _pathDirectionTileCount = pathDirectionTileCount;
            _lineDirectionTileCount = lineDirectionTileCount;
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;
            _dpiX = dpiX;
            _dpiY = dpiY;
            _collectionContainingTheImages = collectionContainingTheImages;
        }

        protected override Task<ImageSource> LoadImage()
        {
            var allImageNodes = _collectionContainingTheImages.SelectMany(n => n.Nodes).OfType<ImageNode>().ToArray();
            var nodesThatNeedLoading = allImageNodes.Where(i => i.NeedsLoading && i.LoadError == null).ToArray();
            if (nodesThatNeedLoading.Any())
                Task.WaitAll(nodesThatNeedLoading.Select(n => n.TryLoadImage()).ToArray());

            var d = new DrawingVisual();
            using (var dc = d.RenderOpen())
            {
                for (int x = 0; x < _pathDirectionTileCount; x++)
                {
                    for (int y = 0; y < _lineDirectionTileCount; y++)
                    {
                        var tile = allImageNodes[x + y * _pathDirectionTileCount];
                        if (tile.LoadError == null)
                            dc.DrawImage(tile.Image, new System.Windows.Rect(x * _tileWidth, y * _tileHeight, _tileWidth, _tileHeight));
                    }
                }
            }

            // TODO: using _dpiX/_dpiY seems incorrect, tiles above tend to be 96 while the values passed in are higher.
            var rtb = new RenderTargetBitmap(_pathDirectionTileCount * (int)_tileWidth, _lineDirectionTileCount * (int)_tileHeight, 96, 96, PixelFormats.Pbgra32);
            rtb.Render(d);
            rtb.Freeze();

            return Task.FromResult<ImageSource>(rtb);
        }
    }
}
