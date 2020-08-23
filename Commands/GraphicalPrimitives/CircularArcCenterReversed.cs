using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("ARCCTRREV")]
    public class CircularArcCenterReversed : Command
    {
        public CircularArcCenterReversed(MetafilePoint center, MetafilePoint start, MetafilePoint end, double radius)
            : base(4, 20)
        {
            Center = center;
            Start = start;
            End = end;
            Radius = radius;
        }

        public MetafilePoint Center { get; }
        public MetafilePoint Start { get; }
        public MetafilePoint End { get; }
        public double Radius { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveCircularArcCenterReversed(this, parameter);
        }
    }
}
