using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.PictureDescriptor
{
    public class LineWidthSpecificationMode : Command
    {
        public LineWidthSpecificationMode(WidthSpecificationModeType widthSpecificationMode)
            : base(2, 3)
        {
            WidthSpecificationMode = widthSpecificationMode;
        }

        public WidthSpecificationModeType WidthSpecificationMode { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptPictureDescriptorLineWidthSpecificationMode(this, parameter);
        }
    }
}
