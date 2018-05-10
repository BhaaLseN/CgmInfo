using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class EllipticalArc : Command
    {
        public EllipticalArc(MetafilePoint center, MetafilePoint firstConjugateDiameter, MetafilePoint secondConjugateDiameter, MetafilePoint start, MetafilePoint end)
            : base(4, 18)
        {
            Center = center;
            FirstConjugateDiameter = firstConjugateDiameter;
            SecondConjugateDiameter = secondConjugateDiameter;
            Start = start;
            End = end;
        }

        public MetafilePoint Center { get; private set; }
        public MetafilePoint FirstConjugateDiameter { get; private set; }
        public MetafilePoint SecondConjugateDiameter { get; private set; }
        public MetafilePoint Start { get; private set; }
        public MetafilePoint End { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveEllipticalArc(this, parameter);
        }
    }
}
