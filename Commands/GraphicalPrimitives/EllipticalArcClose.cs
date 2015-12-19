using System.Drawing;
using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class EllipticalArcClose : Command
    {
        public EllipticalArcClose(PointF center, PointF firstConjugateDiameter, PointF secondConjugateDiameter, PointF start, PointF end, ArcClosureType closure)
            : base(4, 19)
        {
            Center = center;
            FirstConjugateDiameter = firstConjugateDiameter;
            SecondConjugateDiameter = secondConjugateDiameter;
            Start = start;
            End = end;
            Closure = closure;
        }

        public PointF Center { get; private set; }
        public PointF FirstConjugateDiameter { get; private set; }
        public PointF SecondConjugateDiameter { get; private set; }
        public PointF Start { get; private set; }
        public PointF End { get; private set; }
        public ArcClosureType Closure { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveEllipticalArcClose(this, parameter);
        }
    }
}
