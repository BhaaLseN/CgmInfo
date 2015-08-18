using System;
using System.Linq;
using CgmInfo.Commands.ApplicationStructureDescriptor;
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Traversal
{
    public class APSStructureContext : NodeContext
    {
        public void AddAttributeNode(ApplicationStructureAttribute applicationStructureAttribute)
        {
            // in case theres just a single value; show it directly.
            var allValues = applicationStructureAttribute.DataRecord.Elements.SelectMany(el => el.Values).ToArray();
            if (allValues.Length == 1)
            {
                AddNode("Attribute '{0}' = '{1}'", applicationStructureAttribute.AttributeType, allValues[0]);
            }
            else
            {
                var attributeNode = AddNode("Attribute '{0}'", applicationStructureAttribute.AttributeType);
                attributeNode.Nodes.AddRange(allValues.Select(value => new SimpleNode(Convert.ToString(value))));
            }
        }
    }
}
