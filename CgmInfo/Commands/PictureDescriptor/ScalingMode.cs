using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.PictureDescriptor
{
    [TextToken("SCALEMODE")]
    public class ScalingMode : Command
    {
        public ScalingMode(ScalingModeType scalingMode, double metricScalingFactor)
            : base(2, 1)
        {
            ScalingModeType = scalingMode;
            MetricScalingFactor = metricScalingFactor;
        }

        public ScalingModeType ScalingModeType { get; }
        public double MetricScalingFactor { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptPictureDescriptorScalingMode(this, parameter);
        }
    }
}
