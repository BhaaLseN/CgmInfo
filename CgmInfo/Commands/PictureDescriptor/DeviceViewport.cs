using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.PictureDescriptor
{
    [TextToken("DEVVP")]
    public class DeviceViewport : Command
    {
        public DeviceViewport(MetafilePoint firstCorner, MetafilePoint secondCorner)
            : base(2, 8)
        {
            FirstCorner = firstCorner;
            SecondCorner = secondCorner;
        }

        public MetafilePoint FirstCorner { get; }
        public MetafilePoint SecondCorner { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptPictureDescriptorDeviceViewport(this, parameter);
        }
    }
}
