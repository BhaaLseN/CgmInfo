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

        public MetafilePoint Center { get; }
        public MetafilePoint TraverseRadiusEndPoint { get; }
        public MetafilePoint ConjugateRadiusEndPoint { get; }
        public MetafilePoint Start { get; }
        public MetafilePoint End { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveHyperbolicArc(this, parameter);
        }
    }
}
