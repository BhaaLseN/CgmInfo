using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class HyperbolicArc : Command
    {
        public HyperbolicArc(PointF center, PointF traverseRadiusEndPoint, PointF conjugateRadiusEndPoint, PointF start, PointF end)
            : base(4, 22)
        {
            Center = center;
            TraverseRadiusEndPoint = traverseRadiusEndPoint;
            ConjugateRadiusEndPoint = conjugateRadiusEndPoint;
            Start = start;
            End = end;
        }

        public PointF Center { get; private set; }
        public PointF TraverseRadiusEndPoint { get; private set; }
        public PointF ConjugateRadiusEndPoint { get; private set; }
        public PointF Start { get; private set; }
        public PointF End { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveHyperbolicArc(this, parameter);
        }
    }
}
