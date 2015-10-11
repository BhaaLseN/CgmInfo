using CgmInfo.Traversal;

namespace CgmInfo.Commands.PictureDescriptor
{
    public class LineAndEdgeTypeDefinition : Command
    {
        public LineAndEdgeTypeDefinition(int lineType, double dashCycleRepeatLength, int[] dashElements)
            : base(2, 17)
        {
            LineType = lineType;
            DashCycleRepeatLength = dashCycleRepeatLength;
            DashElements = dashElements;
        }

        public int LineType { get; private set; }
        public double DashCycleRepeatLength { get; private set; }
        public int[] DashElements { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptPictureDescriptorLineAndEdgeTypeDefinition(this, parameter);
        }
    }
}
