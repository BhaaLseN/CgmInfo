﻿using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using CgmInfo;
using CgmInfo.Commands;
using CgmInfoGui.Traversal;
using CgmInfoGui.ViewModels.Nodes;
using Microsoft.Win32;
using BinaryMetafileWriter = CgmInfo.BinaryEncoding.MetafileWriter;
using TextMetafileWriter = CgmInfo.TextEncoding.MetafileWriter;

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
                if (SetField(ref _fileName, value))
                    ProcessCommand.NotifyCanExecuteChanged();
            }
        }

        private string _saveAsFilePath;
        public string SaveAsFilePath
        {
            get { return _saveAsFilePath; }
            set
            {
                if (SetField(ref _saveAsFilePath, value))
                    SaveAsCommand.NotifyCanExecuteChanged();
            }
        }
        private string _saveAsFormat = "binary";
        public string SaveAsFormat
        {
            get { return _saveAsFormat; }
            set { SetField(ref _saveAsFormat, value); }
        }

        private List<NodeBase> _metafileNodes;
        public List<NodeBase> MetafileNodes
        {
            get { return _metafileNodes; }
            set { SetField(ref _metafileNodes, value); }
        }

        private List<NodeBase> _apsNodes;
        public List<NodeBase> APSNodes
        {
            get { return _apsNodes; }
            set { SetField(ref _apsNodes, value); }
        }

        private XDocument _xcfDocument;
        public XDocument XCFDocument
        {
            get { return _xcfDocument; }
            set { SetField(ref _xcfDocument, value); }
        }

        private MetafileProperties _metafileDescriptor;
        public MetafileProperties MetafileProperties
        {
            get { return _metafileDescriptor; }
            set { SetField(ref _metafileDescriptor, value); }
        }

        private List<HotspotNode> _hotspots;
        public List<HotspotNode> Hotspots
        {
            get { return _hotspots; }
            set { SetField(ref _hotspots, value); }
        }

        public DelegateCommand BrowseCommand { get; }
        public DelegateCommand ProcessCommand { get; }
        public DelegateCommand BrowseSaveAsCommand { get; }
        public DelegateCommand SaveAsCommand { get; }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (SetField(ref _isBusy, value))
                {
                    BrowseCommand.NotifyCanExecuteChanged();
                    ProcessCommand.NotifyCanExecuteChanged();
                    BrowseSaveAsCommand.NotifyCanExecuteChanged();
                    SaveAsCommand.NotifyCanExecuteChanged();
                }
            }
        }

        public MainViewModel()
        {
            BrowseCommand = new DelegateCommand(Browse, CanBrowse);
            ProcessCommand = new DelegateCommand(Process, CanProcess);
            BrowseSaveAsCommand = new DelegateCommand(BrowseSaveAs, CanBrowseSaveAs);
            SaveAsCommand = new DelegateCommand(SaveAs, CanSaveAs);
        }

        private bool CanBrowse(object parameter) => !IsBusy;
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
                Title = "Select a CGM file",
            };
            if (ofd.ShowDialog() == true)
            {
                FileName = ofd.FileName;
                if (CanProcess(null))
                    Process(null);
            }
        }

        private bool CanProcess(object parameter) => !IsBusy && File.Exists(FileName);
        private async void Process(object parameter)
        {
            IsBusy = true;
            var result = await Task.Run(() =>
            {
                using var reader = MetafileReader.Create(FileName);
                var vmVisitor = new ViewModelBuilderVisitor().WithCommand();
                var metafileContext = new MetafileContext();
                var apsVisitor = new APSStructureBuilderVisitor();
                var apsContext = new APSStructureContext();
                var xcfVisitor = new XCFDocumentBuilderVisitor();
                var xcfContext = new XCFDocumentContext();
                var hotspotVisitor = new HotspotBuilderVisitor();
                var hotspotContext = new HotspotContext();
                Command command;
                do
                {
                    command = reader.Read();
                    if (command != null)
                    {
                        command.Accept(vmVisitor, metafileContext);
                        command.Accept(apsVisitor, apsContext);
                        command.Accept(xcfVisitor, xcfContext);
                        command.Accept(hotspotVisitor, hotspotContext);
                    }
                } while (command != null);
                return new
                {
                    MetafileNodes = metafileContext.RootLevel.ToList(),
                    APSNodes = apsContext.RootLevel.ToList(),
                    XCFDocument = xcfContext.XCF,
                    Hotspots = hotspotContext.RootLevel.OfType<HotspotNode>().ToList(),
                    MetafileProperties = reader.Properties,
                };
            });
            IsBusy = false;
            MetafileNodes = result.MetafileNodes;
            APSNodes = result.APSNodes;
            XCFDocument = result.XCFDocument;
            Hotspots = result.Hotspots;
            MetafileProperties = result.MetafileProperties;
        }

        private bool CanBrowseSaveAs(object parameter) => !IsBusy;
        private void BrowseSaveAs(object parameter)
        {
            var sfd = new SaveFileDialog
            {
                AddExtension = false,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = ".cgm",
                Filter = "Computer Graphics Metafile (*.cgm)|*.cgm",
                RestoreDirectory = true,
                Title = "Save as CGM file",
            };
            if (sfd.ShowDialog() == true)
            {
                SaveAsFilePath = sfd.FileName;
            }
        }
        private bool CanSaveAs(object parameter) => !IsBusy && !string.IsNullOrWhiteSpace(SaveAsFilePath);
        private async void SaveAs(object parameter)
        {
            using MetafileWriter writer = SaveAsFormat switch
            {
                "binary" => new BinaryMetafileWriter(SaveAsFilePath),
                "text" => new TextMetafileWriter(SaveAsFilePath),
                _ => null,
            };
            if (writer == null)
                return;

            IsBusy = true;
            await Task.Run(() =>
            {
                Write(MetafileNodes);
            });
            IsBusy = false;

            void Write(IEnumerable<NodeBase> nodes)
            {
                foreach (var node in nodes)
                {
                    if (node.Command != null)
                        writer.Write(node.Command);
                    Write(node.Nodes);
                }
            }
        }

        private bool SetField<T>(ref T field, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
                return false;

            field = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            return true;
        }
    }
}
