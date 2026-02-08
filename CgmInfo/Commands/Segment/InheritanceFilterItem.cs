using CgmInfo.Commands.Enums;

namespace CgmInfo.Commands.Segment
{
    public class InheritanceFilterItem
    {
        public InheritanceFilterItem(InheritanceFilterDesignator designator, InheritanceFilterSetting setting)
        {
            Designator = designator;
            Setting = setting;
        }

        public InheritanceFilterDesignator Designator { get; }
        public InheritanceFilterSetting Setting { get; }
    }
}
