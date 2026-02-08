using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("NURB")]
    public class NonUniformRationalBSpline : Command
    {
        public NonUniformRationalBSpline(int splineOrder, MetafilePoint[] controlPoints, double[] knots, double start, double end, double[] weights)
            : base(4, 25)
        {
            SplineOrder = splineOrder;
            ControlPoints = controlPoints;
            Knots = knots;
            Start = start;
            End = end;
            Weights = weights;
        }

        public int SplineOrder { get; }
        public MetafilePoint[] ControlPoints { get; }
        public double[] Knots { get; }
        public double Start { get; }
        public double End { get; }
        public double[] Weights { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveNonUniformRationalBSpline(this, parameter);
        }
    }
}
