using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class Polymarker : Command
    {
        public Polymarker(PointF[] points)
            : base(4, 3)
        {
            Points = points;
        }

        public PointF[] Points { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitivePolymarker(this, parameter);
        }
    }
}
