using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("EDGETYPEINITOFFSET")]
    public class EdgeTypeInitialOffset : Command
    {
        public EdgeTypeInitialOffset(double offset)
            : base(5, 47)
        {
            Offset = offset;
        }

        public double Offset { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeEdgeTypeInitialOffset(this, parameter);
        }
    }
}
