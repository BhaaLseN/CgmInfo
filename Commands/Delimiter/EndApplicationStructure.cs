using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    [TextToken("ENDAPS")]
    public class EndApplicationStructure : Command
    {
        public EndApplicationStructure()
            : base(0, 23)
        {
        }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterEndApplicationStructure(this, parameter);
        }
    }
}
