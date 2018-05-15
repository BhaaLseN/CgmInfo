using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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

            throw new NotImplementedException($"Decoding of compression type {_tile.CompressionType} is not implemented yet.");
        }

        public override string DisplayName => "(Image)";

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
