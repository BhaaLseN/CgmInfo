using CgmInfo.Commands.Enums;
using CgmInfo.Commands.PictureDescriptor;

namespace CgmInfo.TextEncoding
{
    internal static class PictureDescriptorReader
    {
        public static ScalingMode ScalingMode(MetafileReader reader)
        {
            return new ScalingMode(ParseScalingMode(reader.ReadEnum()), reader.ReadReal());
        }

        public static ColorSelectionMode ColorSelectionMode(MetafileReader reader)
        {
            return new ColorSelectionMode(ParseColorMode(reader.ReadEnum()));
        }

        public static LineWidthSpecificationMode LineWidthSpecificationMode(MetafileReader reader)
        {
            return new LineWidthSpecificationMode(ParseWidthSpecificationMode(reader.ReadEnum()));
        }

        public static MarkerSizeSpecificationMode MarkerSizeSpecificationMode(MetafileReader reader)
        {
            return new MarkerSizeSpecificationMode(ParseWidthSpecificationMode(reader.ReadEnum()));
        }

        public static EdgeWidthSpecificationMode EdgeWidthSpecificationMode(MetafileReader reader)
        {
            return new EdgeWidthSpecificationMode(ParseWidthSpecificationMode(reader.ReadEnum()));
        }

        public static VdcExtent VdcExtent(MetafileReader reader)
        {
            var firstCorner = reader.ReadPoint();
            var secondCorner = reader.ReadPoint();
            return new VdcExtent(firstCorner, secondCorner);
        }

        public static BackgroundColor BackgroundColor(MetafileReader reader)
        {
            return new BackgroundColor(reader.ReadDirectColor());
        }

        private static ScalingModeType ParseScalingMode(string token)
        {
            // assume abstract; unless its metric
            if (token.ToUpperInvariant() == "METRIC")
                return ScalingModeType.Metric;
            return ScalingModeType.Abstract;
        }
        private static ColorModeType ParseColorMode(string token)
        {
            // assume indexed; unless its direct
            if (token.ToUpperInvariant() == "DIRECT")
                return ColorModeType.Direct;
            return ColorModeType.Indexed;
        }
        private static WidthSpecificationModeType ParseWidthSpecificationMode(string token)
        {
            // assume absolute unless it matches any of the others
            token = token.ToUpperInvariant();
            if (token == "SCALED")
                return WidthSpecificationModeType.Scaled;
            else if (token == "FRACTIONAL")
                return WidthSpecificationModeType.Fractional;
            else if (token == "MM")
                return WidthSpecificationModeType.Millimeters;
            return WidthSpecificationModeType.Absolute;
        }
    }
}
