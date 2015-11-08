using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    public class TextPath : Command
    {
        public TextPath(TextPathType path)
            : base(5, 17)
        {
            Path = path;
        }

        public TextPathType Path { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeTextPath(this, parameter);
        }
    }
}
