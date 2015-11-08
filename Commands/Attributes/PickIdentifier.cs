using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    public class PickIdentifier : Command
    {
        public PickIdentifier(int identifier)
            : base(5, 36)
        {
            Identifier = identifier;
        }

        public int Identifier { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributePickIdentifier(this, parameter);
        }
    }
}
