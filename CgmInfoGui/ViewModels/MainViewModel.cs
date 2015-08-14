using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using CgmInfo.Binary;
using CgmInfo.Commands;
using CgmInfoGui.Traversal;
using CgmInfoGui.ViewModels.Nodes;
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

        private List<NodeBase> _metafileNodes;
        public List<NodeBase> MetafileNodes
        {
            get { return _metafileNodes; }
            set
            {
                if (value != _metafileNodes)
                {
                    _metafileNodes = value;
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
                if (CanProcess(null))
                    Process(null);
            }
        }

        private bool CanProcess(object parameter) => File.Exists(FileName);
        private void Process(object parameter)
        {
            using (var reader = new MetafileReader(FileName))
            {
                var vmVisitor = new ViewModelBuilderVisitor();
                var metafileContext = new MetafileContext();
                Command command;
                do
                {
                    command = reader.ReadCommand();
                    if (command != null)
                    {
                        // stop processing as soon as we reach a non-delimiter or non-metafile descriptor element; we're only interrested in the descriptor for now.
                        if (command.ElementClass >= 2)
                            break;

                        command.Accept(vmVisitor, metafileContext);
                    }
                } while (command != null);
                MetafileNodes = metafileContext.RootLevel.ToList();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
