using CgmInfo.Commands.Enums;
using CgmInfo.Commands.PictureDescriptor;

namespace CgmInfo.BinaryEncoding
{
    // [ISO/IEC 8632-3 8.4]
    internal static class PictureDescriptorReader
    {
        public static ScalingMode ScalingMode(MetafileReader reader, CommandHeader header)
        {
            // P1: (enumerated) scaling mode: valid values are
            //      0 abstract scaling
            //      1 metric scaling
            // P2: (real) metric scaling factor, ignored if P1=0
            //
            // This parameter is always encoded as floating point, regardless of the value of the fixed/floating flag of
            // REAL PRECISION. If a REAL PRECISION (floating, n, m) has preceded, then the precision used is n,m.
            // If a REAL PRECISION element for floating point has not preceded, then a default precision of 9,23 (32-bit
            // floating point) is used.
            int numFloatBytes = 32 / 8;
            if (reader.Descriptor.RealPrecision == RealPrecisionSpecification.FloatingPoint64Bit)
                numFloatBytes = 64 / 8;
            return new ScalingMode(reader.ReadEnum<ScalingModeType>(), reader.ReadFloatingPoint(numFloatBytes));
        }

        public static ColorSelectionMode ColorSelectionMode(MetafileReader reader, CommandHeader header)
        {
            // P1: (enumerated) colour selection mode:
            //      0 indexed colour mode
            //      1 direct colour mode
            return new ColorSelectionMode(reader.ReadEnum<ColorModeType>());
        }

        public static LineWidthSpecificationMode LineWidthSpecificationMode(MetafileReader reader, CommandHeader header)
        {
            //  P1: (enumerated) line width specification mode: valid values are
            //      0 absolute
            //      1 scaled
            //      2 fractional
            //      3 mm
            return new LineWidthSpecificationMode(reader.ReadEnum<WidthSpecificationModeType>());
        }

        public static MarkerSizeSpecificationMode MarkerSizeSpecificationMode(MetafileReader reader, CommandHeader header)
        {
            //  P1: (enumerated) marker size specification mode: valid values are
            //      0 absolute
            //      1 scaled
            //      2 fractional
            //      3 mm
            return new MarkerSizeSpecificationMode(reader.ReadEnum<WidthSpecificationModeType>());
        }

        public static EdgeWidthSpecificationMode EdgeWidthSpecificationMode(MetafileReader reader, CommandHeader header)
        {
            //  P1: (enumerated) edge width specification mode: valid values are
            //      0 absolute
            //      1 scaled
            //      2 fractional
            //      3 mm
            return new EdgeWidthSpecificationMode(reader.ReadEnum<WidthSpecificationModeType>());
        }

        public static VdcExtent VdcExtent(MetafileReader reader, CommandHeader header)
        {
            // P1: (point) first corner
            // P2: (point) second corner
            var firstCorner = reader.ReadPoint();
            var secondCorner = reader.ReadPoint();
            return new VdcExtent(firstCorner, secondCorner);
        }

        public static BackgroundColor BackgroundColor(MetafileReader reader, CommandHeader header)
        {
            // P1: (direct colour) background colour.
            return new BackgroundColor(reader.ReadDirectColor());
        }

        public static DeviceViewport DeviceViewport(MetafileReader reader, CommandHeader header)
        {
            // P1: (viewport point) first corner
            // P2: (viewport point) second corner
            var firstCorner = reader.ReadViewportPoint();
            var secondCorner = reader.ReadViewportPoint();
            return new DeviceViewport(firstCorner, secondCorner);
        }
    }
}
