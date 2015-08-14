using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    public class EndCompoundLine : Command
    {
        public EndCompoundLine()
            : base(0, 16)
        {
        }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterEndCompoundLine(this, parameter);
        }
    }
}
