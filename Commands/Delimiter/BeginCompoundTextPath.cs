using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    public class BeginCompoundTextPath : Command
    {
        public BeginCompoundTextPath()
            : base(0, 17)
        {
        }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterBeginCompoundTextPath(this, parameter);
        }
    }
}
