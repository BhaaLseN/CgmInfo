using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    [TextToken("ENDFIGURE")]
    public class EndFigure : Command
    {
        public EndFigure()
            : base(0, 9)
        {
        }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterEndFigure(this, parameter);
        }
    }
}
