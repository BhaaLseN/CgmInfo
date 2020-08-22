using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("FILLCOLR")]
    public class FillColor : Command
    {
        public FillColor(MetafileColor color)
            : base(5, 23)
        {
            Color = color;
        }

        public MetafileColor Color { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeFillColor(this, parameter);
        }
    }
}
