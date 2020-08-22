using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("MARKERCLIPMODE")]
    public class MarkerClippingMode : Command
    {
        public MarkerClippingMode(ClippingMode mode)
            : base(3, 8)
        {
            Mode = mode;
        }

        public ClippingMode Mode { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptControlMarkerClippingMode(this, parameter);
        }
    }
}
