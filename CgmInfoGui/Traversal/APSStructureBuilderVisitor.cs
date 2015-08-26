using CgmInfo.Commands.ApplicationStructureDescriptor;
using CgmInfo.Commands.Delimiter;
using CgmInfo.Traversal;

namespace CgmInfoGui.Traversal
{
    public class APSStructureBuilderVisitor : CommandVisitor<APSStructureContext>
    {
        public override void AcceptApplicationStructureDescriptorAttribute(ApplicationStructureAttribute applicationStructureAttribute, APSStructureContext parameter)
        {
            parameter.AddAttributeNode(applicationStructureAttribute);
        }

        public override void AcceptDelimiterBeginApplicationStructure(BeginApplicationStructure beginApplicationStructure, APSStructureContext parameter)
        {
            parameter.BeginLevel("{0} '{1}'", beginApplicationStructure.Type, beginApplicationStructure.Identifier);
        }

        public override void AcceptDelimiterBeginApplicationStructureBody(BeginApplicationStructureBody beginApplicationStructureBody, APSStructureContext parameter)
        {
            // left blank for now, but we never know if/when we'd need it.
        }

        public override void AcceptDelimiterEndApplicationStructure(EndApplicationStructure endApplicationStructure, APSStructureContext parameter)
        {
            parameter.EndLevel();
        }
    }
}
