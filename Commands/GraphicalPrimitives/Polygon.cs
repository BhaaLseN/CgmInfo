using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class Polygon : Command
    {
        public Polygon(MetafilePoint[] points)
            : base(4, 7)
        {
            Points = points;
        }

        public MetafilePoint[] Points { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitivePolygon(this, parameter);
        }
    }
}
