using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class ParabolicArc : Command
    {
        public ParabolicArc(PointF tangentIntersectionPoint, PointF start, PointF end)
            : base(4, 23)
        {
            TangentIntersectionPoint = tangentIntersectionPoint;
            Start = start;
            End = end;
        }

        public PointF TangentIntersectionPoint { get; private set; }
        public PointF Start { get; private set; }
        public PointF End { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveParabolicArc(this, parameter);
        }
    }
}
