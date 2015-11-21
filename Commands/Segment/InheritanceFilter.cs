using CgmInfo.Traversal;

namespace CgmInfo.Commands.Segment
{
    public class InheritanceFilter : Command
    {
        public InheritanceFilter(InheritanceFilterItem[] items)
            : base(8, 2)
        {
            Items = items;
        }

        public InheritanceFilterItem[] Items { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptSegmentInheritanceFilter(this, parameter);
        }
    }
}
