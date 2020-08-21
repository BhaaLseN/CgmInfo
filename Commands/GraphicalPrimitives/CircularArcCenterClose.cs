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

        public MetafilePoint Center { get; private set; }
        public MetafilePoint Start { get; private set; }
        public MetafilePoint End { get; private set; }
        public double Radius { get; private set; }
        public ArcClosureType Closure { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveCircularArcCenterClose(this, parameter);
        }
    }
}
