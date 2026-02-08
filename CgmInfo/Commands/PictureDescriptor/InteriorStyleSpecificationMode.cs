using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.PictureDescriptor
{
    [TextToken("INTSTYLEMODE")]
    public class InteriorStyleSpecificationMode : Command
    {
        public InteriorStyleSpecificationMode(WidthSpecificationModeType widthSpecificationMode)
            : base(2, 16)
        {
            WidthSpecificationMode = widthSpecificationMode;
        }

        public WidthSpecificationModeType WidthSpecificationMode { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptPictureDescriptorInteriorStyleSpecificationMode(this, parameter);
        }
    }
}
