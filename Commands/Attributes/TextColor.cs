using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("TEXTCOLR")]
    public class TextColor : Command
    {
        public TextColor(MetafileColor color)
            : base(5, 14)
        {
            Color = color;
        }

        public MetafileColor Color { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeTextColor(this, parameter);
        }
    }
}
