using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class Polyline : Command
    {
        public Polyline(MetafilePoint[] points)
            : base(4, 1)
        {
            Points = points;
        }

        public MetafilePoint[] Points { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitivePolyline(this, parameter);
        }
    }
}
