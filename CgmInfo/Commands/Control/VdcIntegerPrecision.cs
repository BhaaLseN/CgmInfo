using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("VDCINTEGERPREC")]
    public class VdcIntegerPrecision : Command
    {
        public VdcIntegerPrecision(int precision)
            : base(3, 1)
        {
            Precision = precision;
        }

        public int Precision { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptControlVdcIntegerPrecision(this, parameter);
        }
    }
}
