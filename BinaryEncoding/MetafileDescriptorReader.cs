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
            return new VdcType(reader.ReadEnum());
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
            return new RealPrecision(reader.ReadEnum(), reader.ReadInteger(), reader.ReadInteger());
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
            return new MaximumColorIndex(reader.ReadInteger(commandHeader.ParameterListLength));
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

        public static ColorModelCommand ColorModelCommand(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) colour model: valid values are [ISO/IEC 8632-3 8.3]
            //      1 RGB
            //      2 CIELAB
            //      3 CIELUV
            //      4 CMYK
            //      5 RGB - related
            //      > 5 reserved for registered values
            return new ColorModelCommand(reader.ReadInteger(commandHeader.ParameterListLength));
        }

        public static NamePrecision NamePrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) name precision: valid values are 8, 16, 24 or 32 [ISO/IEC 8632-3 8.3]
            return new NamePrecision(reader.ReadInteger());
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
            return new MaximumVdcExtent(reader.ReadVdc(), reader.ReadVdc(), reader.ReadVdc(), reader.ReadVdc());
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
            return new CharacterCodingAnnouncer(reader.ReadEnum());
        }
    }
}
