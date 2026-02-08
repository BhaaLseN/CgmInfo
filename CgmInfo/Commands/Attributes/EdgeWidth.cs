using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("EDGEWIDTH")]
    public class EdgeWidth : Command
    {
        public EdgeWidth(double width)
            : base(5, 28)
        {
            Width = width;
        }

        public double Width { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeEdgeWidth(this, parameter);
        }
    }
}
