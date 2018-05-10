using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
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

        public int SplineOrder { get; private set; }
        public MetafilePoint[] ControlPoints { get; private set; }
        public double[] Knots { get; private set; }
        public double Start { get; private set; }
        public double End { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveNonUniformBSpline(this, parameter);
        }
    }
}
