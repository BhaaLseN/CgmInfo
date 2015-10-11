using System.Collections.Generic;
using System.Drawing;
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

        public static DeviceViewportSpecificationMode DeviceViewportSpecificationMode(MetafileReader reader, CommandHeader header)
        {
            // P1: (enumerated) VC specifier: valid values are
            //      0 fraction of drawing surface
            //      1 millimetres with scale factor
            //      2 physical device coordinates
            // P2: (real) metric scale factor, ignored if P1=0 or P1=2
            //
            // This parameter is always encoded as floating point, regardless of the value of the fixed/floating flag of
            // REAL PRECISION. If a REAL PRECISION (floating, n, m) has preceded, then the precision used is n,m.
            // If a REAL PRECISION element for floating point has not preceded, then a default precision of 9,23 (32-bit
            // floating point) is used.
            int numFloatBytes = 32 / 8;
            if (reader.Descriptor.RealPrecision == RealPrecisionSpecification.FloatingPoint64Bit)
                numFloatBytes = 64 / 8;
            return new DeviceViewportSpecificationMode(reader.ReadEnum<DeviceViewportSpecificationModeType>(), reader.ReadFloatingPoint(numFloatBytes));
        }

        public static InteriorStyleSpecificationMode InteriorStyleSpecificationMode(MetafileReader reader, CommandHeader header)
        {
            // P1: (enumerated) valid values are
            //      0 absolute
            //      1 scaled
            //      2 fractional
            //      3 mm
            return new InteriorStyleSpecificationMode(reader.ReadEnum<WidthSpecificationModeType>());
        }

        public static LineAndEdgeTypeDefinition LineAndEdgeTypeDefinition(MetafileReader reader, CommandHeader header)
        {
            // P1: (index) line type, valid values are negative.
            // P2: (size specification) dash cycle repeat length: see Part 1, subclause 7.1 for its form.
            //      dash cycle repeat length is affected by LINE WIDTH SPECIFICATION MODE
            // P3-P(n+2): (integer) list of n dash elements
            int lineType = reader.ReadIndex();
            double dashCycleRepeatLength = reader.ReadSizeSpecification(reader.Descriptor.LineWidthSpecificationMode);
            var dashElements = new List<int>();
            while (reader.HasMoreData())
                dashElements.Add(reader.ReadInteger());
            return new LineAndEdgeTypeDefinition(lineType, dashCycleRepeatLength, dashElements.ToArray());
        }

        public static HatchStyleDefinition HatchStyleDefinition(MetafileReader reader, CommandHeader header)
        {
            // P1: (index) hatch index, valid values are negative.
            // P2: (enumerated) style indicator: valid values are
            //      0 parallel
            //      1 cross hatch
            // P3: (4(size specification)) hatch direction vectors specifier (x,y,x,y): see Part 1, subclause 7.1 for its form.
            //      hatch direction vectors specifier is affected by INTERIOR STYLE SPECIFICATION MODE
            // P4: (size specification) duty cycle length: see Part 1, subclause 7.1 for its form.
            //      duty cycle length is affected by INTERIOR STYLE SPECIFICATION MODE
            // P5: (integer) number of hatch lines (=n)
            // P6-P(5+n): (integers) list of n gap widths
            // P(6+n)-P(5+2n): (integers) list of n line types
            int hatchIndex = reader.ReadIndex();
            HatchStyleIndicator styleIndicator = reader.ReadEnum<HatchStyleIndicator>();
            double hatchDirectionStartX = reader.ReadSizeSpecification(reader.Descriptor.InteriorStyleSpecificationMode);
            double hatchDirectionStartY = reader.ReadSizeSpecification(reader.Descriptor.InteriorStyleSpecificationMode);
            double hatchDirectionEndX = reader.ReadSizeSpecification(reader.Descriptor.InteriorStyleSpecificationMode);
            double hatchDirectionEndY = reader.ReadSizeSpecification(reader.Descriptor.InteriorStyleSpecificationMode);
            double dutyCycleLength = reader.ReadSizeSpecification(reader.Descriptor.InteriorStyleSpecificationMode);
            int n = reader.ReadInteger();
            var gapWidths = new List<int>();
            for (int i = 0; i < n; i++)
                gapWidths.Add(reader.ReadInteger());
            var lineTypes = new List<int>();
            for (int i = 0; i < n; i++)
                lineTypes.Add(reader.ReadInteger());
            return new HatchStyleDefinition(hatchIndex, styleIndicator,
                new PointF((float)hatchDirectionStartX, (float)hatchDirectionStartY),
                new PointF((float)hatchDirectionEndX, (float)hatchDirectionEndY),
                dutyCycleLength, gapWidths.ToArray(), lineTypes.ToArray());
        }
    }
}
