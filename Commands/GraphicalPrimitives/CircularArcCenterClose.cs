using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("ARCCTRCLOSE")]
    public class CircularArcCenterClose : Command
    {
        public CircularArcCenterClose(MetafilePoint center, MetafilePoint start, MetafilePoint end, double radius, ArcClosureType closure)
            : base(4, 16)
        {
            Center = center;
            Start = start;
            End = end;
            Radius = radius;
            Closure = closure;
        }

        public MetafilePoint Center { get; }
        public MetafilePoint Start { get; }
        public MetafilePoint End { get; }
        public double Radius { get; }
        public ArcClosureType Closure { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveCircularArcCenterClose(this, parameter);
        }
    }
}
