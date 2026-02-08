using System.Collections.Generic;
using System.Linq;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.MetafileDescriptor;

namespace CgmInfo.BinaryEncoding
{
    // [ISO/IEC 8632-3 8.3]
    internal static class MetafileDescriptorReader
    {
        public static MetafileVersion MetafileVersion(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) metafile version number: valid values are 1, 2, 3, 4
            return new MetafileVersion(reader.ReadInteger());
        }

        public static MetafileDescription MetafileDescription(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (string fixed) metafile description string
            return new MetafileDescription(reader.ReadString());
        }

        public static VdcType VdcType(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) VDC TYPE: valid values are
            //      0 VDC values specified in integers
            //      1 VDC values specified in reals
            return new VdcType(reader.ReadEnum<VdcTypeSpecification>());
        }

        public static IntegerPrecision IntegerPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) integer precision: valid values are 8, 16, 24 or 32
            return new IntegerPrecision(reader.ReadInteger());
        }

        public static RealPrecision RealPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) form of representation for real values: valid values are
            //      0 floating point format
            //      1 fixed point format
            // P2: (integer) field width for exponent or whole part(including 1 bit for sign)
            // P3: (integer) field width for fraction or fractional part
            return new RealPrecision(reader.ReadEnum<RealRepresentation>(), reader.ReadInteger(), reader.ReadInteger());
        }

        public static IndexPrecision IndexPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) Index precision: valid values are 8, 16, 24 or 32
            return new IndexPrecision(reader.ReadInteger());
        }

        public static ColorPrecision ColorPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) Colour precision: valid values are 8, 16, 24 or 32
            return new ColorPrecision(reader.ReadInteger());
        }

        public static ColorIndexPrecision ColorIndexPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) Colour index precision: valid values are 8, 16, 24 or 32
            return new ColorIndexPrecision(reader.ReadInteger());
        }

        public static MaximumColorIndex MaximumColorIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (colour index) maximum colour index that may be encountered in the metafile
            return new MaximumColorIndex(reader.ReadColorIndex());
        }

        public static ColorValueExtent ColorValueExtent(MetafileReader reader, CommandHeader commandHeader)
        {
            // If the model is RGB or CMYK, then 2 parameters:
            // P1: (direct colour value) minimum colour value
            // P2: (direct colour value) maximum colour value
            // If the model is CIELAB, CIELUV, or RGB-related then 3 parameters:
            // P1: (real) scale and offset pair for first component
            // P2: (real) scale and offset pair for second component
            // P3: (real) scale and offset pair for third component
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

        public static ColorModelCommand ColorModelCommand(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) colour model: valid values are
            //      1 RGB
            //      2 CIELAB
            //      3 CIELUV
            //      4 CMYK
            //      5 RGB - related
            //      > 5 reserved for registered values
            return new ColorModelCommand(reader.ReadIndex());
        }

        public static NamePrecision NamePrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) name precision: valid values are 8, 16, 24 or 32
            return new NamePrecision(reader.ReadInteger());
        }

        public static MetafileElementsList MetafileElementsList(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) number of elements specified
            // P2: (index-pair array) List of metafile elements in this metafile. Each element is represented by two values:
            //      the first is its element class code (as in Table 2)
            //      the second is its element id code (as in Table 3 to Table 10).
            int numberOfElements = reader.ReadInteger(); // unused
            var elements = new List<MetafileElementsListElement>();
            while (reader.HasMoreData())
            {
                int elementClass = reader.ReadIndex();
                int elementId = reader.ReadIndex();
                elements.Add(new MetafileElementsListElement(elementClass, elementId));
            }
            return new MetafileElementsList(elements);
        }

        public static FontList FontList(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1-Pn: (string fixed) n font names
            var fonts = new List<string>();
            while (reader.HasMoreData())
                fonts.Add(reader.ReadString());
            return new FontList(fonts);
        }

        public static MaximumVdcExtent MaximumVdcExtent(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) first corner
            // P2: (point) second corner
            return new MaximumVdcExtent(reader.ReadPoint(), reader.ReadPoint());
        }

        public static SegmentPriorityExtent SegmentPriorityExtent(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) minimum segment priority value: valid values are non-negative integers
            // P2: (integer) maximum segment priority value: valid values are non-negative integers
            return new SegmentPriorityExtent(reader.ReadInteger(), reader.ReadInteger());
        }

        public static CharacterSetList CharacterSetList(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) CHARACTER SET TYPE: valid codes are
            //      0 94 - character G - set
            //      1 96 - character G - set
            //      2 94 - character multibyte G-set
            //      3 96 - character multibyte G-set
            //      4 complete code
            // P2: (string fixed) Designation sequence tail; see Part 1, subclause 7.3.14.
            var entries = new List<CharacterSetListEntry>();
            while (reader.HasMoreData(3)) // enums take up 2 bytes, strings at least 1 byte
            {
                entries.Add(new CharacterSetListEntry(reader.ReadEnum<CharacterSetType>(), reader.ReadString()));
            }
            return new CharacterSetList(entries);
        }

        public static CharacterCodingAnnouncer CharacterCodingAnnouncer(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) character coding announcer: valid values are
            //      0 basic 7 - bit
            //      1 basic 8 - bit
            //      2 extended 7 - bit
            //      3 extended 8 - bit
            return new CharacterCodingAnnouncer(reader.ReadEnum<CharacterCodingAnnouncerType>());
        }

        public static FontProperties FontProperties(MetafileReader reader, CommandHeader commandHeader)
        {
            // FONT PROPERTIES: has a variable number of parameter 3-tuples (P1,P2,P3); each parameter 3-tuple contains
            // P1: (index) property indicator, valid values are
            //      1 font index
            //      2 standard version
            //      3 design source
            //      4 font family
            //      5 posture
            //      6 weight
            //      7 proportionate width
            //      8 included glyph collections
            //      9 included glyphs
            //      10 design size
            //      11 minimum size
            //      12 maximum size
            //      13 design group
            //      14 structure
            //      >14 reserved for registered values
            // P2: (integer) priority, valid values are non-negative integers.
            // P3: (structured data record) property value record, each record contains a single member and is comprised of
            // [data type indicator, data element count, data element(s)].
            var properties = new List<FontProperty>();
            while (reader.HasMoreData((reader.Descriptor.IndexPrecision + reader.Descriptor.IntegerPrecision) / 8))
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
