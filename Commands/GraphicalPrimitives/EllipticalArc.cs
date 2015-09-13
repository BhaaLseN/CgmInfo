using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class EllipticalArc : Command
    {
        public EllipticalArc(PointF center, PointF firstConjugateDiameter, PointF secondConjugateDiameter, PointF start, PointF end)
            : base(4, 18)
        {
            Center = center;
            FirstConjugateDiameter = firstConjugateDiameter;
            SecondConjugateDiameter = secondConjugateDiameter;
            Start = start;
            End = end;
        }

        public PointF Center { get; private set; }
        public PointF FirstConjugateDiameter { get; private set; }
        public PointF SecondConjugateDiameter { get; private set; }
        public PointF Start { get; private set; }
        public PointF End { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveEllipticalArc(this, parameter);
        }
    }
}
