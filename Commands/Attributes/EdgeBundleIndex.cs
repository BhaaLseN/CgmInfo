using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("EDGEINDEX")]
    public class EdgeBundleIndex : Command
    {
        public EdgeBundleIndex(int index)
            : base(5, 26)
        {
            Index = index;
        }

        public int Index { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeEdgeBundleIndex(this, parameter);
        }
    }
}
