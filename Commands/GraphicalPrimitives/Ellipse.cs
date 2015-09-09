using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class Ellipse : Command
    {
        public Ellipse(PointF center, PointF firstConjugateDiameter, PointF secondConjugateDiameter)
            : base(4, 17)
        {
            Center = center;
            FirstConjugateDiameter = firstConjugateDiameter;
            SecondConjugateDiameter = secondConjugateDiameter;
        }

        public PointF Center { get; private set; }
        public PointF FirstConjugateDiameter { get; private set; }
        public PointF SecondConjugateDiameter { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveEllipse(this, parameter);
        }
    }
}
