using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("ELLIPARCCLOSE")]
    public class EllipticalArcClose : Command
    {
        public EllipticalArcClose(MetafilePoint center, MetafilePoint firstConjugateDiameter, MetafilePoint secondConjugateDiameter, MetafilePoint start, MetafilePoint end, ArcClosureType closure)
            : base(4, 19)
        {
            Center = center;
            FirstConjugateDiameter = firstConjugateDiameter;
            SecondConjugateDiameter = secondConjugateDiameter;
            Start = start;
            End = end;
            Closure = closure;
        }

        public MetafilePoint Center { get; }
        public MetafilePoint FirstConjugateDiameter { get; }
        public MetafilePoint SecondConjugateDiameter { get; }
        public MetafilePoint Start { get; }
        public MetafilePoint End { get; }
        public ArcClosureType Closure { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveEllipticalArcClose(this, parameter);
        }
    }
}
