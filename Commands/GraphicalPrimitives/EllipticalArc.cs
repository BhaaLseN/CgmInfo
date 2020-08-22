using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("ELLIPARC")]
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

        public MetafilePoint Center { get; }
        public MetafilePoint FirstConjugateDiameter { get; }
        public MetafilePoint SecondConjugateDiameter { get; }
        public MetafilePoint Start { get; }
        public MetafilePoint End { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveEllipticalArc(this, parameter);
        }
    }
}
