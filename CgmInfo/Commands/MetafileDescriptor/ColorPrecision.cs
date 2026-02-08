using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("COLRPREC")]
    public class ColorPrecision : Command
    {
        public ColorPrecision(int precision)
            : base(1, 7)
        {
            Precision = precision;
        }

        public int Precision { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorColorPrecision(this, parameter);
        }
    }
}
