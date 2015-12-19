using System.Drawing;
using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class CircularArc3PointClose : Command
    {
        public CircularArc3PointClose(PointF start, PointF intermediate, PointF end, ArcClosureType closure)
            : base(4, 14)
        {
            Start = start;
            Intermediate = intermediate;
            End = end;
            Closure = closure;
        }

        public PointF Intermediate { get; private set; }
        public PointF Start { get; private set; }
        public PointF End { get; private set; }
        public ArcClosureType Closure { get; set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveCircularArc3PointClose(this, parameter);
        }
    }
}
