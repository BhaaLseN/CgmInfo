using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.PictureDescriptor
{
    public class BackgroundColor : Command
    {
        public BackgroundColor(Color color)
            : base(2, 7)
        {
            Color = color;
        }

        public Color Color { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptPictureDescriptorBackgroundColor(this, parameter);
        }
    }
}
