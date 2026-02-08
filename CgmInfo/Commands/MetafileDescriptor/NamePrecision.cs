using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("NAMEPREC")]
    public class NamePrecision : Command
    {
        public NamePrecision(int precision)
            : base(1, 16)
        {
            Precision = precision;
        }

        public int Precision { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorNamePrecision(this, parameter);
        }
    }
}
