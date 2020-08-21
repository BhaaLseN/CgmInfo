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

        public MetafilePoint[] Points { get; private set; }
        public EdgeOutFlags[] Flags { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitivePolygonSet(this, parameter);
        }
    }
}
