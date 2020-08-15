using System;
using System.Linq;
using CgmInfo.Commands;
using CgmInfo.Commands.MetafileDescriptor;

namespace CgmInfo.Writers
{
    // [ISO/IEC 8632-3 8.3]
    internal static class MetafileDescriptorWriter
    {
        public static void MetafileVersion(MetafileWriter writer, Command command)
        {
            var metafileVersion = (MetafileVersion)command;
            // P1: (integer) metafile version number: valid values are 1, 2, 3, 4
            writer.WriteInteger(metafileVersion.Version);
        }

        public static void MetafileDescription(MetafileWriter writer, Command command)
        {
            var metafileDescription = (MetafileDescription)command;
            // P1: (string fixed) metafile description string
            writer.WriteString(metafileDescription.Description);
        }

        public static void VdcType(MetafileWriter writer, Command command)
        {
            var vdcType = (VdcType)command;
            // P1: (enumerated) VDC TYPE: valid values are
            //      0 VDC values specified in integers
            //      1 VDC values specified in reals
            writer.WriteEnum(vdcType.Specification);

            writer.Descriptor.VdcType = vdcType.Specification;
        }

        public static void IntegerPrecision(MetafileWriter writer, Command command)
        {
            var integerPrecision = (IntegerPrecision)command;
            // P1: (integer) integer precision: valid values are 8, 16, 24 or 32
            writer.WriteInteger(integerPrecision.Precision);

            writer.Descriptor.IntegerPrecision = integerPrecision.Precision;
        }

        public static void RealPrecision(MetafileWriter writer, Command command)
        {
            var realPrecision = (RealPrecision)command;
            // P1: (enumerated) form of representation for real values: valid values are
            //      0 floating point format
            //      1 fixed point format
            writer.WriteEnum(realPrecision.RepresentationForm);
            // P2: (integer) field width for exponent or whole part(including 1 bit for sign)
            writer.WriteInteger(realPrecision.ExponentWidth);
            // P3: (integer) field width for fraction or fractional part
            writer.WriteInteger(realPrecision.FractionWidth);

            writer.Descriptor.RealPrecision = realPrecision.Specification;
        }

        public static void IndexPrecision(MetafileWriter writer, Command command)
        {
            var indexPrecision = (IndexPrecision)command;
            // P1: (integer) Index precision: valid values are 8, 16, 24 or 32
            writer.WriteInteger(indexPrecision.Precision);

            writer.Descriptor.IndexPrecision = indexPrecision.Precision;
        }

        public static void ColorPrecision(MetafileWriter writer, Command command)
        {
            var colorPrecision = (ColorPrecision)command;
            // P1: (integer) Colour precision: valid values are 8, 16, 24 or 32
            writer.WriteInteger(colorPrecision.Precision);

            writer.Descriptor.ColorPrecision = colorPrecision.Precision;
        }

        public static void ColorIndexPrecision(MetafileWriter writer, Command command)
        {
            var colorIndexPrecision = (ColorIndexPrecision)command;
            // P1: (integer) Colour index precision: valid values are 8, 16, 24 or 32
            writer.WriteInteger(colorIndexPrecision.Precision);

            writer.Descriptor.ColorIndexPrecision = colorIndexPrecision.Precision;
        }

        public static void MaximumColorIndex(MetafileWriter writer, Command command)
        {
            var maximumColorIndex = (MaximumColorIndex)command;
            // P1: (colour index) maximum colour index that may be encountered in the metafile
            writer.WriteColorIndex(maximumColorIndex.Index);
        }

        public static void ColorValueExtent(MetafileWriter writer, Command command)
        {
            var colorValueExtent = (ColorValueExtent)command;
            if (writer.Descriptor.ColorModel == Commands.Enums.ColorModel.RGB || writer.Descriptor.ColorModel == Commands.Enums.ColorModel.CMYK)
            {
                // If the model is RGB or CMYK, then 2 parameters:
                // P1: (direct colour value) minimum colour value
                writer.WriteDirectColor(colorValueExtent.Minimum);
                // P2: (direct colour value) maximum colour value
                writer.WriteDirectColor(colorValueExtent.Maximum);
            }
            else if (writer.Descriptor.ColorModel == Commands.Enums.ColorModel.CIELAB || writer.Descriptor.ColorModel == Commands.Enums.ColorModel.CIELUV || writer.Descriptor.ColorModel == Commands.Enums.ColorModel.RGBrelated)
            {
                // If the model is CIELAB, CIELUV, or RGB-related then 3 parameters:
                // P1: (real) scale and offset pair for first component
                writer.WriteReal(colorValueExtent.FirstScale);
                writer.WriteReal(colorValueExtent.FirstOffset);
                // P2: (real) scale and offset pair for second component
                writer.WriteReal(colorValueExtent.SecondScale);
                writer.WriteReal(colorValueExtent.SecondOffset);
                // P3: (real) scale and offset pair for third component
                writer.WriteReal(colorValueExtent.ThirdScale);
                writer.WriteReal(colorValueExtent.ThirdOffset);
            }
            else
            {
                throw new NotSupportedException($"The color model '{writer.Descriptor.ColorModel}' is not supported.");
            }
        }

