using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
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

        public HorizontalTextAlignment Horizontal { get; private set; }
        public VerticalTextAlignment Vertical { get; private set; }
        public double HorizontalContinuousAlignment { get; private set; }
        public double VerticalContinuousAlignment { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeTextAlignment(this, parameter);
        }
    }
}
