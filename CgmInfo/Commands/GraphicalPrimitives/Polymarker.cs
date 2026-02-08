using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("MARKER")]
    public class Polymarker : Command
    {
        public Polymarker(MetafilePoint[] points)
            : base(4, 3)
        {
            Points = points;
        }

        public MetafilePoint[] Points { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitivePolymarker(this, parameter);
        }
    }
}
