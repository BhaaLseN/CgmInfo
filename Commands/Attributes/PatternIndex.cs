using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("PATINDEX")]
    public class PatternIndex : Command
    {
        public PatternIndex(int index)
            : base(5, 25)
        {
            Index = index;
        }

        public int Index { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributePatternIndex(this, parameter);
        }
    }
}
