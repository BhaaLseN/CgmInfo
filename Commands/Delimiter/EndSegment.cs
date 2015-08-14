using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    public class EndSegment : Command
    {
        public EndSegment()
            : base(0, 7)
        {
        }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterEndSegment(this, parameter);
        }
    }
}
