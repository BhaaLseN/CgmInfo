using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    [TextToken("BEGSEG")]
    public class BeginSegment : Command
    {
        public BeginSegment(int identifier)
            : base(0, 6)
        {
            Identifier = identifier;
        }

        public int Identifier { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterBeginSegment(this, parameter);
        }
    }
}
