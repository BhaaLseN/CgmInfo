using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.Attributes
{
    public class CharacterOrientation : Command
    {
        public CharacterOrientation(MetafilePoint up, MetafilePoint @base)
            : base(5, 16)
        {
            Base = @base;
            Up = up;
        }

        public MetafilePoint Up { get; private set; }
        public MetafilePoint Base { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeCharacterOrientation(this, parameter);
        }
    }
}
