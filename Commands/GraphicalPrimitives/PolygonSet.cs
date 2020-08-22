using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("POLYGONSET")]
    public class PolygonSet : Command
    {
        public PolygonSet(MetafilePoint[] points, EdgeOutFlags[] flags)
            : base(4, 8)
        {
            Points = points;
            Flags = flags;
        }

        public MetafilePoint[] Points { get; }
        public EdgeOutFlags[] Flags { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitivePolygonSet(this, parameter);
        }
    }
}
