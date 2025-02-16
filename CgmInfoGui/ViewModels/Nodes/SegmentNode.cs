namespace CgmInfoGui.ViewModels.Nodes;

public class SegmentNode : NodeBase
{
    public SegmentNode(int identifier)
    {
        Identifier = identifier;
        Descriptor = new SimpleNode("SEGMENT DESCRIPTOR");
        Nodes.Add(Descriptor);
    }
    public int Identifier { get; }
    public SimpleNode Descriptor { get; }

    public override string DisplayName
    {
        get { return string.Format("BEGIN SEGMENT: {0}", Identifier); }
    }
}
