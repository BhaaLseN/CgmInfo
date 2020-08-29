using System;
using System.Linq;
using System.Text;
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

        public override string DisplayName => "BEGIN APPLICATION STRUCTURE: " + GenerateDisplayText();

        public string GenerateDisplayText()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} '{1}'", Type, Identifier);

            var name = Nodes.OfType<APSAttributeNode>().FirstOrDefault(n => string.Equals(n.Name, "NAME", StringComparison.OrdinalIgnoreCase));
            if (name != null)
                sb.AppendFormat(" - '{0}'", name.Value);
            var layerName = Nodes.OfType<APSAttributeNode>().FirstOrDefault(n => string.Equals(n.Name, "LAYERNAME", StringComparison.OrdinalIgnoreCase));
            if (layerName != null)
                sb.AppendFormat(" ({0})", layerName.Value);

            return sb.ToString();
        }
    }
}
