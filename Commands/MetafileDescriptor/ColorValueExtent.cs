using System;
using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("COLRVALUEEXT")]
    public class ColorValueExtent : Command
    {
        internal ColorValueExtent()
            : base(1, 10)
        {
        }
        public ColorValueExtent(ColorSpace colorSpace, MetafileColor minimum, MetafileColor maximum)
            : this()
        {
            if (colorSpace != ColorSpace.RGB && colorSpace != ColorSpace.CMYK)
                throw new ArgumentOutOfRangeException(nameof(colorSpace), colorSpace, "Color Space must be either RGB or CMYK for use with this constructor");
            ColorSpace = colorSpace;
            Minimum = minimum;
            Maximum = maximum;
        }
        public ColorValueExtent(ColorSpace colorSpace,
            double firstScale, double firstOffset,
            double secondScale, double secondOffset,
            double thirdScale, double thirdOffset)
            : this()
        {
            if (colorSpace != ColorSpace.CIE)
                throw new ArgumentOutOfRangeException(nameof(colorSpace), colorSpace, "Color Space must be CIE for use with this constructor");
            ColorSpace = colorSpace;
            FirstScale = firstScale;
            SecondScale = secondScale;
            ThirdScale = thirdScale;
            FirstOffset = firstOffset;
            SecondOffset = secondOffset;
            ThirdOffset = thirdOffset;
        }

        public ColorSpace ColorSpace { get; }
        // for RGB and CMYK
        public MetafileColor Minimum { get; }
        public MetafileColor Maximum { get; }
        // for CIE*
        public double FirstScale { get; }
        public double SecondScale { get; }
        public double ThirdScale { get; }
        public double FirstOffset { get; }
        public double SecondOffset { get; }
        public double ThirdOffset { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorColorValueExtent(this, parameter);
        }
    }
}
