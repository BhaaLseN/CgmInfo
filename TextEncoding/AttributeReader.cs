using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CgmInfo.Commands.Attributes;
using CgmInfo.Commands.Enums;
using CgmInfo.Utilities;

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

        public static FillReferencePoint FillReferencePoint(MetafileReader reader)
        {
            return new FillReferencePoint(reader.ReadPoint());
        }

        public static PatternTable PatternTable(MetafileReader reader)
        {
            int index = reader.ReadIndex();
            int nx = reader.ReadInteger();
            int ny = reader.ReadInteger();
            // TODO: not really used in text encoding; but in case we ever need it,
            //       the same check for zero as in binary encoding needs to happen.
            //       intentionally unused until that time comes.
            int localColorPrecision = reader.ReadInteger();
            var colors = new List<MetafileColor>();
            int count = nx * ny;
            while (reader.HasMoreData(3) && count --> 0)
                colors.Add(reader.ReadColor());

            return new PatternTable(index, nx, ny, colors.ToArray());
        }

        public static PatternSize PatternSize(MetafileReader reader)
        {
            // NOTE: Pattern size may only be 'absolute' (VDC) in Version 1 and 2 metafiles. In Version 3 and 4 metafiles it may be
            //       expressed in any of the modes which can be selected with INTERIOR STYLE SPECIFICATION MODE.
            var specificationMode = reader.Properties.Version < 3 ? WidthSpecificationModeType.Absolute : reader.Descriptor.InteriorStyleSpecificationMode;
            return new PatternSize(
                new PointF((float)reader.ReadSizeSpecification(specificationMode), (float)reader.ReadSizeSpecification(specificationMode)),
                new PointF((float)reader.ReadSizeSpecification(specificationMode), (float)reader.ReadSizeSpecification(specificationMode)));
        }

        public static ColorTable ColorTable(MetafileReader reader)
        {
            int startIndex = reader.ReadColorIndex();
            var colors = new List<MetafileColor>();
            while (reader.HasMoreData())
                colors.Add(reader.ReadDirectColor());
            return new ColorTable(startIndex, colors.ToArray());
        }

        public static AspectSourceFlags AspectSourceFlags(MetafileReader reader)
        {
            var asf = new Dictionary<AspectSourceFlagsType, AspectSourceFlagsValue>();
            while (reader.HasMoreData(2))
                SetASFValue(asf, reader.ReadEnum(), reader.ReadEnum());
            return new AspectSourceFlags(asf);
        }

        public static PickIdentifier PickIdentifier(MetafileReader reader)
        {
            return new PickIdentifier(reader.ReadName());
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

        private static readonly Dictionary<string, AspectSourceFlagsType[]> ASFMapping = new Dictionary<string, AspectSourceFlagsType[]>
        {
            // single values
            { "LINETYPE", new[] { AspectSourceFlagsType.LineType } },
            { "LINEWIDTH", new[] { AspectSourceFlagsType.LineWidth } },
            { "LINECOLR", new[] { AspectSourceFlagsType.LineColor } },
            { "MARKERTYPE", new[] { AspectSourceFlagsType.MarkerType } },
            { "MARKERSIZE", new[] { AspectSourceFlagsType.MarkerSize } },
            { "MARKERCOLR", new[] { AspectSourceFlagsType.MarkerColor } },
            { "TEXTFONTINDEX", new[] { AspectSourceFlagsType.TextFontIndex } },
            { "TEXTPREC", new[] { AspectSourceFlagsType.TextPrecision } },
            { "CHAREXPAN", new[] { AspectSourceFlagsType.CharacterExpansionFactor } },
            { "CHARSPACE", new[] { AspectSourceFlagsType.CharacterSpacing } },
            { "TEXTCOLR", new[] { AspectSourceFlagsType.TextColor } },
            { "INTSTYLE", new[] { AspectSourceFlagsType.InteriorStyle } },
            { "FILLCOLR", new[] { AspectSourceFlagsType.FillColor } },
            { "HATCHINDEX", new[] { AspectSourceFlagsType.HatchIndex } },
            { "PATINDEX", new[] { AspectSourceFlagsType.PatternIndex } },
            { "EDGETYPE", new[] { AspectSourceFlagsType.EdgeType } },
            { "EDGEWIDTH", new[] { AspectSourceFlagsType.EdgeWidth } },
            { "EDGECOLR", new[] { AspectSourceFlagsType.EdgeColor } },

            // pseudo ASF classes; they set a range of ASF at the same time
            // NOTE: The pseudo-ASFs are a shorthand convenience for setting a number of ASFs at once.

            // ALL: set all ASFs as indicated.
            { "ALL", Enum.GetValues(typeof(AspectSourceFlagsType)).Cast<AspectSourceFlagsType>().ToArray() },

            // ALLLINE: set LINETYPE, LINEWIDTH, and LINECOLR ASFs as indicated.
            { "ALLLINE", new[]
                {
                    AspectSourceFlagsType.LineType, AspectSourceFlagsType.LineWidth, AspectSourceFlagsType.LineColor,
                }
            },

            // ALLMARKER: set MARKERTYPE, MARKERSIZE, and MARKERCOLR ASFs as indicated.
            { "ALLMARKER", new[]
                {
                    AspectSourceFlagsType.MarkerType, AspectSourceFlagsType.MarkerSize, AspectSourceFlagsType.MarkerColor,
                }
            },

            // ALLTEXT: set TEXTFONTINDEX, TEXTPREC, CHAREXPAN, CHARSPACE, and TEXTCOLR ASFs as indicated.
            { "ALLTEXT", new[]
                {
                    AspectSourceFlagsType.TextFontIndex, AspectSourceFlagsType.TextPrecision, AspectSourceFlagsType.CharacterExpansionFactor,
                    AspectSourceFlagsType.CharacterSpacing, AspectSourceFlagsType.TextColor,
                }
            },

            // ALLFILL: set INTSTYLE, FILLCOLR, HATCHINDEX, and PATINDEX ASFs as indicated.
            { "ALLFILL", new[]
                {
                    AspectSourceFlagsType.InteriorStyle, AspectSourceFlagsType.FillColor, AspectSourceFlagsType.HatchIndex,
                    AspectSourceFlagsType.PatternIndex,
                }
            },

            // ALLEDGE: set EDGETYPE, EDGEWIDTH, and EDGECOLR as indicated.
            { "ALLEDG", new[]
                {
                    AspectSourceFlagsType.EdgeType, AspectSourceFlagsType.EdgeWidth, AspectSourceFlagsType.EdgeColor,
                }
            },
        };
        private static void SetASFValue(Dictionary<AspectSourceFlagsType, AspectSourceFlagsValue> asf, string typeToken, string valueToken)
        {
            AspectSourceFlagsType[] asfTypes;
            if (ASFMapping.TryGetValue(typeToken.ToUpperInvariant(), out asfTypes))
            {
                AspectSourceFlagsValue asfValue = valueToken.ToUpperInvariant() == "BUNDLED"
                    ? AspectSourceFlagsValue.Bundled
                    : AspectSourceFlagsValue.Individual;

                foreach (AspectSourceFlagsType asfType in asfTypes)
                    asf[asfType] = asfValue;
            }
        }
    }
}
