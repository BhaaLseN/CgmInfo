using System.Drawing;
using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class CircularArcCenterClose : Command
    {
        public CircularArcCenterClose(PointF center, PointF start, PointF end, double radius, ArcClosureType closure)
            : base(4, 16)
        {
            Center = center;
            Start = start;
            End = end;
            Radius = radius;
            Closure = closure;
        }

        public PointF Center { get; private set; }
        public PointF Start { get; private set; }
        public PointF End { get; private set; }
        public double Radius { get; private set; }
        public ArcClosureType Closure { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveCircularArcCenterClose(this, parameter);
        }
    }
}
