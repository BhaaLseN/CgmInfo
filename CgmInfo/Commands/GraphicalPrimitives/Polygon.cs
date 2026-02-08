using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("POLYGON")]
    public class Polygon : Command
    {
        public Polygon(MetafilePoint[] points)
            : base(4, 7)
        {
            Points = points;
        }

        public MetafilePoint[] Points { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitivePolygon(this, parameter);
        }
    }
}
