using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class ProtectionRegionIndicator : Command
    {
        public ProtectionRegionIndicator(int index, RegionIndicator indicator)
            : base(3, 17)
        {
            Indicator = indicator;
            Index = index;
        }

        public int Index { get; private set; }
        public RegionIndicator Indicator { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptControlProtectionRegionIndicator(this, parameter);
        }
    }
}
