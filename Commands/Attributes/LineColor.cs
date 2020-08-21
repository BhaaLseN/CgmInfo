using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("LINECOLR")]
    public class LineColor : Command
    {
        public LineColor(MetafileColor color)
            : base(5, 4)
        {
            Color = color;
        }

        public MetafileColor Color { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeLineColor(this, parameter);
        }
    }
}
