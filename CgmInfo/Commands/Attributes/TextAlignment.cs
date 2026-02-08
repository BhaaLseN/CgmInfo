using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("TEXTALIGN")]
    public class TextAlignment : Command
    {
        public TextAlignment(HorizontalTextAlignment horizontal, VerticalTextAlignment vertical, double horizontalContinuousAlignment, double verticalContinuousAlignment)
            : base(5, 18)
        {
            Horizontal = horizontal;
            Vertical = vertical;
            HorizontalContinuousAlignment = horizontalContinuousAlignment;
            VerticalContinuousAlignment = verticalContinuousAlignment;
        }

        public HorizontalTextAlignment Horizontal { get; }
        public VerticalTextAlignment Vertical { get; }
        public double HorizontalContinuousAlignment { get; }
        public double VerticalContinuousAlignment { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeTextAlignment(this, parameter);
        }
    }
}
