using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    public class EndPicture : Command
    {
        public EndPicture()
            : base(0, 5)
        {
        }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterEndPicture(this, parameter);
        }
    }
}
