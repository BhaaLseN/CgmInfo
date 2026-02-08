using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("NUB")]
    public class NonUniformBSpline : Command
    {
        public NonUniformBSpline(int splineOrder, MetafilePoint[] controlPoints, double[] knots, double start, double end)
            : base(4, 24)
        {
            SplineOrder = splineOrder;
            ControlPoints = controlPoints;
            Knots = knots;
            Start = start;
            End = end;
        }

        public int SplineOrder { get; }
        public MetafilePoint[] ControlPoints { get; }
        public double[] Knots { get; }
        public double Start { get; }
        public double End { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveNonUniformBSpline(this, parameter);
        }
    }
}
