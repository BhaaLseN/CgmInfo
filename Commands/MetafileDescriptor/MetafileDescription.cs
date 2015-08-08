using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class MetafileDescription : Command
    {
        public MetafileDescription(string description)
            : base(1, 2)
        {
            Description = description;
        }

        public string Description { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorMetafileDescription(this, parameter);
        }
    }
}
