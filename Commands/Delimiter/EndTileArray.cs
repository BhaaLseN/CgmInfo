using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    public class EndTileArray : Command
    {
        public EndTileArray()
            : base(0, 20)
        {
        }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterEndTileArray(this, parameter);
        }
    }
}
