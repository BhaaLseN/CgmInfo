using CgmInfo.Commands.MetafileDescriptor;

namespace CgmInfo.TextEncoding
{
    internal static class MetafileDescriptorReader
    {
        public static MetafileVersion MetafileVersion(MetafileReader reader)
        {
            return new MetafileVersion(reader.ReadInteger());
        }

        public static MetafileDescription MetafileDescription(MetafileReader reader)
        {
            return new MetafileDescription(reader.ReadString());
        }

        public static VdcType VdcType(MetafileReader reader)
        {
            return new VdcType(ParseVdcType(reader.ReadEnum()));
        }
        private static int ParseVdcType(string token)
        {
            // assume integers unless the value is real
            if (token.ToUpperInvariant() == "REAL")
                return 1;
            return 0;
        }

        public static IntegerPrecision IntegerPrecision(MetafileReader reader)
        {
            // min is either 0 or negative, so subtracting it from max gives us roughly the number of values possible
            int minValue = reader.ReadInteger();
            int maxValue = reader.ReadInteger();
            return new IntegerPrecision(GetBitPrecision(maxValue - minValue));
        }

        public static MaximumColorIndex MaximumColorIndex(MetafileReader reader)
        {
            return new MaximumColorIndex(reader.ReadInteger());
        }

        public static ColorModelCommand ColorModelCommand(MetafileReader reader)
        {
            // {1=RGB, 2=CIELAB, 3 = CIELUV, 4 = CMYK, 5 = RGB - related, > 5, reserved for registered values}
            return new ColorModelCommand(reader.ReadInteger());
        }

        // returns the amount of bits (multiples of a byte) required to store input
        private static int GetBitPrecision(int input)
        {
            if (input <= 0xFF)
                return 8;
            else if (input <= 0xFFFF)
                return 16;
            else if (input <= 0xFFFFFF)
                return 24;
            return 32;
        }
    }
}
