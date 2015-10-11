using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.PictureDescriptor
{
    public class ColorSelectionMode : Command
    {
        public ColorSelectionMode(ColorModeType colorMode)
            : base(2, 2)
        {
            ColorMode = colorMode;
        }

        public ColorModeType ColorMode { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptPictureDescriptorColorSelectionMode(this, parameter);
        }
    }
}
