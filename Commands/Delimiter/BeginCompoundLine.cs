using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    public class BeginCompoundLine : Command
    {
        public BeginCompoundLine()
            : base(0, 15)
        {
        }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterBeginCompoundLine(this, parameter);
        }
    }
}
