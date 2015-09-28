using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.PictureDescriptor
{
    public class DeviceViewport : Command
    {
        public DeviceViewport(PointF firstCorner, PointF secondCorner)
            : base(2, 8)
        {
            FirstCorner = firstCorner;
            SecondCorner = secondCorner;
        }

        public PointF FirstCorner { get; private set; }
        public PointF SecondCorner { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptPictureDescriptorDeviceViewport(this, parameter);
        }
    }
}
