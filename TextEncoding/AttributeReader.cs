using CgmInfo.Commands.Attributes;

namespace CgmInfo.TextEncoding
{
    internal static class AttributeReader
    {
        public static LineBundleIndex LineBundleIndex(MetafileReader reader)
        {
            return new LineBundleIndex(reader.ReadIndex());
        }

        public static LineType LineType(MetafileReader reader)
        {
            return new LineType(reader.ReadIndex());
        }

        public static LineWidth LineWidth(MetafileReader reader)
        {
            return new LineWidth(reader.ReadSizeSpecification(reader.Descriptor.LineWidthSpecificationMode));
        }

        public static LineColor LineColor(MetafileReader reader)
        {
            return new LineColor(reader.ReadColor());
        }

        public static MarkerBundleIndex MarkerBundleIndex(MetafileReader reader)
        {
            return new MarkerBundleIndex(reader.ReadIndex());
        }

        public static MarkerType MarkerType(MetafileReader reader)
        {
            return new MarkerType(reader.ReadIndex());
        }

        public static MarkerSize MarkerSize(MetafileReader reader)
        {
            return new MarkerSize(reader.ReadSizeSpecification(reader.Descriptor.MarkerSizeSpecificationMode));
        }

        public static MarkerColor MarkerColor(MetafileReader reader)
        {
            return new MarkerColor(reader.ReadColor());
        }

        public static TextBundleIndex TextBundleIndex(MetafileReader reader)
        {
            return new TextBundleIndex(reader.ReadIndex());
        }

        public static TextFontIndex TextFontIndex(MetafileReader reader)
        {
            return new TextFontIndex(reader.ReadIndex());
        }
    }
}
