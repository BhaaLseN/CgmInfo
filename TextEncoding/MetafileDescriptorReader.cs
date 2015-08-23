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
            return new IntegerPrecision(GetBitPrecision(reader.ReadInteger(), reader.ReadInteger()));
        }

        public static RealPrecision RealPrecision(MetafileReader reader)
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

            // TODO: unless writing metafiles, we probably don't really care about the number of significant digits
            //       at least we don't for reading, and unless we should, we'll just ignore it here (intentionally unused)
            int significantDigits = reader.ReadInteger();

            return new RealPrecision(0, exponentWidth, fractionWidth);
        }

        public static IndexPrecision IndexPrecision(MetafileReader reader)
        {
            return new IndexPrecision(GetBitPrecision(reader.ReadInteger(), reader.ReadInteger()));
        }

        public static ColorPrecision ColorPrecision(MetafileReader reader)
        {
            return new ColorPrecision(GetBitPrecision(reader.ReadInteger()));
        }

        public static ColorIndexPrecision ColorIndexPrecision(MetafileReader reader)
        {
            return new ColorIndexPrecision(GetBitPrecision(reader.ReadInteger()));
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

        public static NamePrecision NamePrecision(MetafileReader reader)
        {
            return new NamePrecision(GetBitPrecision(reader.ReadInteger(), reader.ReadInteger()));
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
        private static int GetBitPrecision(int minValue, int maxValue)
        {
            // min is either 0 or negative, so subtracting it from max gives us roughly the number of values possible
            return GetBitPrecision(maxValue - minValue);
        }
    }
}
