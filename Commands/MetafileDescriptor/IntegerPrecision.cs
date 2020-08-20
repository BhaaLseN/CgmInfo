using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("INTEGERPREC")]
    public class IntegerPrecision : Command
    {
        public IntegerPrecision(int precision)
            : base(1, 4)
        {
            Precision = precision;
        }

        public int Precision { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorIntegerPrecision(this, parameter);
        }
    }
}
