using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.Attributes
{
    public class MarkerColor : Command
    {
        public MarkerColor(MetafileColor color)
            : base(5, 8)
        {
            Color = color;
        }

        public MetafileColor Color { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeMarkerColor(this, parameter);
        }
    }
}
