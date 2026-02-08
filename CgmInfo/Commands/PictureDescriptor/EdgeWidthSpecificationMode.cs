using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.PictureDescriptor
{
    [TextToken("EDGEWIDTHMODE")]
    public class EdgeWidthSpecificationMode : Command
    {
        public EdgeWidthSpecificationMode(WidthSpecificationModeType widthSpecificationMode)
            : base(2, 5)
        {
            WidthSpecificationMode = widthSpecificationMode;
        }

        public WidthSpecificationModeType WidthSpecificationMode { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptPictureDescriptorEdgeWidthSpecificationMode(this, parameter);
        }
    }
}
