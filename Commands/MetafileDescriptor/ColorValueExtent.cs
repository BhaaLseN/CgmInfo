using System;
using System.Drawing;
using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class ColorValueExtent : Command
    {
        internal ColorValueExtent()
            : base(1, 10)
        {
        }
        public ColorValueExtent(ColorSpace colorSpace, Color minimum, Color maximum)
            : this()
        {
            if (colorSpace != ColorSpace.RGB && colorSpace != ColorSpace.CMYK)
                throw new ArgumentOutOfRangeException("colorSpace", colorSpace, "Color Space must be either RGB or CMYK for use with this constructor");
            ColorSpace = colorSpace;
            Minimum = minimum;
            Maximum = maximum;
        }
        public ColorValueExtent(ColorSpace colorSpace, double firstComponent, double secondComponent, double thirdComponent)
            : this()
        {
            if (colorSpace != ColorSpace.CIE)
                throw new ArgumentOutOfRangeException("colorSpace", colorSpace, "Color Space must be CIE for use with this constructor");
            ColorSpace = colorSpace;
            FirstComponent = firstComponent;
            SecondComponent = secondComponent;
            ThirdComponent = thirdComponent;
        }

        public ColorSpace ColorSpace { get; private set; }
        // for RGB and CMYK
        public Color Minimum { get; private set; }
        public Color Maximum { get; private set; }
        // for CIE*
        public double FirstComponent { get; set; }
        public double SecondComponent { get; set; }
        public double ThirdComponent { get; set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorColorValueExtent(this, parameter);
        }
    }
}
