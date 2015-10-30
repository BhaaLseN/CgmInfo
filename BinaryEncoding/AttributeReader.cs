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
    }
}
