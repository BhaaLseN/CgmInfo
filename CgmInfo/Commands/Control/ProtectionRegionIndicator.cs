using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("PROTREGION")]
    public class ProtectionRegionIndicator : Command
    {
        public ProtectionRegionIndicator(int index, RegionIndicator indicator)
            : base(3, 17)
        {
            Indicator = indicator;
            Index = index;
        }

        public int Index { get; }
        public RegionIndicator Indicator { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptControlProtectionRegionIndicator(this, parameter);
        }
    }
}
