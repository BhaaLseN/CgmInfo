using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class Polymarker : Command
    {
        public Polymarker(MetafilePoint[] points)
            : base(4, 3)
        {
            Points = points;
        }

        public MetafilePoint[] Points { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitivePolymarker(this, parameter);
        }
    }
}
