using CgmInfo.Commands.Attributes;

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
    }
}
