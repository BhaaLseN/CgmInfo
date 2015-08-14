using CgmInfo.Commands.Delimiter;

namespace CgmInfoGui.ViewModels.Nodes
{
    public class ApplicationStructureViewModel : NodeBase
    {
        public ApplicationStructureViewModel(BeginApplicationStructure beginApplicationStructure)
        {
            Identifier = beginApplicationStructure.Identifier;
            Type = beginApplicationStructure.Type;
            Descriptor = new SimpleNode("APPLICATION STRUCTURE DESCRIPTOR")
            {
                new SimpleNode(string.Format("Identifier: '{0}'", beginApplicationStructure.Identifier)),
                new SimpleNode(string.Format("Type: '{0}'", beginApplicationStructure.Type)),
                new SimpleNode(string.Format("Inheritance: {0}", beginApplicationStructure.Inheritance)),
            };
            Nodes.Add(Descriptor);
        }
        public string Identifier { get; }
        public string Type { get; }
        public SimpleNode Descriptor { get; }

        public override string DisplayName
        {
            get { return string.Format("BEGIN APPLICATION STRUCTURE: {0} '{1}'", Type, Identifier); }
        }
    }
}
