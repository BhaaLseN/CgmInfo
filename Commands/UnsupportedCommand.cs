using CgmInfo.Traversal;

namespace CgmInfo.Commands
{
    public sealed class UnsupportedCommand : Command
    {
        public UnsupportedCommand(int elementClass, int elementId)
            : base(elementClass, elementId)
        {
        }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptUnsupportedCommand(this, parameter);
        }
    }
}
