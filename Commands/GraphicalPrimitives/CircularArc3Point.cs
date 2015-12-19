using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class CircularArc3Point : Command
    {
        public CircularArc3Point(PointF start, PointF intermediate, PointF end)
            : base(4, 13)
        {
            Start = start;
            Intermediate = intermediate;
            End = end;
        }

        public PointF Intermediate { get; private set; }
        public PointF Start { get; private set; }
        public PointF End { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveCircularArc3Point(this, parameter);
        }
    }
}
