using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class ParabolicArc : Command
    {
        public ParabolicArc(MetafilePoint tangentIntersectionPoint, MetafilePoint start, MetafilePoint end)
            : base(4, 23)
        {
            TangentIntersectionPoint = tangentIntersectionPoint;
            Start = start;
            End = end;
        }

        public MetafilePoint TangentIntersectionPoint { get; private set; }
        public MetafilePoint Start { get; private set; }
        public MetafilePoint End { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveParabolicArc(this, parameter);
        }
    }
}