        public static void ColorModel(MetafileWriter writer, Command command)
        {
            var colorModelCommand = (ColorModelCommand)command;
            // P1: (index) colour model: valid values are
            //      1 RGB
            //      2 CIELAB
            //      3 CIELUV
            //      4 CMYK
            //      5 RGB - related
            //      > 5 reserved for registered values
            writer.WriteIndex((int)colorModelCommand.ColorModel);

            writer.Descriptor.ColorModel = colorModelCommand.ColorModel;
        }

        public static void NamePrecision(MetafileWriter writer, Command command)
        {
            var namePrecision = (NamePrecision)command;
            // P1: (integer) name precision: valid values are 8, 16, 24 or 32
            writer.WriteInteger(namePrecision.Precision);

            writer.Descriptor.NamePrecision = namePrecision.Precision;
        }

        public static void MetafileElementsList(MetafileWriter writer, Command command)
        {
            var metafileElementsList = (MetafileElementsList)command;
            // P1: (integer) number of elements specified
            writer.WriteInteger(metafileElementsList.Elements.Count());
            // P2: (index-pair array) List of metafile elements in this metafile. Each element is represented by two values:
            //      the first is its element class code (as in Table 2)
            //      the second is its element id code (as in Table 3 to Table 10).
            foreach (var element in metafileElementsList.Elements)
            {
                writer.WriteIndex(element.ElementClass);
                writer.WriteIndex(element.ElementId);
            }
        }

        public static void FontList(MetafileWriter writer, Command command)
        {
            var fontList = (FontList)command;
            // P1-Pn: (string fixed) n font names
            foreach (string font in fontList.Fonts)
                writer.WriteString(font);
        }

        public static void MaximumVdcExtent(MetafileWriter writer, Command command)
        {
            var maximumVdcExtent = (MaximumVdcExtent)command;
            // P1: (point) first corner
            writer.WritePoint(maximumVdcExtent.FirstCorner);
            // P2: (point) second corner
            writer.WritePoint(maximumVdcExtent.SecondCorner);
        }

        public static void SegmentPriorityExtent(MetafileWriter writer, Command command)
        {
            var segmentPriorityExtent = (SegmentPriorityExtent)command;
            // P1: (integer) minimum segment priority value: valid values are non-negative integers
            writer.WriteInteger(segmentPriorityExtent.MinimumPriorityValue);
            // P2: (integer) maximum segment priority value: valid values are non-negative integers
            writer.WriteInteger(segmentPriorityExtent.MaximumPriorityValue);
        }

        public static void CharacterSetList(MetafileWriter writer, Command command)
        {
            var characterSetList = (CharacterSetList)command;

            foreach (var entry in characterSetList.Entries)
            {
                // P1: (enumerated) CHARACTER SET TYPE: valid codes are
                //      0 94 - character G - set
                //      1 96 - character G - set
                //      2 94 - character multibyte G-set
                //      3 96 - character multibyte G-set
                //      4 complete code
                writer.WriteEnum(entry.CharacterSetType);
                // P2: (string fixed) Designation sequence tail; see Part 1, subclause 7.3.14.
                writer.WriteString(entry.DesignationSequenceTail);
            }
        }

        public static void CharacterCodingAnnouncer(MetafileWriter writer, Command command)
        {
            var characterCodingAnnouncer = (CharacterCodingAnnouncer)command;
            // P1: (enumerated) character coding announcer: valid values are
            //      0 basic 7 - bit
            //      1 basic 8 - bit
            //      2 extended 7 - bit
            //      3 extended 8 - bit
            writer.WriteEnum(characterCodingAnnouncer.CharacterCodingAnnouncerType);
        }

        public static void FontProperties(MetafileWriter writer, Command command)
        {
            var fontProperties = (FontProperties)command;
            foreach (var property in fontProperties.Properties)
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
                writer.WriteIndex(property.Indicator);
                // P2: (integer) priority, valid values are non-negative integers.
                writer.WriteInteger(property.Priority);
                // P3: (structured data record) property value record, each record contains a single member and is comprised of
                // [data type indicator, data element count, data element(s)].
                // NOTE: we only store that single member; but we still have to write a full record.
                writer.WriteStructuredDataRecord(new StructuredDataRecord(new[] { property.Record }));
            }
        }
    }
}
