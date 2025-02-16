using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CgmInfo;
using CgmInfo.Commands;
using CgmInfo.Traversal;
using CgmInfoGui.Services;
using CgmInfoGui.Traversal;
using CgmInfoGui.ViewModels.Nodes;
using CommunityToolkit.Mvvm.Input;
using Dock.Model.Mvvm.Controls;
using BinaryMetafileWriter = CgmInfo.BinaryEncoding.MetafileWriter;
using TextMetafileWriter = CgmInfo.TextEncoding.MetafileWriter;

namespace CgmInfoGui.ViewModels;

public partial class MetafileStructureViewModel : Document, ICommandReceiver
{
    private readonly MainWindowViewModel _mainWindowViewModel;
    private readonly IFileService _fileService;

    private List<NodeBase>? _metafileNodes;
    public List<NodeBase>? MetafileNodes
    {
        get { return _metafileNodes; }
        set { SetProperty(ref _metafileNodes, value); }
    }
    private string? _saveAsFilePath;
    public string? SaveAsFilePath
    {
        get { return _saveAsFilePath; }
        set
        {
            if (SetProperty(ref _saveAsFilePath, value))
                SaveAsCommand.NotifyCanExecuteChanged();
        }
    }
    private string _saveAsFormat = "binary";
    public string SaveAsFormat
    {
        get { return _saveAsFormat; }
        set { SetProperty(ref _saveAsFormat, value); }
    }
    public bool IsBusy
    {
        get => _mainWindowViewModel.IsBusy;
        set => _mainWindowViewModel.IsBusy = value;
    }

    public MetafileStructureViewModel(MainWindowViewModel mainWindowViewModel, IFileService fileService)
    {
        _mainWindowViewModel = mainWindowViewModel;
        _mainWindowViewModel.PropertyChanged += MainWindowViewModel_PropertyChanged;
        _fileService = fileService;
        Title = "Metafile Structure";
        CanClose = false;
    }

    private void MainWindowViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(MainWindowViewModel.IsBusy))
            return;

        BrowseSaveAsCommand.NotifyCanExecuteChanged();
        SaveAsCommand.NotifyCanExecuteChanged();
    }
    private bool CanBrowseSaveAs() => !IsBusy;
    [RelayCommand(CanExecute = nameof(CanBrowseSaveAs))]
    private async Task BrowseSaveAs()
    {
        SaveAsFilePath = await _fileService.SaveFileAsync("Save as CGM file", ("Computer Graphics Metafile", "*.cgm"));
    }
    [MemberNotNullWhen(true, nameof(SaveAsFilePath))]
    private bool CanSaveAs() => !IsBusy && !string.IsNullOrWhiteSpace(SaveAsFilePath);
    [RelayCommand(CanExecute = nameof(CanSaveAs))]
    private async Task SaveAs()
    {
        if (!CanSaveAs())
            return;

        using MetafileWriter? writer = SaveAsFormat switch
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
            Write(MetafileNodes!);
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

    ICommandAcceptor ICommandReceiver.BeginReceiving() => new Acceptor(this);
    private sealed class Acceptor(MetafileStructureViewModel parent) : ICommandAcceptor
    {
        private readonly ICommandVisitor<MetafileContext> _visitor = new ViewModelBuilderVisitor().WithCommand();
        private readonly MetafileContext _parameters = new();
        public void Accept(Command command) => command.Accept(_visitor, _parameters);
        public void Dispose() => parent.MetafileNodes = [.. _parameters.RootLevel];
    }
}
