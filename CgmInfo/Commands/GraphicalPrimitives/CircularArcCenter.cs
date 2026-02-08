using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("ARCCTR")]
    public class CircularArcCenter : Command
    {
        public CircularArcCenter(MetafilePoint center, MetafilePoint start, MetafilePoint end, double radius)
            : base(4, 15)
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
            visitor.AcceptGraphicalPrimitiveCircularArcCenter(this, parameter);
        }
    }
}
