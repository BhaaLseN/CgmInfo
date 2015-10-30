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
    }
}
