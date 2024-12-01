using System.Collections;
using System.Collections.Generic;
using CgmInfo.Commands;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CgmInfoGui.ViewModels.Nodes;

public abstract class NodeBase : ObservableObject, IEnumerable<NodeBase>
{
    public abstract string DisplayName { get; }
    public List<NodeBase> Nodes { get; } = [];
    public Command? Command { get; set; }

    private bool _isExpanded;
    public bool IsExpanded
    {
        get { return _isExpanded; }
        set { SetProperty(ref _isExpanded, value); }
    }

    public virtual void Add(NodeBase node) => Nodes.Add(node);

    public IEnumerator<NodeBase> GetEnumerator() => Nodes.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Nodes.GetEnumerator();
}
