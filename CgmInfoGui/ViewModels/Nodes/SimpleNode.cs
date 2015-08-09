namespace CgmInfoGui.ViewModels.Nodes
{
    public class SimpleNode : NodeBase
    {
        public SimpleNode(string name)
        {
            Name = name;
        }
        public string Name { get; }

        public override string DisplayName
        {
            get { return Name; }
        }
    }
}
