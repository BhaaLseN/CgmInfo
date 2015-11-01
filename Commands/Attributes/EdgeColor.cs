using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.Attributes
{
    public class EdgeColor : Command
    {
        public EdgeColor(MetafileColor color)
            : base(5, 29)
        {
            Color = color;
        }

        public MetafileColor Color { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeEdgeColor(this, parameter);
        }
    }
}
