using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class Circle : Command
    {
        public Circle(PointF center, double radius)
            : base(4, 12)
        {
            Center = center;
            Radius = radius;
        }

        public PointF Center { get; private set; }
        public double Radius { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveCircle(this, parameter);
        }
    }
}
