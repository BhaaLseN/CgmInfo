using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    [TextToken("BEGFIGURE")]
    public class BeginFigure : Command
    {
        public BeginFigure()
            : base(0, 8)
        {
        }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterBeginFigure(this, parameter);
        }
    }
}
