using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("MARKERINDEX")]
    public class MarkerBundleIndex : Command
    {
        public MarkerBundleIndex(int index)
            : base(5, 5)
        {
            Index = index;
        }

        public int Index { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeMarkerBundleIndex(this, parameter);
        }
    }
}
