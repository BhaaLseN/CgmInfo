using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class DisjointPolyline : Command
    {
        public DisjointPolyline(PointF[] points)
            : base(4, 2)
        {
            Points = points;
        }

        public PointF[] Points { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveDisjointPolyline(this, parameter);
        }
    }
}
