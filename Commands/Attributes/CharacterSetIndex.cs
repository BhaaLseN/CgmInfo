using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("CHARSETINDEX")]
    public class CharacterSetIndex : Command
    {
        public CharacterSetIndex(int index)
            : base(5, 19)
        {
            Index = index;
        }

        public int Index { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeCharacterSetIndex(this, parameter);
        }
    }
}
