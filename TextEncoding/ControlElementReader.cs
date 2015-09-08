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
            // FIXME: implement COLOUR SELECTION MODE first; needs to select either color index or color value.
            throw new System.NotSupportedException("Requires COLOUR SELECTION MODE to be implemented");
            //return new AuxiliaryColor(reader.ReadColor());
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

        private static ClippingMode GetClippingMode(string token)
        {
            ClippingMode ret;
            if (!Enum.TryParse<ClippingMode>(token, true, out ret))
                ret = ClippingMode.Locus;
            return ret;
        }
    }
}
