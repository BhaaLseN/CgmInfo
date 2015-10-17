using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.PictureDescriptor
{
    public class BackgroundColor : Command
    {
        public BackgroundColor(MetafileColor color)
            : base(2, 7)
        {
            Color = color;
        }

        public MetafileColor Color { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptPictureDescriptorBackgroundColor(this, parameter);
        }
    }
}
