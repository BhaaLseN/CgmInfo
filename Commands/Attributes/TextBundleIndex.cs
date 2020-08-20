using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("TEXTINDEX")]
    public class TextBundleIndex : Command
    {
        public TextBundleIndex(int index)
            : base(5, 9)
        {
            Index = index;
        }

        public int Index { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeTextBundleIndex(this, parameter);
        }
    }
}
