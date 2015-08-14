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
            IndexPrecision = 16;
            VdcType = VdcTypeSpecification.Integer;
            VdcIntegerPrecision = 16;
            VdcRealPrecision = RealPrecisionSpecification.FixedPoint32Bit;
        }

        public ColorModel ColorModel { get; internal set; }
        public int ColorPrecision { get; internal set; }
        public RealPrecisionSpecification RealPrecision { get; internal set; }
        public int IndexPrecision { get; internal set; }

        public VdcTypeSpecification VdcType { get; internal set; }
        public int VdcIntegerPrecision { get; internal set; }
        public RealPrecisionSpecification VdcRealPrecision { get; internal set; }
    }
}
