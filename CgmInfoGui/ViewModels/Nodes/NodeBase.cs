using System.Collections.Generic;

namespace CgmInfoGui.ViewModels.Nodes
{
    public abstract class NodeBase
    {
        public abstract string DisplayName { get; }
        public List<NodeBase> Nodes { get; } = new List<NodeBase>();
    }
}
