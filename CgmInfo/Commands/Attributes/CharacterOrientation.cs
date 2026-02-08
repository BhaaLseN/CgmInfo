using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("CHARORI")]
    public class CharacterOrientation : Command
    {
        public CharacterOrientation(MetafilePoint up, MetafilePoint @base)
            : base(5, 16)
        {
            Base = @base;
            Up = up;
        }

        public MetafilePoint Up { get; }
        public MetafilePoint Base { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeCharacterOrientation(this, parameter);
        }
    }
}
