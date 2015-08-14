using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    public class EndProtectionRegion : Command
    {
        public EndProtectionRegion()
            : base(0, 14)
        {
        }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterEndProtectionRegion(this, parameter);
        }
    }
}
