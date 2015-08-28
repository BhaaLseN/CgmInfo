using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CgmInfo.Commands.Enums;
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
        private static VdcTypeSpecification ParseVdcType(string token)
        {
            // assume integers unless the value is real
            if (token.ToUpperInvariant() == "REAL")
                return VdcTypeSpecification.Real;
            return VdcTypeSpecification.Integer;
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

        public static ColorValueExtent ColorValueExtent(MetafileReader reader)
        {
            ColorValueExtent result;
            if (reader.Descriptor.ColorModel == ColorModel.RGB)
            {
                Color min = reader.ReadColor();
                Color max = reader.ReadColor();
                result = new ColorValueExtent(ColorSpace.RGB, min, max);
            }
            else if (reader.Descriptor.ColorModel == ColorModel.CMYK)
            {
                Color min = reader.ReadColor();
                Color max = reader.ReadColor();
                result = new ColorValueExtent(ColorSpace.CMYK, min, max);
            }
            else if (reader.Descriptor.ColorModel == ColorModel.CIELAB || reader.Descriptor.ColorModel == ColorModel.CIELUV || reader.Descriptor.ColorModel == ColorModel.RGBrelated)
            {
                double first = reader.ReadReal();
                double second = reader.ReadReal();
                double third = reader.ReadReal();
                result = new ColorValueExtent(ColorSpace.CIE, first, second, third);
            }
            else
            {
                // unsupported, just return a default unknown color space
                result = new ColorValueExtent();
            }

            return result;
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

        public static FontList FontList(MetafileReader reader)
        {
            return new FontList(reader.ReadToEndOfElement());
        }

        public static MaximumVdcExtent MaximumVdcExtent(MetafileReader reader)
        {
            var firstCorner = reader.ReadPoint();
            var secondCorner = reader.ReadPoint();
            return new MaximumVdcExtent(firstCorner.X, firstCorner.Y, secondCorner.X, secondCorner.Y);
        }

        public static CharacterSetList CharacterSetList(MetafileReader reader)
        {
            var tokens = reader.ReadToEndOfElement();
            var pairs = tokens
                .Select((t, i) => new { Token = t, Index = i })
                .GroupBy(a => a.Index / 2)
                .Where(g => g.Count() == 2) // drop half entries; which shouldn't be in there anyways
                .Select(g => new { CharacterSetType = g.First().Token, Tail = g.Skip(1).First().Token });

            var entries = new List<CharacterSetListEntry>();
            foreach (var pair in pairs)
            {
                entries.Add(new CharacterSetListEntry(ParseCharacterSetType(pair.CharacterSetType), pair.Tail));
            }
            return new CharacterSetList(entries);
        }
        private static CharacterSetType ParseCharacterSetType(string token)
        {
            token = token.ToUpperInvariant();
            // assume 94-character G-set by default; unless its one of the others
            if (token == "STD96")
                return CharacterSetType.GSet96Characters;
            else if (token == "STD94MULTIBYTE")
                return CharacterSetType.GSet94CharactersMultibyte;
            else if (token == "STD96MULTIBYTE")
                return CharacterSetType.GSet96CharactersMultibyte;
            else if (token == "COMPLETECODE")
                return CharacterSetType.CompleteCode;
            return CharacterSetType.GSet94Characters;
        }

        public static CharacterCodingAnnouncer CharacterCodingAnnouncer(MetafileReader reader)
        {
            return new CharacterCodingAnnouncer(ParseCharacterCodingAnnouncerType(reader.ReadEnum()));
        }
        private static int ParseCharacterCodingAnnouncerType(string token)
        {
            token = token.ToUpperInvariant();
            // assume basic 7-bit announcer, unless its one of the others
            if (token == "BASIC8BIT")
                return 1;
            else if (token == "EXTD7BIT")
                return 2;
            else if (token == "EXTD8BIT")
                return 3;
            return 0;
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
