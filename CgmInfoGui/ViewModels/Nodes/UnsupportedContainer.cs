namespace CgmInfoGui.ViewModels.Nodes;

public class UnsupportedContainer : NodeBase
{
    public override string DisplayName
    {
        get { return string.Format("Unsupported Nodes [{0} entries]", Nodes.Count); }
    }
}
