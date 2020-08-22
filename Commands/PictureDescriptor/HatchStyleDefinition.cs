using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.PictureDescriptor
{
    [TextToken("HATCHSTYLEDEF")]
    public class HatchStyleDefinition : Command
    {
        public HatchStyleDefinition(int hatchIndex, HatchStyleIndicator styleIndicator, MetafilePoint hatchDirectionStart, MetafilePoint hatchDirectionEnd, double dutyCycleLength, int[] gapWidths, int[] lineTypes)
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

        public int HatchIndex { get; }
        public double DutyCycleLength { get; }
        public int[] GapWidths { get; }
        public int[] LineTypes { get; }
        public MetafilePoint HatchDirectionStart { get; }
        public MetafilePoint HatchDirectionEnd { get; }
        public HatchStyleIndicator StyleIndicator { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptPictureDescriptorHatchStyleDefinition(this, parameter);
        }
    }
}
