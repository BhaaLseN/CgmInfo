using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("HYPERBARC")]
    public class HyperbolicArc : Command
    {
        public HyperbolicArc(MetafilePoint center, MetafilePoint traverseRadiusEndPoint, MetafilePoint conjugateRadiusEndPoint, MetafilePoint start, MetafilePoint end)
            : base(4, 22)
        {
            Center = center;
            TraverseRadiusEndPoint = traverseRadiusEndPoint;
            ConjugateRadiusEndPoint = conjugateRadiusEndPoint;
            Start = start;
            End = end;
        }

        public MetafilePoint Center { get; private set; }
        public MetafilePoint TraverseRadiusEndPoint { get; private set; }
        public MetafilePoint ConjugateRadiusEndPoint { get; private set; }
        public MetafilePoint Start { get; private set; }
        public MetafilePoint End { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveHyperbolicArc(this, parameter);
        }
    }
}
