using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("TEXTFONTINDEX")]
    public class TextFontIndex : Command
    {
        public TextFontIndex(int index)
            : base(5, 10)
        {
            Index = index;
        }

        public int Index { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeTextFontIndex(this, parameter);
        }
    }
}
