using CgmInfo.Commands;
using CgmInfoGui.Traversal;
using CgmInfoGui.Visuals;
using CommunityToolkit.Mvvm.ComponentModel;
using Dock.Model.Mvvm.Controls;

namespace CgmInfoGui.ViewModels;

public partial class VisualsViewModel : Document, ICommandReceiver
{
    [ObservableProperty]
    private VisualRoot? _visualRoot;

    public VisualsViewModel()
    {
        Title = "Visuals";
        CanClose = false;
    }

    ICommandAcceptor ICommandReceiver.BeginReceiving() => new Acceptor(this);
    private sealed class Acceptor(VisualsViewModel parent) : ICommandAcceptor
    {
        private readonly GraphicalElementBuilderVisitor _visitor = new();
        private readonly GraphicalElementContext _parameters = new();
        public void Accept(Command command) => command.Accept(_visitor, _parameters);
        public void Dispose() => parent.VisualRoot = _parameters.Visuals;
    }
}
