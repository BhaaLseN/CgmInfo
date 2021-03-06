using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("MARKERCOLR")]
    public class MarkerColor : Command
    {
        public MarkerColor(MetafileColor color)
            : base(5, 8)
        {
            Color = color;
        }

        public MetafileColor Color { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeMarkerColor(this, parameter);
        }
    }
}
