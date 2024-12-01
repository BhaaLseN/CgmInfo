using System.Collections.Generic;
using System.Linq;
using CgmInfo.Commands;
using CgmInfoGui.Traversal;
using CgmInfoGui.ViewModels.Nodes;
using Dock.Model.Mvvm.Controls;

namespace CgmInfoGui.ViewModels;

public class HotspotsViewModel : Document, ICommandReceiver
{
    private List<HotspotNode>? _hotspots;
    public List<HotspotNode>? Hotspots
    {
        get { return _hotspots; }
        set { SetProperty(ref _hotspots, value); }
    }

    public HotspotsViewModel()
    {
        Title = "Hotspots";
        CanClose = false;
    }

    ICommandAcceptor ICommandReceiver.BeginReceiving() => new Acceptor(this);
    private sealed class Acceptor(HotspotsViewModel parent) : ICommandAcceptor
    {
        private readonly HotspotBuilderVisitor _visitor = new();
        private readonly HotspotContext _parameters = new();
        public void Accept(Command command) => command.Accept(_visitor, _parameters);
        public void Dispose() => parent.Hotspots = [.. _parameters.RootLevel.OfType<HotspotNode>()];
    }
}
