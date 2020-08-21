using System.Collections.Generic;
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
            return new IntegerPrecision(TextEncodingHelper.GetBitPrecision(reader.ReadInteger(), reader.ReadInteger()));
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

            return new RealPrecision(RealRepresentation.FloatingPoint, exponentWidth, fractionWidth);
        }

        public static IndexPrecision IndexPrecision(MetafileReader reader)
        {
            return new IndexPrecision(TextEncodingHelper.GetBitPrecision(reader.ReadInteger(), reader.ReadInteger()));
        }

        public static ColorPrecision ColorPrecision(MetafileReader reader)
        {
            return new ColorPrecision(TextEncodingHelper.GetBitPrecision(reader.ReadInteger()));
        }

        public static ColorIndexPrecision ColorIndexPrecision(MetafileReader reader)
        {
            return new ColorIndexPrecision(TextEncodingHelper.GetBitPrecision(reader.ReadInteger()));
        }

        public static MaximumColorIndex MaximumColorIndex(MetafileReader reader)
        {
            return new MaximumColorIndex(reader.ReadColorIndex());
        }

        public static ColorValueExtent ColorValueExtent(MetafileReader reader)
        {
            ColorValueExtent result;
            if (reader.Descriptor.ColorModel == ColorModel.RGB)
            {
                var min = reader.ReadDirectColor();
                var max = reader.ReadDirectColor();
                result = new ColorValueExtent(ColorSpace.RGB, min, max);
            }
            else if (reader.Descriptor.ColorModel == ColorModel.CMYK)
            {
                var min = reader.ReadDirectColor();
                var max = reader.ReadDirectColor();
                result = new ColorValueExtent(ColorSpace.CMYK, min, max);
            }
            else if (reader.Descriptor.ColorModel == ColorModel.CIELAB || reader.Descriptor.ColorModel == ColorModel.CIELUV || reader.Descriptor.ColorModel == ColorModel.RGBrelated)
            {
                double firstScale = reader.ReadReal();
                double firstOffset = reader.ReadReal();
                double secondScale = reader.ReadReal();
                double secondOffset = reader.ReadReal();
                double thirdScale = reader.ReadReal();
                double thirdOffset = reader.ReadReal();
                result = new ColorValueExtent(ColorSpace.CIE, firstScale, firstOffset, secondScale, secondOffset, thirdScale, thirdOffset);
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
            return new NamePrecision(TextEncodingHelper.GetBitPrecision(reader.ReadInteger(), reader.ReadInteger()));
        }

        public static MetafileElementsList MetafileElementsList(MetafileReader reader)
        {
            var elements = new List<MetafileElementsListElement>();
            while (reader.HasMoreData())
                elements.Add(MetafileElementsListElement.Parse(reader.ReadString()));
            return new MetafileElementsList(elements);
        }

        public static FontList FontList(MetafileReader reader)
        {
            var fonts = new List<string>();
            while (reader.HasMoreData())
                fonts.Add(reader.ReadString());
            return new FontList(fonts);
        }

        public static MaximumVdcExtent MaximumVdcExtent(MetafileReader reader)
        {
            var firstCorner = reader.ReadPoint();
            var secondCorner = reader.ReadPoint();
            return new MaximumVdcExtent(firstCorner, secondCorner);
        }

        public static SegmentPriorityExtent SegmentPriorityExtent(MetafileReader reader)
        {
            return new SegmentPriorityExtent(reader.ReadInteger(), reader.ReadInteger());
        }

        public static CharacterSetList CharacterSetList(MetafileReader reader)
        {
            var entries = new List<CharacterSetListEntry>();
            while (reader.HasMoreData(2))
                entries.Add(new CharacterSetListEntry(ParseCharacterSetType(reader.ReadEnum()), reader.ReadString()));
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
        private static CharacterCodingAnnouncerType ParseCharacterCodingAnnouncerType(string token)
        {
            token = token.ToUpperInvariant();
            // assume basic 7-bit announcer, unless its one of the others
            if (token == "BASIC8BIT")
                return CharacterCodingAnnouncerType.Basic8Bit;
            else if (token == "EXTD7BIT")
                return CharacterCodingAnnouncerType.Extended7Bit;
            else if (token == "EXTD8BIT")
                return CharacterCodingAnnouncerType.Extended8Bit;
            return CharacterCodingAnnouncerType.Basic7Bit;
        }

        public static FontProperties FontProperties(MetafileReader reader)
        {
            var properties = new List<FontProperty>();
            while (reader.HasMoreData())
            {
                int propertyIndicator = reader.ReadIndex();
                int priority = reader.ReadInteger();
                // The SDR for each of the standardized properties contains only one member (typed sequence) [ISO/IEC 8632-1 7.3.21]
                var record = reader.ReadStructuredDataRecord();
                properties.Add(new FontProperty(propertyIndicator, priority, record.Elements.First()));
            }
            return new FontProperties(properties.ToArray());
        }
    }
}
