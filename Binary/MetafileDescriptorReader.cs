using System.Collections.Generic;
using System.Drawing;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.MetafileDescriptor;

namespace CgmInfo.Binary
{
    internal static class MetafileDescriptorReader
    {
        public static MetafileVersion MetafileVersion(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) metafile version number: valid values are 1, 2, 3, 4 [ISO/IEC 8632-3 8.3]
            return new MetafileVersion(reader.ReadInteger(commandHeader.ParameterListLength));
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
            return new VdcType(reader.ReadInteger(commandHeader.ParameterListLength));
        }

        public static IntegerPrecision IntegerPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) integer precision: valid values are 8, 16, 24 or 32 [ISO/IEC 8632-3 8.3]
            return new IntegerPrecision(reader.ReadInteger(commandHeader.ParameterListLength));
        }

        public static RealPrecision RealPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) form of representation for real values: valid values are [ISO/IEC 8632-3 8.3]
            //      0 floating point format
            //      1 fixed point format
            // P2: (integer) field width for exponent or whole part(including 1 bit for sign)
            // P3: (integer) field width for fraction or fractional part
            return new RealPrecision(reader.ReadInteger(2), reader.ReadInteger(2), reader.ReadInteger(2));
        }

        public static IndexPrecision IndexPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) Index precision: valid values are 8,16,24,32 [ISO/IEC 8632-3 8.3]
            return new IndexPrecision(reader.ReadInteger(commandHeader.ParameterListLength));
        }

        public static ColorPrecision ColorPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) Colour precision: valid values are 8,16,24,32 [ISO/IEC 8632-3 8.3]
            return new ColorPrecision(reader.ReadInteger(commandHeader.ParameterListLength));
        }

        public static ColorIndexPrecision ColorIndexPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) Colour index precision: valid values are 8,16,24,32 [ISO/IEC 8632-3 8.3]
            return new ColorIndexPrecision(reader.ReadInteger(commandHeader.ParameterListLength));
        }

        public static MaximumColorIndex MaximumColorIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (colour index) maximum colour index that may be encountered in the metafile. [ISO/IEC 8632-3 8.3]
            return new MaximumColorIndex(reader.ReadInteger(commandHeader.ParameterListLength));
        }

        public static ColorValueExtent ColorValueExtent(MetafileReader reader, CommandHeader commandHeader)
        {
            // If the model is RGB or CMYK, then 2 parameters: [ISO/IEC 8632-3 8.3]
            // P1: (direct colour value) minimum colour value
            // P2: (direct colour value) maximum colour value
            // If the model is CIELAB, CIELUV, or RGB-related then 3 parameters:
            // P1: (real) scale and offset pair for first component.
            // P2: (real) scale and offset pair for second component.
            // P3: (real) scale and offset pair for third component.
            ColorValueExtent result;
            if (reader.Descriptor.ColorModel == ColorModel.RGB)
            {
                int minR = reader.ReadColorValue();
                int minG = reader.ReadColorValue();
                int minB = reader.ReadColorValue();
                Color min = Color.FromArgb(minR, minG, minB);
                int maxR = reader.ReadColorValue();
                int maxG = reader.ReadColorValue();
                int maxB = reader.ReadColorValue();
                Color max = Color.FromArgb(maxR, maxG, maxB);
                result = new ColorValueExtent(ColorSpace.RGB, min, max);
            }
            else if (reader.Descriptor.ColorModel == ColorModel.CMYK)
            {
                int minC = reader.ReadColorValue();
                int minM = reader.ReadColorValue();
                int minY = reader.ReadColorValue();
                int minK = reader.ReadColorValue();
                Color min = ColorFromCMYK(minC, minM, minY, minK);
                int maxC = reader.ReadColorValue();
                int maxM = reader.ReadColorValue();
                int maxY = reader.ReadColorValue();
                int maxK = reader.ReadColorValue();
                Color max = ColorFromCMYK(maxC, maxM, maxY, maxK);
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
            //      > 5 reserved for registered values.
            return new ColorModelCommand(reader.ReadInteger(commandHeader.ParameterListLength));
        }

        public static NamePrecision NamePrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) name precision: valid values are 8, 16, 24 or 32 [ISO/IEC 8632-3 8.3]
            return new NamePrecision(reader.ReadInteger(commandHeader.ParameterListLength));
        }

        public static FontList ReadFontList(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1-Pn: (string fixed) n font names [ISO/IEC 8632-3 8.3]
            var fonts = new List<string>();
            while (reader.HasMoreData())
                fonts.Add(reader.ReadString());
            return new FontList(fonts);
        }

        private static Color ColorFromCMYK(int cyan, int magenta, int yellow, int black)
        {
            double c = cyan / 255.0;
            double m = magenta / 255.0;
            double y = yellow / 255.0;
            double k = black / 255.0;

            double r = c * (1.0 - k) + k;
            double g = m * (1.0 - k) + k;
            double b = y * (1.0 - k) + k;

            r = (1.0 - r) * 255.0 + 0.5;
            g = (1.0 - g) * 255.0 + 0.5;
            b = (1.0 - b) * 255.0 + 0.5;

            int red = (int)r;
            int green = (int)g;
            int blue = (int)b;

            return Color.FromArgb(red, green, blue);
        }
    }
}
