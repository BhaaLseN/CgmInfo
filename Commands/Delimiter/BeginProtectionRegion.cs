using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    public class BeginProtectionRegion : Command
    {
        public BeginProtectionRegion(int regionIndex)
            : base(0, 13)
        {
            RegionIndex = regionIndex;
        }

        public int RegionIndex { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterBeginProtectionRegion(this, parameter);
        }
    }
}
