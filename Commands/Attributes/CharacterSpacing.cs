using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    public class CharacterSpacing : Command
    {
        public CharacterSpacing(double additionalIntercharacterSpace)
            : base(5, 13)
        {
            AdditionalIntercharacterSpace = additionalIntercharacterSpace;
        }

        public double AdditionalIntercharacterSpace { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeCharacterSpacing(this, parameter);
        }
    }
}
