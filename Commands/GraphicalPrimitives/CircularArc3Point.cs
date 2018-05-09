using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class CircularArc3Point : Command
    {
        public CircularArc3Point(MetafilePoint start, MetafilePoint intermediate, MetafilePoint end)
            : base(4, 13)
        {
            Start = start;
            Intermediate = intermediate;
            End = end;
        }

        public MetafilePoint Intermediate { get; private set; }
        public MetafilePoint Start { get; private set; }
        public MetafilePoint End { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveCircularArc3Point(this, parameter);
        }
    }
}
