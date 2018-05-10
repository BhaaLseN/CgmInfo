using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
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

        public MetafilePoint Center { get; private set; }
        public MetafilePoint Start { get; private set; }
        public MetafilePoint End { get; private set; }
        public double Radius { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveCircularArcCenter(this, parameter);
        }
    }
}
