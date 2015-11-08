using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    public class EdgeTypeInitialOffset : Command
    {
        public EdgeTypeInitialOffset(double offset)
            : base(5, 47)
        {
            Offset = offset;
        }

        public double Offset { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeEdgeTypeInitialOffset(this, parameter);
        }
    }
}
