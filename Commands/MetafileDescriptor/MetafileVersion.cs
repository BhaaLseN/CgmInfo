using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("MFVERSION")]
    public class MetafileVersion : Command
    {
        public MetafileVersion(int version)
            : base(1, 1)
        {
            Version = version;
        }

        public int Version { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorMetafileVersion(this, parameter);
        }
    }
}
