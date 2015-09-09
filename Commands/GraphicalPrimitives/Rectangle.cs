using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class Rectangle : Command
    {
        public Rectangle(PointF firstCorner, PointF secondCorner)
            : base(4, 11)
        {
            FirstCorner = firstCorner;
            SecondCorner = secondCorner;
        }

        public PointF FirstCorner { get; private set; }
        public PointF SecondCorner { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveRectangle(this, parameter);
        }
    }
}
