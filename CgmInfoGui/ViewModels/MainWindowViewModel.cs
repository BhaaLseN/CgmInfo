using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using CgmInfo;
using CgmInfo.Commands;
using CgmInfoGui.Services;
using CommunityToolkit.Mvvm.Input;
using Dock.Model.Controls;
using Dock.Model.Core;

namespace CgmInfoGui.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, ICommandReceiver
{
    private readonly IFileService _fileService;

    private string? _fileName;
    public string? FileName
    {
        get { return _fileName; }
        set
        {
            if (SetProperty(ref _fileName, value))
                ProcessCommand.NotifyCanExecuteChanged();
        }
    }

    private MetafileProperties? _metafileDescriptor;
    public MetafileProperties? MetafileProperties
    {
        get { return _metafileDescriptor; }
        set { SetProperty(ref _metafileDescriptor, value); }
    }

    private IRootDock? _layout;
    public IRootDock? Layout { get => _layout; set => SetProperty(ref _layout, value); }
    public IList<IDockable> Documents { get; }

    public MetafileStructureViewModel MetafileStructure { get; }
    public ApplicationStructureViewModel ApplicationStructure { get; }
    public CompanionFileViewModel CompanionFile { get; }
    public HotspotsViewModel Hotspots { get; }

    private bool _isBusy;

    public bool IsBusy
    {
        get { return _isBusy; }
        set
        {
            if (SetProperty(ref _isBusy, value))
            {
                BrowseCommand.NotifyCanExecuteChanged();
                ProcessCommand.NotifyCanExecuteChanged();
            }
        }
    }

    public MainWindowViewModel(IFileService fileService)
    {
        _fileService = fileService;

        MetafileStructure = new(this, _fileService);
        ApplicationStructure = new();
        CompanionFile = new();
        Hotspots = new();
        Documents = [MetafileStructure, ApplicationStructure, CompanionFile, Hotspots];

        _layout = new DockFactory(this).CreateLayout();
    }

    private bool CanBrowse(object parameter) => !IsBusy;
    [RelayCommand(CanExecute = nameof(CanBrowse))]
    private async Task Browse(object parameter)
    {
        string? fileName = await _fileService.OpenFileAsync("Select a CGM file", ("Computer Graphics Metafile", "*.cgm"));
        if (!string.IsNullOrWhiteSpace(fileName) && File.Exists(fileName))
        {
            FileName = fileName;
            await Process();
        }
    }

    [MemberNotNullWhen(true, nameof(FileName))]
    private bool CanProcess() => !IsBusy && File.Exists(FileName);
    [RelayCommand(CanExecute = nameof(CanProcess))]
    private async Task Process()
    {
        if (!CanProcess())
            return;

        using var acceptors = BeginReceiving();
        IsBusy = true;
        var result = await Task.Run(() =>
        {
            using var reader = MetafileReader.Create(FileName);
            Command? command;
            do
            {
                command = reader.Read();
                if (command != null)
                    acceptors.Accept(command);
            } while (command != null);
            return reader.Properties;
        });
        IsBusy = false;
        MetafileProperties = result;
    }

    public ICommandAcceptor BeginReceiving() => new ListAcceptor(Documents.OfType<ICommandReceiver>());
    private sealed class ListAcceptor : ICommandAcceptor
    {
        private readonly ICommandAcceptor[] _acceptors;

        public ListAcceptor(IEnumerable<ICommandReceiver> documents)
        {
            _acceptors = documents.Select(r => r.BeginReceiving()).ToArray();
        }

        public void Accept(Command command) => Array.ForEach(_acceptors, a => a.Accept(command));
        public void Dispose() => Array.ForEach(_acceptors, a => a.Dispose());
    }
}

public interface ICommandReceiver
{
    ICommandAcceptor BeginReceiving();
}
public interface ICommandAcceptor : IDisposable
{
    void Accept(Command command);
}
