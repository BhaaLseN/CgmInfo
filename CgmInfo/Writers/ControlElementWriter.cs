using CgmInfo.Commands;
using CgmInfo.Commands.MetafileDescriptor;

namespace CgmInfo.Writers
{
    // [ISO/IEC 8632-3 8.5]
    internal static class ControlElementWriter
    {
        public static void VdcIntegerPrecision(MetafileWriter writer, Command command)
        {
            var vdcIntegerPrecision = (VdcIntegerPrecision)command;
            // P1: (integer) VDC integer precision; legal values are 16, 24 or 32; the value 8 is not permitted.
            writer.WriteInteger(vdcIntegerPrecision.Precision);

            writer.Descriptor.VdcIntegerPrecision = vdcIntegerPrecision.Precision;
        }

        public static void VdcRealPrecision(MetafileWriter writer, Command command)
        {
            var vdcRealPrecision = (VdcRealPrecision)command;
            // P1: (enumerated) form of representation for real values: valid values are
            //      0 floating point format
            //      1 fixed point format
            writer.WriteEnum(vdcRealPrecision.RepresentationForm);
            // P2: (integer) field width for exponent or whole part (including 1 bit for sign)
            writer.WriteInteger(vdcRealPrecision.ExponentWidth);
            // P3: (integer) field width for fraction or fractional part
            writer.WriteInteger(vdcRealPrecision.FractionWidth);

            writer.Descriptor.VdcRealPrecision = vdcRealPrecision.Specification;
        }

        public static void AuxiliaryColor(MetafileWriter writer, Command command)
        {
            var auxiliaryColor = (AuxiliaryColor)command;
            // P1: (colour) auxiliary colour
            writer.WriteColor(auxiliaryColor.Color);
        }

        public static void Transparency(MetafileWriter writer, Command command)
        {
            var transparency = (Transparency)command;
            // P1: (enumerated) on - off indicator: valid values are
            //      0 off: auxiliary colour background is required
            //      1 on: transparent background is required
            writer.WriteEnum(transparency.Indicator);
        }

        public static void ClipRectangle(MetafileWriter writer, Command command)
        {
            var clipRectangle = (ClipRectangle)command;
            // P1: (point) first corner
            writer.WritePoint(clipRectangle.FirstCorner);
            // P2: (point) second corner
            writer.WritePoint(clipRectangle.SecondCorner);
        }

        public static void ClipIndicator(MetafileWriter writer, Command command)
        {
            var clipIndicator = (ClipIndicator)command;
            // P1: (enumerated) clip indicator: valid values are
            //      0 off
            //      1 on
            writer.WriteEnum(clipIndicator.Indicator);
        }

        public static void LineClippingMode(MetafileWriter writer, Command command)
        {
            var lineClippingMode = (LineClippingMode)command;
            // P1: (enumerated) clipping mode: valid values are
            //      0 locus
            //      1 shape
            //      2 locus then shape
            writer.WriteEnum(lineClippingMode.Mode);
        }

        public static void MarkerClippingMode(MetafileWriter writer, Command command)
        {
            var markerClippingMode = (MarkerClippingMode)command;
            // P1: (enumerated) clipping mode: valid values are
            //      0 locus
            //      1 shape
            //      2 locus then shape
            writer.WriteEnum(markerClippingMode.Mode);
        }

        public static void EdgeClippingMode(MetafileWriter writer, Command command)
        {
            var edgeClippingMode = (EdgeClippingMode)command;
            // P1: (enumerated) clipping mode: valid values are
            //      0 locus
            //      1 shape
            //      2 locus then shape
            writer.WriteEnum(edgeClippingMode.Mode);
        }

        public static void NewRegion(MetafileWriter writer, Command command)
        {
            // NEW REGION: has no parameters
        }

        public static void SavePrimitiveContext(MetafileWriter writer, Command command)
        {
            var savePrimitiveContext = (SavePrimitiveContext)command;
            // P1: (name) context name
            writer.WriteName(savePrimitiveContext.ContextName);
        }

        public static void RestorePrimitiveContext(MetafileWriter writer, Command command)
        {
            var restorePrimitiveContext = (RestorePrimitiveContext)command;
            // P1: (name) context name
            writer.WriteName(restorePrimitiveContext.ContextName);
        }

        public static void ProtectionRegionIndicator(MetafileWriter writer, Command command)
        {
            var protectionRegionIndicator = (ProtectionRegionIndicator)command;
            // P1: (index) region index
            writer.WriteIndex(protectionRegionIndicator.Index);
            // P2: (index) region indicator: valid values are
            //      1 off
            //      2 clip
            //      3 shield
            writer.WriteEnum(protectionRegionIndicator.Indicator);
        }

        public static void GeneralizedTextPathMode(MetafileWriter writer, Command command)
        {
            var generalizedTextPathMode = (GeneralizedTextPathMode)command;
            // P1: (enumerated) text path mode: valid values are
            //      0 off
            //      1 non - tangential
            //      2 axis - tangential
            writer.WriteEnum(generalizedTextPathMode.Mode);
        }

        public static void MiterLimit(MetafileWriter writer, Command command)
        {
            var miterLimit = (MiterLimit)command;
            // P1: (real) mitre limit
            writer.WriteReal(miterLimit.Limit);
        }
    }
}
