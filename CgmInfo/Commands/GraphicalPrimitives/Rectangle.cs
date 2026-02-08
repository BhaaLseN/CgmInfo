using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("RECT")]
    public class Rectangle : Command
    {
        public Rectangle(MetafilePoint firstCorner, MetafilePoint secondCorner)
            : base(4, 11)
        {
            FirstCorner = firstCorner;
            SecondCorner = secondCorner;
        }

        public MetafilePoint FirstCorner { get; }
        public MetafilePoint SecondCorner { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveRectangle(this, parameter);
        }
    }
}
