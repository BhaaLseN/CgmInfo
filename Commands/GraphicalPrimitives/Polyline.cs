using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class Polyline : Command
    {
        public Polyline(PointF[] points)
            : base(4, 1)
        {
            Points = points;
        }

        public PointF[] Points { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitivePolyline(this, parameter);
        }
    }
}
