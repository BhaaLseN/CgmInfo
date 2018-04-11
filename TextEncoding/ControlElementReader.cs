using System;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.MetafileDescriptor;

namespace CgmInfo.TextEncoding
{
    internal static class ControlElementReader
    {
        public static VdcIntegerPrecision VdcIntegerPrecision(MetafileReader reader)
        {
            return new VdcIntegerPrecision(TextEncodingHelper.GetBitPrecision(reader.ReadInteger(), reader.ReadInteger()));
        }

        public static VdcRealPrecision VdcRealPrecision(MetafileReader reader)
        {
            double minValue = reader.ReadReal();
            double maxValue = reader.ReadReal();

            // assume floating point; with their respective values from the binary encoding (also ANSI/IEEE 754 stuff)
            int exponentWidth = 12;
            int fractionWidth = 52;
            if ((float)minValue >= float.MinValue && (float)maxValue <= float.MaxValue)
            {
                exponentWidth = 9;
                fractionWidth = 23;
            }

            // TODO: same as MetafileDescriptorReader.RealPrecision
            int significantDigits = reader.ReadInteger();

            return new VdcRealPrecision(RealRepresentation.FloatingPoint, exponentWidth, fractionWidth);
        }

        public static AuxiliaryColor AuxiliaryColor(MetafileReader reader)
        {
            return new AuxiliaryColor(reader.ReadColor());
        }

        public static Transparency Transparency(MetafileReader reader)
        {
            return new Transparency(TextEncodingHelper.GetOnOffValue(reader.ReadEnum()));
        }

        public static ClipRectangle ClipRectangle(MetafileReader reader)
        {
            return new ClipRectangle(reader.ReadPoint(), reader.ReadPoint());
        }

        public static ClipIndicator ClipIndicator(MetafileReader reader)
        {
            return new ClipIndicator(TextEncodingHelper.GetOnOffValue(reader.ReadEnum()));
        }

        public static LineClippingMode LineClippingMode(MetafileReader reader)
        {
            return new LineClippingMode(GetClippingMode(reader.ReadEnum()));
        }

        public static MarkerClippingMode MarkerClippingMode(MetafileReader reader)
        {
            return new MarkerClippingMode(GetClippingMode(reader.ReadEnum()));
        }

        public static EdgeClippingMode EdgeClippingMode(MetafileReader reader)
        {
            return new EdgeClippingMode(GetClippingMode(reader.ReadEnum()));
        }

        public static NewRegion NewRegion(MetafileReader reader)
        {
            return new NewRegion();
        }

        public static SavePrimitiveContext SavePrimitiveContext(MetafileReader reader)
        {
            // TODO: spec says type "I", which is integer; even though there is a type for "N" (name)
            //       needs to be verified, I couldn't find any file that uses this yet.
            return new SavePrimitiveContext(reader.ReadName());
        }

        public static RestorePrimitiveContext RestorePrimitiveContext(MetafileReader reader)
        {
            // TODO: spec says type "I", which is integer; even though there is a type for "N" (name)
            //       needs to be verified, I couldn't find any file that uses this yet.
            return new RestorePrimitiveContext(reader.ReadName());
        }

        public static ProtectionRegionIndicator ProtectionRegionIndicator(MetafileReader reader)
        {
            // this one uses an index/integer type to store the enum, not a string like the others [ISO/IEC 8632-4 7.4]
            return new ProtectionRegionIndicator(reader.ReadIndex(), (RegionIndicator)reader.ReadIndex());
        }

        public static GeneralizedTextPathMode GeneralizedTextPathMode(MetafileReader reader)
        {
            return new GeneralizedTextPathMode(GetTextPathMode(reader.ReadEnum()));
        }

        public static MiterLimit MiterLimit(MetafileReader reader)
        {
            return new MiterLimit(reader.ReadReal());
        }

        private static TextPathMode GetTextPathMode(string token)
        {
            token = token.ToUpperInvariant();
            if (token == "AXIS")
                return TextPathMode.AxisTangential;
            if (token == "NONAXIS")
                return TextPathMode.NonTangential;
            return TextPathMode.Off;
        }
        private static ClippingMode GetClippingMode(string token)
        {
            if (!Enum.TryParse<ClippingMode>(token, true, out var ret))
                ret = ClippingMode.Locus;
            return ret;
        }
    }
}
