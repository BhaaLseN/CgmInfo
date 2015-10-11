using System.Drawing;
using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.PictureDescriptor
{
    public class HatchStyleDefinition : Command
    {
        public HatchStyleDefinition(int hatchIndex, HatchStyleIndicator styleIndicator, PointF hatchDirectionStart, PointF hatchDirectionEnd, double dutyCycleLength, int[] gapWidths, int[] lineTypes)
            : base(2, 18)
        {
            HatchIndex = hatchIndex;
            StyleIndicator = styleIndicator;
            HatchDirectionStart = hatchDirectionStart;
            HatchDirectionEnd = hatchDirectionEnd;
            DutyCycleLength = dutyCycleLength;
            GapWidths = gapWidths;
            LineTypes = lineTypes;
        }

        public int HatchIndex { get; private set; }
        public double DutyCycleLength { get; private set; }
        public int[] GapWidths { get; private set; }
        public int[] LineTypes { get; private set; }
        public PointF HatchDirectionStart { get; private set; }
        public PointF HatchDirectionEnd { get; private set; }
        public HatchStyleIndicator StyleIndicator { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptPictureDescriptorHatchStyleDefinition(this, parameter);
        }
    }
}
