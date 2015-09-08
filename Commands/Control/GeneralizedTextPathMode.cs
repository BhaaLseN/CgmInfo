using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class GeneralizedTextPathMode : Command
    {
        public GeneralizedTextPathMode(TextPathMode mode)
            : base(3, 18)
        {
            Mode = mode;
        }

        public TextPathMode Mode { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptControlGeneralizedTextPathMode(this, parameter);
        }
    }
}
