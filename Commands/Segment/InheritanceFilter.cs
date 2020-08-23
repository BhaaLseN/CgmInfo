using CgmInfo.Traversal;

namespace CgmInfo.Commands.Segment
{
    [TextToken("INHFILTER")]
    public class InheritanceFilter : Command
    {
        public InheritanceFilter(InheritanceFilterItem[] items)
            : base(8, 2)
        {
            Items = items;
        }

        public InheritanceFilterItem[] Items { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptSegmentInheritanceFilter(this, parameter);
        }
    }
}
