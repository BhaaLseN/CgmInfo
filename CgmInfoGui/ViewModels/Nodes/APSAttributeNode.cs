using System;
using System.Linq;
using CgmInfo.Commands.ApplicationStructureDescriptor;

namespace CgmInfoGui.ViewModels.Nodes
{
    public class APSAttributeNode : NodeBase
    {
        public APSAttributeNode(ApplicationStructureAttribute apsAttribute)
        {
            Name = apsAttribute.AttributeType;
            Nodes.AddRange(apsAttribute.DataRecord.Elements.SelectMany(e => e.Values).Select(e => new SimpleNode(Convert.ToString(e))));
        }
        public string Name { get; }

        public override string DisplayName
        {
            get { return Name; }
        }
    }
}
