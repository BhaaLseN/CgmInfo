using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
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

        public MetafilePoint Center { get; private set; }
        public MetafilePoint FirstConjugateDiameter { get; private set; }
        public MetafilePoint SecondConjugateDiameter { get; private set; }
        public MetafilePoint Start { get; private set; }
        public MetafilePoint End { get; private set; }
        public ArcClosureType Closure { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveEllipticalArcClose(this, parameter);
        }
    }
}
