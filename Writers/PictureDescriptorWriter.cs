using CgmInfo.Commands;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.PictureDescriptor;

namespace CgmInfo.Writers
{
    // [ISO/IEC 8632-3 8.4]
    internal static class PictureDescriptorWriter
    {
        public static void ScalingMode(MetafileWriter writer, Command command)
        {
            var scalingMode = (ScalingMode)command;
            // P1: (enumerated) scaling mode: valid values are
            //      0 abstract scaling
            //      1 metric scaling
            writer.WriteEnum(scalingMode.ScalingModeType);
            // P2: (real) metric scaling factor, ignored if P1=0
            //
            // This parameter is always encoded as floating point, regardless of the value of the fixed/floating flag of
            // REAL PRECISION. If a REAL PRECISION (floating, n, m) has preceded, then the precision used is n,m.
            // If a REAL PRECISION element for floating point has not preceded, then a default precision of 9,23 (32-bit
            // floating point) is used.
            int precision = 32;
            if (writer.Descriptor.RealPrecision == RealPrecisionSpecification.FloatingPoint64Bit)
                precision = 64;
            writer.WriteFloatingPoint(scalingMode.MetricScalingFactor, precision);
        }

        public static void ColorSelectionMode(MetafileWriter writer, Command command)
        {
            var colorSelectionMode = (ColorSelectionMode)command;
            // P1: (enumerated) colour selection mode:
            //      0 indexed colour mode
            //      1 direct colour mode
            writer.WriteEnum(colorSelectionMode.ColorMode);

            writer.Descriptor.ColorSelectionMode = colorSelectionMode.ColorMode;
        }

        public static void LineWidthSpecificationMode(MetafileWriter writer, Command command)
        {
            var lineWidthSpecificationMode = (LineWidthSpecificationMode)command;
            //  P1: (enumerated) line width specification mode: valid values are
            //      0 absolute
            //      1 scaled
            //      2 fractional
            //      3 mm
            writer.WriteEnum(lineWidthSpecificationMode.WidthSpecificationMode);

            writer.Descriptor.LineWidthSpecificationMode = lineWidthSpecificationMode.WidthSpecificationMode;
        }

        public static void MarkerSizeSpecificationMode(MetafileWriter writer, Command command)
        {
            var markerSizeSpecificationMode = (MarkerSizeSpecificationMode)command;
            //  P1: (enumerated) marker size specification mode: valid values are
            //      0 absolute
            //      1 scaled
            //      2 fractional
            //      3 mm
            writer.WriteEnum(markerSizeSpecificationMode.WidthSpecificationMode);

            writer.Descriptor.MarkerSizeSpecificationMode = markerSizeSpecificationMode.WidthSpecificationMode;
        }

        public static void EdgeWidthSpecificationMode(MetafileWriter writer, Command command)
        {
            var edgeWidthSpecificationMode = (EdgeWidthSpecificationMode)command;
            //  P1: (enumerated) edge width specification mode: valid values are
            //      0 absolute
            //      1 scaled
            //      2 fractional
            //      3 mm
            writer.WriteEnum(edgeWidthSpecificationMode.WidthSpecificationMode);

            writer.Descriptor.EdgeWidthSpecificationMode = edgeWidthSpecificationMode.WidthSpecificationMode;
        }

        public static void VdcExtent(MetafileWriter writer, Command command)
        {
            var vdcExtent = (VdcExtent)command;
            // P1: (point) first corner
            writer.WritePoint(vdcExtent.FirstCorner);
            // P2: (point) second corner
            writer.WritePoint(vdcExtent.SecondCorner);
        }

        public static void BackgroundColor(MetafileWriter writer, Command command)
        {
            var backgroundColor = (BackgroundColor)command;
            // P1: (direct colour) background colour.
            writer.WriteDirectColor(backgroundColor.Color);
        }

        public static void DeviceViewport(MetafileWriter writer, Command command)
        {
            var deviceViewport = (DeviceViewport)command;
            // P1: (viewport point) first corner
            writer.WriteViewportPoint(deviceViewport.FirstCorner);
            // P2: (viewport point) second corner
            writer.WriteViewportPoint(deviceViewport.SecondCorner);
        }

        public static void DeviceViewportSpecificationMode(MetafileWriter writer, Command command)
        {
            var deviceViewportSpecificationMode = (DeviceViewportSpecificationMode)command;
            // P1: (enumerated) VC specifier: valid values are
            //      0 fraction of drawing surface
            //      1 millimetres with scale factor
            //      2 physical device coordinates
            writer.WriteEnum(deviceViewportSpecificationMode.SpecificationMode);
            // P2: (real) metric scale factor, ignored if P1=0 or P1=2
            //
            // This parameter is always encoded as floating point, regardless of the value of the fixed/floating flag of
            // REAL PRECISION. If a REAL PRECISION (floating, n, m) has preceded, then the precision used is n,m.
            // If a REAL PRECISION element for floating point has not preceded, then a default precision of 9,23 (32-bit
            // floating point) is used.
            int precision = 32;
            if (writer.Descriptor.RealPrecision == RealPrecisionSpecification.FloatingPoint64Bit)
                precision = 64;
            writer.WriteFloatingPoint(deviceViewportSpecificationMode.ScaleFactor, precision);

            writer.Descriptor.DeviceViewportSpecificationMode = deviceViewportSpecificationMode.SpecificationMode;
        }

