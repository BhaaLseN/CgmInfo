using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.PictureDescriptor
{
    [TextToken("MARKERSIZEMODE")]
    public class MarkerSizeSpecificationMode : Command
    {
        public MarkerSizeSpecificationMode(WidthSpecificationModeType widthSpecificationMode)
            : base(2, 4)
        {
            WidthSpecificationMode = widthSpecificationMode;
        }

        public WidthSpecificationModeType WidthSpecificationMode { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptPictureDescriptorMarkerSizeSpecificationMode(this, parameter);
        }
    }
}
