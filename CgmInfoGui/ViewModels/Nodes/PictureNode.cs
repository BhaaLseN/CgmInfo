namespace CgmInfoGui.ViewModels.Nodes;

public class PictureNode : NodeBase
{
    public PictureNode(string name)
    {
        Name = name;
        Descriptor = new SimpleNode("PICTURE DESCRIPTOR");
        Nodes.Add(Descriptor);
    }
    public string Name { get; }
    public SimpleNode Descriptor { get; }

    public override string DisplayName
    {
        get { return string.Format("BEGIN PICTURE: '{0}'", Name); }
    }
}
