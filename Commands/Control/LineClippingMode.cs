using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("LINECLIPMODE")]
    public class LineClippingMode : Command
    {
        public LineClippingMode(ClippingMode mode)
            : base(3, 7)
        {
            Mode = mode;
        }

        public ClippingMode Mode { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptControlLineClippingMode(this, parameter);
        }
    }
}
