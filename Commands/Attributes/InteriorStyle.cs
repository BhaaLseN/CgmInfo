using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("INTSTYLE")]
    public class InteriorStyle : Command
    {
        public InteriorStyle(InteriorStyleType style)
            : base(5, 22)
        {
            Style = style;
        }

        public InteriorStyleType Style { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeInteriorStyle(this, parameter);
        }
    }
}
