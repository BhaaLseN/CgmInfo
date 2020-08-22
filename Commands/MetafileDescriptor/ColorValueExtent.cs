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

        public ColorSpace ColorSpace { get; private set; }
        // for RGB and CMYK
        public MetafileColor Minimum { get; private set; }
        public MetafileColor Maximum { get; private set; }
        // for CIE*
        public double FirstScale { get; set; }
        public double SecondScale { get; set; }
        public double ThirdScale { get; set; }
        public double FirstOffset { get; set; }
        public double SecondOffset { get; set; }
        public double ThirdOffset { get; set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorColorValueExtent(this, parameter);
        }
    }
}
