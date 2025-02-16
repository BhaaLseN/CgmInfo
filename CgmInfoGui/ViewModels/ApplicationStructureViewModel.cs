using System.Collections.Generic;
using CgmInfo.Commands;
using CgmInfoGui.Traversal;
using CgmInfoGui.ViewModels.Nodes;
using Dock.Model.Mvvm.Controls;

namespace CgmInfoGui.ViewModels;

public class ApplicationStructureViewModel : Document, ICommandReceiver
{
    private List<NodeBase>? _apsNodes;
    public List<NodeBase>? APSNodes
    {
        get { return _apsNodes; }
        set { SetProperty(ref _apsNodes, value); }
    }

    public ApplicationStructureViewModel()
    {
        Title = "APS Structure";
        CanClose = false;
    }

    ICommandAcceptor ICommandReceiver.BeginReceiving() => new Acceptor(this);
    private sealed class Acceptor(ApplicationStructureViewModel parent) : ICommandAcceptor
    {
        private readonly APSStructureBuilderVisitor _visitor = new();
        private readonly APSStructureContext _parameters = new();
        public void Accept(Command command) => command.Accept(_visitor, _parameters);
        public void Dispose() => parent.APSNodes = [.. _parameters.RootLevel];
    }
}
