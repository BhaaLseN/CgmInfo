using CgmInfo.Commands.Enums;

namespace CgmInfo
{
    public class MetafileDescriptor
    {
        public MetafileDescriptor()
        {
            // ISO/IEC 8632-3 9
            // also ISO/IEC 8632-1 8
            ColorModel = ColorModel.RGB;
            ColorPrecision = 8;
            RealPrecision = RealPrecisionSpecification.FixedPoint32Bit;
            IntegerPrecision = 16;
            IndexPrecision = 16;
            VdcType = VdcTypeSpecification.Integer;
            VdcIntegerPrecision = 16;
            VdcRealPrecision = RealPrecisionSpecification.FixedPoint32Bit;
            ColorSelectionMode = ColorModeType.Indexed;

            LineWidthSpecificationMode = WidthSpecificationModeType.Scaled;
            MarkerSizeSpecificationMode = WidthSpecificationModeType.Scaled;
            EdgeWidthSpecificationMode = WidthSpecificationModeType.Scaled;
        }

        public ColorModel ColorModel { get; internal set; }
        public int ColorPrecision { get; internal set; }
        public RealPrecisionSpecification RealPrecision { get; internal set; }
        public int IntegerPrecision { get; internal set; }
        public int IndexPrecision { get; internal set; }

        public ColorModeType ColorSelectionMode { get; internal set; }

        public VdcTypeSpecification VdcType { get; internal set; }
        public int VdcIntegerPrecision { get; internal set; }
        public RealPrecisionSpecification VdcRealPrecision { get; internal set; }

        public WidthSpecificationModeType LineWidthSpecificationMode { get; internal set; }
        public WidthSpecificationModeType MarkerSizeSpecificationMode { get; internal set; }
        public WidthSpecificationModeType EdgeWidthSpecificationMode { get; internal set; }
    }
}
