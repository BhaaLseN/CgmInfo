using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    public class LineBundleIndex : Command
    {
        public LineBundleIndex(int index)
            : base(5, 1)
        {
            Index = index;
        }

        public int Index { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeLineBundleIndex(this, parameter);
        }
    }
}
