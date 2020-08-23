using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("PICKID")]
    public class PickIdentifier : Command
    {
        public PickIdentifier(int identifier)
            : base(5, 36)
        {
            Identifier = identifier;
        }

        public int Identifier { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributePickIdentifier(this, parameter);
        }
    }
}
