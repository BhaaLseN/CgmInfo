using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    public class EndCompoundTextPath : Command
    {
        public EndCompoundTextPath()
            : base(0, 18)
        {
        }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterEndCompoundTextPath(this, parameter);
        }
    }
}
