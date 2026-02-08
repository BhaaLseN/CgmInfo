using CgmInfo.Traversal;

namespace CgmInfo.Commands.PictureDescriptor
{
    [TextToken("LINEEDGETYPEDEF")]
    public class LineAndEdgeTypeDefinition : Command
    {
        public LineAndEdgeTypeDefinition(int lineType, double dashCycleRepeatLength, int[] dashElements)
            : base(2, 17)
        {
            LineType = lineType;
            DashCycleRepeatLength = dashCycleRepeatLength;
            DashElements = dashElements;
        }

        public int LineType { get; }
        public double DashCycleRepeatLength { get; }
        public int[] DashElements { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptPictureDescriptorLineAndEdgeTypeDefinition(this, parameter);
        }
    }
}
