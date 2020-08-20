using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("EDGEVIS")]
    public class EdgeVisibility : Command
    {
        public EdgeVisibility(OnOffIndicator visibility)
            : base(5, 30)
        {
            Visibility = visibility;
        }

        public OnOffIndicator Visibility { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeEdgeVisibility(this, parameter);
        }
    }
}
