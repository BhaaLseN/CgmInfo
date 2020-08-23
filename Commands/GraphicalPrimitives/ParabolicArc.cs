using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("PARABARC")]
    public class ParabolicArc : Command
    {
        public ParabolicArc(MetafilePoint tangentIntersectionPoint, MetafilePoint start, MetafilePoint end)
            : base(4, 23)
        {
            TangentIntersectionPoint = tangentIntersectionPoint;
            Start = start;
            End = end;
        }

        public MetafilePoint TangentIntersectionPoint { get; }
        public MetafilePoint Start { get; }
        public MetafilePoint End { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveParabolicArc(this, parameter);
        }
    }
}
