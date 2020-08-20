using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("DISJTLINE")]
    public class DisjointPolyline : Command
    {
        public DisjointPolyline(MetafilePoint[] points)
            : base(4, 2)
        {
            Points = points;
        }

        public MetafilePoint[] Points { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveDisjointPolyline(this, parameter);
        }
    }
}
