using CgmInfo.Commands.Enums;

namespace CgmInfo
{
    public class MetafileDescriptor
    {
        public MetafileDescriptor()
        {
            // ISO/IEC 8632-3 9
            ColorModel = ColorModel.RGB;
            ColorPrecision = 8;
            RealPrecision = RealPrecisionSpecification.FixedPoint32Bit;
        }

        public ColorModel ColorModel { get; internal set; }
        public int ColorPrecision { get; internal set; }
        public RealPrecisionSpecification RealPrecision { get; internal set; }
    }
}
