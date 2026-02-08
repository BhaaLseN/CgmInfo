using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("MFDESC")]
    public class MetafileDescription : Command
    {
        public MetafileDescription(string description)
            : base(1, 2)
        {
            Description = description;
        }

        public string Description { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorMetafileDescription(this, parameter);
        }
    }
}
