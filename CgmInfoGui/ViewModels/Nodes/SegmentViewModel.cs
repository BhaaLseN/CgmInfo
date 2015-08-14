namespace CgmInfoGui.ViewModels.Nodes
{
    public class SegmentViewModel : NodeBase
    {
        public SegmentViewModel(string name)
        {
            Name = name;
            Descriptor = new SimpleNode("SEGMENT DESCRIPTOR");
            Nodes.Add(Descriptor);
        }
        public string Name { get; }
        public SimpleNode Descriptor { get; }

        public override string DisplayName
        {
            get { return string.Format("BEGIN SEGMENT: '{0}'", Name); }
        }
    }
}
