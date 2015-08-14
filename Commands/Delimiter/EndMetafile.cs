using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    public class EndMetafile : Command
    {
        public EndMetafile()
            : base(0, 2)
        {
        }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterEndMetafile(this, parameter);
        }
    }
}
