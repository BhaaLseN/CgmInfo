using CgmInfo.Traversal;

namespace CgmInfo.Commands.ApplicationStructureDescriptor
{
    [TextToken("APSATTR")]
    public class ApplicationStructureAttribute : Command
    {
        public ApplicationStructureAttribute(string attributeType, StructuredDataRecord dataRecord)
            : base(9, 1)
        {
            AttributeType = attributeType;
            DataRecord = dataRecord;

        }

        public string AttributeType { get; }
        public StructuredDataRecord DataRecord { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptApplicationStructureDescriptorAttribute(this, parameter);
        }
    }
}
