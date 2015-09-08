using CgmInfo.Commands.Enums;
using CgmInfo.Commands.MetafileDescriptor;

namespace CgmInfo.BinaryEncoding
{
    internal static class ControlElementReader
    {
        public static VdcIntegerPrecision VdcIntegerPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) VDC integer precision; legal values are 16, 24 or 32; the value 8 is not permitted. [ISO/IEC 8632-3 8.5]
            return new VdcIntegerPrecision(reader.ReadInteger());
        }

        public static VdcRealPrecision VdcRealPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) form of representation for real values: valid values are [ISO/IEC 8632-3 8.5]
            //      0 floating point format
            //      1 fixed point format
            // P2: (integer) field width for exponent or whole part (including 1 bit for sign)
            // P3: (integer) field width for fraction or fractional part
            return new VdcRealPrecision(reader.ReadEnum<RealRepresentation>(), reader.ReadInteger(), reader.ReadInteger());
        }

        public static AuxiliaryColor AuxiliaryColor(MetafileReader reader, CommandHeader commandHeader)
        {
            // FIXME: implement COLOUR SELECTION MODE first; needs to select either color index or color value.
            throw new System.NotSupportedException("Requires COLOUR SELECTION MODE to be implemented");
            // P1: (colour) auxiliary colour [ISO/IEC 8632-3 8.5]
            //return new AuxiliaryColor(reader.ReadColor());
        }

        public static Transparency Transparency(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) on - off indicator: valid values are [ISO/IEC 8632-3 8.5]
            //      0 off: auxiliary colour background is required
            //      1 on: transparent background is required
            return new Transparency(reader.ReadEnum<OnOffIndicator>());
        }

        public static ClipRectangle ClipRectangle(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) first corner [ISO/IEC 8632-3 8.5]
            // P2: (point) second corner
            return new ClipRectangle(reader.ReadPoint(), reader.ReadPoint());
        }

        public static ClipIndicator ClipIndicator(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) clip indicator: valid values are [ISO/IEC 8632-3 8.5]
            //      0 off
            //      1 on
            return new ClipIndicator(reader.ReadEnum<OnOffIndicator>());
        }

        public static LineClippingMode LineClippingMode(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) clipping mode: valid values are [ISO/IEC 8632-3 8.5]
            //      0 locus
            //      1 shape
            //      2 locus then shape
            return new LineClippingMode(reader.ReadEnum<ClippingMode>());
        }

        public static MarkerClippingMode MarkerClippingMode(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) clipping mode: valid values are [ISO/IEC 8632-3 8.5]
            //      0 locus
            //      1 shape
            //      2 locus then shape
            return new MarkerClippingMode(reader.ReadEnum<ClippingMode>());
        }

        public static EdgeClippingMode EdgeClippingMode(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) clipping mode: valid values are [ISO/IEC 8632-3 8.5]
            //      0 locus
            //      1 shape
            //      2 locus then shape
            return new EdgeClippingMode(reader.ReadEnum<ClippingMode>());
        }

        public static NewRegion NewRegion(MetafileReader reader, CommandHeader commandHeader)
        {
            // NEW REGION: has no parameters [ISO/IEC 8632-3 8.5]
            return new NewRegion();
        }

        public static SavePrimitiveContext SavePrimitiveContext(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (name) context name [ISO/IEC 8632-3 8.5]
            return new SavePrimitiveContext(reader.ReadString());
        }

        public static RestorePrimitiveContext RestorePrimitiveContext(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (name) context name [ISO/IEC 8632-3 8.5]
            return new RestorePrimitiveContext(reader.ReadString());
        }
    }
}