        public static void InteriorStyleSpecificationMode(MetafileWriter writer, Command command)
        {
            var interiorStyleSpecificationMode = (InteriorStyleSpecificationMode)command;
            // P1: (enumerated) valid values are
            //      0 absolute
            //      1 scaled
            //      2 fractional
            //      3 mm
            writer.WriteEnum(interiorStyleSpecificationMode.WidthSpecificationMode);

            writer.Descriptor.InteriorStyleSpecificationMode = interiorStyleSpecificationMode.WidthSpecificationMode;
        }

        public static void LineAndEdgeTypeDefinition(MetafileWriter writer, Command command)
        {
            var lineAndEdgeTypeDefinition = (LineAndEdgeTypeDefinition)command;
            // P1: (index) line type, valid values are negative.
            writer.WriteIndex(lineAndEdgeTypeDefinition.LineType);
            // P2: (size specification) dash cycle repeat length: see Part 1, subclause 7.1 for its form.
            //      dash cycle repeat length is affected by LINE WIDTH SPECIFICATION MODE
            writer.WriteSizeSpecification(lineAndEdgeTypeDefinition.DashCycleRepeatLength, writer.Descriptor.LineWidthSpecificationMode);
            // P3-P(n+2): (integer) list of n dash elements
            foreach (int dashElement in lineAndEdgeTypeDefinition.DashElements)
                writer.WriteInteger(dashElement);
        }

        public static void HatchStyleDefinition(MetafileWriter writer, Command command)
        {
            var hatchStyleDefinition = (HatchStyleDefinition)command;
            // P1: (index) hatch index, valid values are negative.
            writer.WriteIndex(hatchStyleDefinition.HatchIndex);
            // P2: (enumerated) style indicator: valid values are
            //      0 parallel
            //      1 cross hatch
            writer.WriteEnum(hatchStyleDefinition.StyleIndicator);
            // P3: (4(size specification)) hatch direction vectors specifier (x,y,x,y): see Part 1, subclause 7.1 for its form.
            //      hatch direction vectors specifier is affected by INTERIOR STYLE SPECIFICATION MODE
            writer.WriteSizeSpecification(hatchStyleDefinition.HatchDirectionStart.X, writer.Descriptor.InteriorStyleSpecificationMode);
            writer.WriteSizeSpecification(hatchStyleDefinition.HatchDirectionStart.Y, writer.Descriptor.InteriorStyleSpecificationMode);
            writer.WriteSizeSpecification(hatchStyleDefinition.HatchDirectionEnd.X, writer.Descriptor.InteriorStyleSpecificationMode);
            writer.WriteSizeSpecification(hatchStyleDefinition.HatchDirectionEnd.Y, writer.Descriptor.InteriorStyleSpecificationMode);
            // P4: (size specification) duty cycle length: see Part 1, subclause 7.1 for its form.
            //      duty cycle length is affected by INTERIOR STYLE SPECIFICATION MODE
            writer.WriteSizeSpecification(hatchStyleDefinition.DutyCycleLength, writer.Descriptor.InteriorStyleSpecificationMode);
            // P5: (integer) number of hatch lines (=n)
            writer.WriteInteger(hatchStyleDefinition.LineTypes.Length);
            // P6-P(5+n): (integers) list of n gap widths
            foreach (int gapWidth in hatchStyleDefinition.GapWidths)
                writer.WriteInteger(gapWidth);
            // P(6+n)-P(5+2n): (integers) list of n line types
            foreach (int lineType in hatchStyleDefinition.LineTypes)
                writer.WriteInteger(lineType);
        }

        public static void GeometricPatternDefinition(MetafileWriter writer, Command command)
        {
            var geometricPatternDefinition = (GeometricPatternDefinition)command;
            // P1: (index) geometric pattern index
            writer.WriteIndex(geometricPatternDefinition.GeometricPatternIndex);
            // P2: (name) segment identifier
            writer.WriteName(geometricPatternDefinition.SegmentIdentifier);
            // P3: (point) first corner point
            writer.WritePoint(geometricPatternDefinition.FirstCorner);
            // P4: (point) second corner point
            writer.WritePoint(geometricPatternDefinition.SecondCorner);
        }
    }
}
