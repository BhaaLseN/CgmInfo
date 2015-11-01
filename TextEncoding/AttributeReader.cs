using CgmInfo.Commands.Attributes;
using CgmInfo.Commands.Enums;

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

        public static TextPrecision TextPrecision(MetafileReader reader)
        {
            return new TextPrecision(ParseTextPrecision(reader.ReadEnum()));
        }

        public static CharacterExpansionFactor CharacterExpansionFactor(MetafileReader reader)
        {
            return new CharacterExpansionFactor(reader.ReadReal());
        }

        public static CharacterSpacing CharacterSpacing(MetafileReader reader)
        {
            return new CharacterSpacing(reader.ReadReal());
        }

        public static TextColor TextColor(MetafileReader reader)
        {
            return new TextColor(reader.ReadColor());
        }

        public static CharacterHeight CharacterHeight(MetafileReader reader)
        {
            return new CharacterHeight(reader.ReadVdc());
        }

        public static CharacterOrientation CharacterOrientation(MetafileReader reader)
        {
            return new CharacterOrientation(reader.ReadPoint(), reader.ReadPoint());
        }

        public static TextPath TextPath(MetafileReader reader)
        {
            return new TextPath(ParseTextPath(reader.ReadEnum()));
        }

        public static TextAlignment TextAlignment(MetafileReader reader)
        {
            return new TextAlignment(ParseHorizontalAlignment(reader.ReadEnum()), ParseVerticalAlignment(reader.ReadEnum()), reader.ReadReal(), reader.ReadReal());
        }

        public static CharacterSetIndex CharacterSetIndex(MetafileReader reader)
        {
            return new CharacterSetIndex(reader.ReadIndex());
        }

        public static AlternateCharacterSetIndex AlternateCharacterSetIndex(MetafileReader reader)
        {
            return new AlternateCharacterSetIndex(reader.ReadIndex());
        }

        public static FillBundleIndex FillBundleIndex(MetafileReader reader)
        {
            return new FillBundleIndex(reader.ReadIndex());
        }

        public static InteriorStyle InteriorStyle(MetafileReader reader)
        {
            return new InteriorStyle(ParseInteriorStyle(reader.ReadEnum()));
        }

        public static FillColor FillColor(MetafileReader reader)
        {
            return new FillColor(reader.ReadColor());
        }

        public static HatchIndex HatchIndex(MetafileReader reader)
        {
            return new HatchIndex(reader.ReadIndex());
        }

        public static PatternIndex PatternIndex(MetafileReader reader)
        {
            return new PatternIndex(reader.ReadIndex());
        }

        public static EdgeBundleIndex EdgeBundleIndex(MetafileReader reader)
        {
            return new EdgeBundleIndex(reader.ReadIndex());
        }

        public static EdgeType EdgeType(MetafileReader reader)
        {
            return new EdgeType(reader.ReadIndex());
        }

        public static EdgeWidth EdgeWidth(MetafileReader reader)
        {
            return new EdgeWidth(reader.ReadSizeSpecification(reader.Descriptor.EdgeWidthSpecificationMode));
        }

        public static EdgeColor EdgeColor(MetafileReader reader)
        {
            return new EdgeColor(reader.ReadColor());
        }

        public static EdgeVisibility EdgeVisibility(MetafileReader reader)
        {
            return new EdgeVisibility(TextEncodingHelper.GetOnOffValue(reader.ReadEnum()));
        }

        private static TextPrecisionType ParseTextPrecision(string token)
        {
            // assume string unless it matches any of the other possibilities
            token = token.ToUpperInvariant();
            if (token == "CHAR")
                return TextPrecisionType.Character;
            else if (token == "STROKE")
                return TextPrecisionType.Stroke;
            return TextPrecisionType.String;
        }
        private static TextPathType ParseTextPath(string token)
        {
            // assume right unless it matches any of the other possibilities
            token = token.ToUpperInvariant();
            if (token == "LEFT")
                return TextPathType.Left;
            else if (token == "UP")
                return TextPathType.Up;
            else if (token == "DOWN")
                return TextPathType.Down;
            return TextPathType.Right;
        }
        private static HorizontalTextAlignment ParseHorizontalAlignment(string token)
        {
            // assume normal unless it matches any of the other possibilities
            token = token.ToUpperInvariant();
            if (token == "LEFT")
                return HorizontalTextAlignment.Left;
            else if (token == "CTR")
                return HorizontalTextAlignment.Center;
            else if (token == "RIGHT")
                return HorizontalTextAlignment.Right;
            else if (token == "CONTHORIZ")
                return HorizontalTextAlignment.Continuous;
            return HorizontalTextAlignment.Normal;
        }
        private static VerticalTextAlignment ParseVerticalAlignment(string token)
        {
            // assume normal unless it matches any of the other possibilities
            token = token.ToUpperInvariant();
            if (token == "TOP")
                return VerticalTextAlignment.Top;
            else if (token == "HALF")
                return VerticalTextAlignment.Half;
            else if (token == "BASE")
                return VerticalTextAlignment.Base;
            else if (token == "BOTTOM")
                return VerticalTextAlignment.Bottom;
            else if (token == "CONTVERT")
                return VerticalTextAlignment.Continuous;
            return VerticalTextAlignment.Normal;
        }
        private static InteriorStyleType ParseInteriorStyle(string token)
        {
            // assume hollow unless it matches any of the other possibilities
            token = token.ToUpperInvariant();
            if (token == "SOLID")
                return InteriorStyleType.Solid;
            else if (token == "PAT")
                return InteriorStyleType.Pattern;
            else if (token == "HATCH")
                return InteriorStyleType.Hatch;
            else if (token == "EMPTY")
                return InteriorStyleType.Empty;
            else if (token == "GEOPAT")
                return InteriorStyleType.GeometricPattern;
            else if (token == "INTERP")
                return InteriorStyleType.Interpolated;
            return InteriorStyleType.Hollow;
        }
    }
}
