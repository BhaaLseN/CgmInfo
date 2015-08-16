using CgmInfo.Traversal;

namespace CgmInfo.Commands.ApplicationStructureDescriptor
{
    public class ApplicationStructureAttribute : Command
    {
        public ApplicationStructureAttribute(string attributeType, StructuredDataRecord dataRecord)
            : base(9, 1)
        {
            AttributeType = attributeType;
            DataRecord = dataRecord;

        }

        public string AttributeType { get; private set; }
        public StructuredDataRecord DataRecord { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptApplicationStructureDescriptorAttribute(this, parameter);
        }
    }
}
