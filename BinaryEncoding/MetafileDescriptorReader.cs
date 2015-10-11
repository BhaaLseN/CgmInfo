using System.Collections.Generic;
using System.Drawing;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.MetafileDescriptor;

namespace CgmInfo.BinaryEncoding
{
    internal static class MetafileDescriptorReader
    {
        public static MetafileVersion MetafileVersion(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) metafile version number: valid values are 1, 2, 3, 4 [ISO/IEC 8632-3 8.3]
            return new MetafileVersion(reader.ReadInteger());
        }

        public static MetafileDescription MetafileDescription(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (string fixed) metafile description string [ISO/IEC 8632-3 8.3]
            return new MetafileDescription(reader.ReadString());
        }

        public static VdcType VdcType(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) VDC TYPE: valid values are [ISO/IEC 8632-3 8.3]
            //      0 VDC values specified in integers
            //      1 VDC values specified in reals
            return new VdcType(reader.ReadEnum<VdcTypeSpecification>());
        }

        public static IntegerPrecision IntegerPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) integer precision: valid values are 8, 16, 24 or 32 [ISO/IEC 8632-3 8.3]
            return new IntegerPrecision(reader.ReadInteger());
        }

        public static RealPrecision RealPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) form of representation for real values: valid values are [ISO/IEC 8632-3 8.3]
            //      0 floating point format
            //      1 fixed point format
            // P2: (integer) field width for exponent or whole part(including 1 bit for sign)
            // P3: (integer) field width for fraction or fractional part
            return new RealPrecision(reader.ReadEnum<RealRepresentation>(), reader.ReadInteger(), reader.ReadInteger());
        }

        public static IndexPrecision IndexPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) Index precision: valid values are 8, 16, 24 or 32 [ISO/IEC 8632-3 8.3]
            return new IndexPrecision(reader.ReadInteger());
        }

        public static ColorPrecision ColorPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) Colour precision: valid values are 8, 16, 24 or 32 [ISO/IEC 8632-3 8.3]
            return new ColorPrecision(reader.ReadInteger());
        }

        public static ColorIndexPrecision ColorIndexPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) Colour index precision: valid values are 8, 16, 24 or 32 [ISO/IEC 8632-3 8.3]
            return new ColorIndexPrecision(reader.ReadInteger());
        }

        public static MaximumColorIndex MaximumColorIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (colour index) maximum colour index that may be encountered in the metafile [ISO/IEC 8632-3 8.3]
            return new MaximumColorIndex(reader.ReadInteger(commandHeader.ParameterListLength, true));
        }

        public static ColorValueExtent ColorValueExtent(MetafileReader reader, CommandHeader commandHeader)
        {
            // If the model is RGB or CMYK, then 2 parameters: [ISO/IEC 8632-3 8.3]
            // P1: (direct colour value) minimum colour value
            // P2: (direct colour value) maximum colour value
            // If the model is CIELAB, CIELUV, or RGB-related then 3 parameters:
            // P1: (real) scale and offset pair for first component
            // P2: (real) scale and offset pair for second component
            // P3: (real) scale and offset pair for third component
            ColorValueExtent result;
            if (reader.Descriptor.ColorModel == ColorModel.RGB)
            {
                Color min = reader.ReadDirectColor();
                Color max = reader.ReadDirectColor();
                result = new ColorValueExtent(ColorSpace.RGB, min, max);
            }
            else if (reader.Descriptor.ColorModel == ColorModel.CMYK)
            {
                Color min = reader.ReadDirectColor();
                Color max = reader.ReadDirectColor();
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

        public static ColorModelCommand ColorModelCommand(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) colour model: valid values are [ISO/IEC 8632-3 8.3]
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
            // P1: (integer) name precision: valid values are 8, 16, 24 or 32 [ISO/IEC 8632-3 8.3]
            return new NamePrecision(reader.ReadInteger());
        }

        public static MetafileElementsList MetafileElementsList(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) number of elements specified [ISO/IEC 8632-3 8.3]
            // P2: (index-pair array) List of metafile elements in this metafile. Each element is represented by two values:
            //      the first is its element class code (as in Table 2)
            //      the second is its element id code (as in Table 3 to Table 10).
            int numberOfElements = reader.ReadInteger(); // unused
            var elements = new List<string>();
            while (reader.HasMoreData())
            {
                int elementClass = reader.ReadIndex();
                int elementId = reader.ReadIndex();
                elements.Add(GetMetafileElementsListName(elementClass, elementId));
            }
            return new MetafileElementsList(elements);
        }

        public static FontList FontList(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1-Pn: (string fixed) n font names [ISO/IEC 8632-3 8.3]
            var fonts = new List<string>();
            while (reader.HasMoreData())
                fonts.Add(reader.ReadString());
            return new FontList(fonts);
        }

        public static MaximumVdcExtent MaximumVdcExtent(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) first corner [ISO/IEC 8632-3 8.3]
            // P2: (point) second corner
            return new MaximumVdcExtent(reader.ReadPoint(), reader.ReadPoint());
        }

        public static CharacterSetList CharacterSetList(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) CHARACTER SET TYPE: valid codes are [ISO/IEC 8632-3 8.3]
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
            // P1: (enumerated) character coding announcer: valid values are [ISO/IEC 8632-3 8.3]
            //      0 basic 7 - bit
            //      1 basic 8 - bit
            //      2 extended 7 - bit
            //      3 extended 8 - bit
            return new CharacterCodingAnnouncer(reader.ReadEnum<CharacterCodingAnnouncerType>());
        }

        // returns a readable name for the given class/id.
        // NOTE: only handles pseudo classes at this point; the rest seems to be uncommon.
        // TODO: move elsewhere if other types should be supported (both from binary and text encoding)
        private static string GetMetafileElementsListName(int elementClass, int elementId)
        {
            switch (elementClass)
            {
                case -1:
                    {
                        switch (elementId)
                        {
                            // drawing set: (-1,0)
                            case 0:
                                return "Drawing";
                            // drawing-plus-control set: (-1,1)
                            case 1:
                                return "Drawing+Control";
                            // version-2 set: (-1,2)
                            case 2:
                                return "Version 2";
                            // extended-primitives set: (-1,3)
                            case 3:
                                return "Extended Primitives";
                            // version-2-gksm set: (-1,4)
                            case 4:
                                return "Version 2 (GKSM)";
                            // version-3 set: (-1,5)
                            case 5:
                                return "Version 3";
                            // version-4 set: (-1,6)
                            case 6:
                                return "Version 4";
                        }
                        goto default;
                    }
                default:
                    return string.Format("Class {0}, Id {1}", elementClass, elementId);
            }
        }
    }
}
