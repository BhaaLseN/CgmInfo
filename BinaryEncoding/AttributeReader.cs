using CgmInfo.Commands.Attributes;
using CgmInfo.Commands.Enums;

namespace CgmInfo.BinaryEncoding
{
    // [ISO/IEC 8632-3 8.7]
    internal static class AttributeReader
    {
        public static LineBundleIndex LineBundleIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) line bundle index
            return new LineBundleIndex(reader.ReadIndex());
        }

        public static LineType LineType(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) line type: the following values are standardized:
            //      1 solid
            //      2 dash
            //      3 dot
            //      4 dash-dot
            //      5 dash-dot-dot
            //      >5 reserved for registered values
            //      negative for private use
            return new LineType(reader.ReadIndex());
        }

        public static LineWidth LineWidth(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (size specification) line width: see Part 1, subclause 7.1 for its form.
            //      line width is affected by LINE WIDTH SPECIFICATION MODE
            return new LineWidth(reader.ReadSizeSpecification(reader.Descriptor.LineWidthSpecificationMode));
        }

        public static LineColor LineColor(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (colour) line colour
            return new LineColor(reader.ReadColor());
        }

        public static MarkerBundleIndex MarkerBundleIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) marker bundle index
            return new MarkerBundleIndex(reader.ReadIndex());
        }

        public static MarkerType MarkerType(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) marker type: the following values are standardized:
            //      1 dot
            //      2 plus
            //      3 asterisk
            //      4 circle
            //      5 cross
            //      >5 reserved for registered values
            //      negative for private use
            return new MarkerType(reader.ReadIndex());
        }

        public static MarkerSize MarkerSize(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (size specification) marker size: see Part 1, subclause 7.1 for its form.
            //      marker size is affected by MARKER SIZE SPECIFICATION MODE
            return new MarkerSize(reader.ReadSizeSpecification(reader.Descriptor.MarkerSizeSpecificationMode));
        }

        public static MarkerColor MarkerColor(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (colour) marker colour
            return new MarkerColor(reader.ReadColor());
        }

        public static TextBundleIndex TextBundleIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) text bundle index
            return new TextBundleIndex(reader.ReadIndex());
        }

        public static TextFontIndex TextFontIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) text font index
            return new TextFontIndex(reader.ReadIndex());
        }

        public static TextPrecision TextPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) text precision: valid values are
            //      0 string
            //      1 character
            //      2 stroke
            return new TextPrecision(reader.ReadEnum<TextPrecisionType>());
        }

        public static CharacterExpansionFactor CharacterExpansionFactor(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (real) character expansion factor
            return new CharacterExpansionFactor(reader.ReadReal());
        }

        public static CharacterSpacing CharacterSpacing(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (real) additional inter-character space
            return new CharacterSpacing(reader.ReadReal());
        }

        public static TextColor TextColor(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (colour) text colour
            return new TextColor(reader.ReadColor());
        }

        public static CharacterHeight CharacterHeight(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (vdc) character height
            return new CharacterHeight(reader.ReadVdc());
        }

        public static CharacterOrientation CharacterOrientation(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (vdc) X character up component
            // P2: (vdc) Y character up component
            // P3: (vdc) X character base component
            // P4: (vdc) Y character base component
            return new CharacterOrientation(reader.ReadPoint(), reader.ReadPoint());
        }

        public static TextPath TextPath(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) text path: valid values are:
            //      0 right
            //      1 left
            //      2 up
            //      3 down
            return new TextPath(reader.ReadEnum<TextPathType>());
        }

        public static TextAlignment TextAlignment(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) horizontal alignment: valid values are:
            //      0 normal horizontal
            //      1 left
            //      2 centre
            //      3 right
            //      4 continuous horizontal
            // P2: (enumerated) vertical alignment
            //      0 normal vertical
            //      1 top
            //      2 cap
            //      3 half
            //      4 base
            //      5 bottom
            //      6 continuous vertical
            // P3: (real) continuous horizontal alignment
            // P4: (real) continuous vertical alignment
            return new TextAlignment(reader.ReadEnum<HorizontalTextAlignment>(), reader.ReadEnum<VerticalTextAlignment>(), reader.ReadReal(), reader.ReadReal());
        }

        public static CharacterSetIndex CharacterSetIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) character set index
            return new CharacterSetIndex(reader.ReadIndex());
        }

        public static AlternateCharacterSetIndex AlternateCharacterSetIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) alternate character set index
            return new AlternateCharacterSetIndex(reader.ReadIndex());
        }

        public static FillBundleIndex FillBundleIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) fill bundle index
            return new FillBundleIndex(reader.ReadIndex());
        }

        public static InteriorStyle InteriorStyle(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) interior style: valid values are
            //      0 hollow
            //      1 solid
            //      2 pattern
            //      3 hatch
            //      4 empty
            //      5 geometric pattern
            //      6 interpolated
            return new InteriorStyle(reader.ReadEnum<InteriorStyleType>());
        }

        public static FillColor FillColor(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (colour) fill colour
            return new FillColor(reader.ReadColor());
        }

        public static HatchIndex HatchIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) hatch index: the following values are standardized:
            //      1 horizontal
            //      2 vertical
            //      3 positive slope
            //      4 negative slope
            //      5 horizontal/vertical crosshatch
            //      6 positive/negative slope crosshatch
            //      >6 reserved for registered values
            //      negative for private use
            return new HatchIndex(reader.ReadIndex());
        }

        public static PatternIndex PatternIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) pattern index
            return new PatternIndex(reader.ReadIndex());
        }

        public static EdgeBundleIndex EdgeBundleIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) edge bundle index
            return new EdgeBundleIndex(reader.ReadIndex());
        }

        public static EdgeType EdgeType(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) edge type: the following values are standardized:
            //      1 solid
            //      2 dash
            //      3 dot
            //      4 dash-dot
            //      5 dash-dot-dot
            //      >5 reserved for registered values
            //      negative for private use
            // TODO: all other enumerated types use index, but this one uses integer. typo in spec?
            return new EdgeType(reader.ReadIndex());
        }

        public static EdgeWidth EdgeWidth(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (size specification) edge width: see part 1, subclause 7.1 for its form.
            //      edge width is affected by EDGE WIDTH SPECIFICATION MODE
            return new EdgeWidth(reader.ReadSizeSpecification(reader.Descriptor.EdgeWidthSpecificationMode));
        }

        public static EdgeColor EdgeColor(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (colour) edge colour
            return new EdgeColor(reader.ReadColor());
        }

        public static EdgeVisibility EdgeVisibility(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) edge visibility: valid values are
            //      0 off
            //      1 on
            return new EdgeVisibility(reader.ReadEnum<OnOffIndicator>());
        }
    }
}
