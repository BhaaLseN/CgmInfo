using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    [TextToken("BEGAPSBODY")]
    public class BeginApplicationStructureBody : Command
    {
        public BeginApplicationStructureBody()
            : base(0, 22)
        {
        }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterBeginApplicationStructureBody(this, parameter);
        }
    }
}
