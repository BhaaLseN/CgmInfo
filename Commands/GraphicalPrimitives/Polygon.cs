using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class Polygon : Command
    {
        public Polygon(PointF[] points)
            : base(4, 7)
        {
            Points = points;
        }

        public PointF[] Points { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitivePolygon(this, parameter);
        }
    }
}
