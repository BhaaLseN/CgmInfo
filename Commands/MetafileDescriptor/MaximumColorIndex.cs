using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("MAXCOLRINDEX")]
    public class MaximumColorIndex : Command
    {
        public MaximumColorIndex(int index)
            : base(1, 9)
        {
            Index = index;
        }

        public int Index { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorMaximumColorIndex(this, parameter);
        }
    }
}
