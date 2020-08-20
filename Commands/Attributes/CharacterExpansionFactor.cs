using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("CHAREXPAN")]
    public class CharacterExpansionFactor : Command
    {
        public CharacterExpansionFactor(double factor)
            : base(5, 12)
        {
            Factor = factor;
        }

        public double Factor { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeCharacterExpansionFactor(this, parameter);
        }
    }
}
