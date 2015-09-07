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
    }
}
