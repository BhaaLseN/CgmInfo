using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    public class FillBundleIndex : Command
    {
        public FillBundleIndex(int index)
            : base(5, 21)
        {
            Index = index;
        }

        public int Index { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeFillBundleIndex(this, parameter);
        }
    }
}
