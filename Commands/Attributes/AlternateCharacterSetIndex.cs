using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("ALTCHARSETINDEX")]
    public class AlternateCharacterSetIndex : Command
    {
        public AlternateCharacterSetIndex(int index)
            : base(5, 20)
        {
            Index = index;
        }

        public int Index { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeAlternateCharacterSetIndex(this, parameter);
        }
    }
}
