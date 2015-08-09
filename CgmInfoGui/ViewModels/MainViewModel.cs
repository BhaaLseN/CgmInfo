using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Win32;

namespace CgmInfoGui.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (value != _fileName)
                {
                    _fileName = value;
                    ProcessCommand.NotifyCanExecuteChanged();
                    OnPropertyChanged();
                }
            }
        }
        public DelegateCommand BrowseCommand { get; }
        public DelegateCommand ProcessCommand { get; }

        public MainViewModel()
        {
            BrowseCommand = new DelegateCommand(Browse);
            ProcessCommand = new DelegateCommand(Process, CanProcess);
        }

        private void Browse(object parameter)
        {
            var ofd = new OpenFileDialog
            {
                AddExtension = false,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = ".cgm",
                Filter = "Computer Graphics Metafile (*.cgm)|*.cgm",
                Multiselect = false,
                RestoreDirectory = true,
                Title = "Select a CGM file (binary encoded)",
            };
            if (ofd.ShowDialog() == true)
            {
                FileName = ofd.FileName;
            }
        }

        private bool CanProcess(object parameter) => File.Exists(FileName);
        private void Process(object parameter)
        {
            // TODO: implement
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
