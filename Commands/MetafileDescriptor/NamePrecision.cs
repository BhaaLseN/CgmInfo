using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class NamePrecision : Command
    {
        public NamePrecision(int precision)
            : base(1, 16)
        {
            Precision = precision;
        }

        public int Precision { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorNamePrecision(this, parameter);
        }
    }
}
