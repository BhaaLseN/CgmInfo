using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class Rectangle : Command
    {
        public Rectangle(MetafilePoint firstCorner, MetafilePoint secondCorner)
            : base(4, 11)
        {
            FirstCorner = firstCorner;
            SecondCorner = secondCorner;
        }

        public MetafilePoint FirstCorner { get; private set; }
        public MetafilePoint SecondCorner { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveRectangle(this, parameter);
        }
    }
}
