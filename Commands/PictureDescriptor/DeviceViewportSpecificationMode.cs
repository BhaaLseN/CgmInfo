using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.PictureDescriptor
{
    [TextToken("DEVVPMODE")]
    public class DeviceViewportSpecificationMode : Command
    {
        public DeviceViewportSpecificationMode(DeviceViewportSpecificationModeType deviceViewportSpecificationMode, double scaleFactor)
            : base(2, 9)
        {
            SpecificationMode = deviceViewportSpecificationMode;
            ScaleFactor = scaleFactor;
        }

        public DeviceViewportSpecificationModeType SpecificationMode { get; }
        public double ScaleFactor { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptPictureDescriptorDeviceViewportSpecificationMode(this, parameter);
        }
    }
}
