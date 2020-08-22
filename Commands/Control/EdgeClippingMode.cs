using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("EDGECLIPMODE")]
    public class EdgeClippingMode : Command
    {
        public EdgeClippingMode(ClippingMode mode)
            : base(3, 9)
        {
            Mode = mode;
        }

        public ClippingMode Mode { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptControlEdgeClippingMode(this, parameter);
        }
    }
}
