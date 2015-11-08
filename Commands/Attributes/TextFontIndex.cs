using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    public class TextFontIndex : Command
    {
        public TextFontIndex(int index)
            : base(5, 10)
        {
            Index = index;
        }

        public int Index { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeTextFontIndex(this, parameter);
        }
    }
}
