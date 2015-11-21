using System.Collections.Generic;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.Segment;

namespace CgmInfo.TextEncoding
{
    internal static class SegmentReader
    {
        public static CopySegment CopySegment(MetafileReader reader)
        {
            return new CopySegment(reader.ReadName(), reader.ReadMatrix(), ParseTransformationApplication(reader.ReadEnum()));
        }
        public static InheritanceFilter InheritanceFilter(MetafileReader reader)
        {
            var items = new List<InheritanceFilterItem>();
            while (reader.HasMoreData(2))
            {
                items.Add(new InheritanceFilterItem(
                    ParseFilterDesignator(reader.ReadEnum()),
                    ParseFilterSetting(reader.ReadEnum())));
            }
            return new InheritanceFilter(items.ToArray());
        }

        private static SegmentTransformationApplication ParseTransformationApplication(string token)
        {
            // assume "no" unless its "yes"
            if (token.ToUpperInvariant() == "YES")
                return SegmentTransformationApplication.Yes;
            return SegmentTransformationApplication.No;
        }
        private static readonly Dictionary<string, InheritanceFilterDesignator> FilterDesignatorMapping = new Dictionary<string, InheritanceFilterDesignator>
        {
            // single values
            { "LINEINDEX", InheritanceFilterDesignator.LineBundleIndex },
            { "LINETYPE", InheritanceFilterDesignator.LineType },
            { "LINEWIDTH", InheritanceFilterDesignator.LineWidth },
            { "LINECOLR", InheritanceFilterDesignator.LineColor },
            { "LINECLIPMODE", InheritanceFilterDesignator.LineClippingMode },
            { "MARKERINDEX", InheritanceFilterDesignator.MarkerBundleIndex },
            { "MARKERTYPE", InheritanceFilterDesignator.MarkerType },
            { "MARKERSIZE", InheritanceFilterDesignator.MarkerSize },
            { "MARKERCOLR", InheritanceFilterDesignator.MarkerColor },
            { "MARKERCLIPMODE", InheritanceFilterDesignator.MarkerClippingMode },
            { "TEXTINDEX", InheritanceFilterDesignator.TextBundleIndex },
            { "TEXTFONTINDEX", InheritanceFilterDesignator.TextFontIndex },
            { "TEXTPREC", InheritanceFilterDesignator.TextPrecision },
            { "CHAREXPAN", InheritanceFilterDesignator.CharacterExpansionFactor },
            { "CHARSPACE", InheritanceFilterDesignator.CharacterSpacing },
            { "TEXTCOLR", InheritanceFilterDesignator.TextColor },
            { "CHARHEIGHT", InheritanceFilterDesignator.CharacterHeight },
            { "CHARORI", InheritanceFilterDesignator.CharacterOrientation },
            { "TEXTPATH", InheritanceFilterDesignator.TextPath },
            { "TEXTALIGN", InheritanceFilterDesignator.TextAlignment },
            { "FILLINDEX", InheritanceFilterDesignator.FillBundleIndex },
            { "INTSTYLE", InheritanceFilterDesignator.InteriorStyle },
            { "FILLCOLR", InheritanceFilterDesignator.FillColor },
            { "HATCHINDEX", InheritanceFilterDesignator.HatchIndex },
            { "PATINDEX", InheritanceFilterDesignator.PatternIndex },
            { "EDGEINDEX", InheritanceFilterDesignator.EdgeBundleIndex },
            { "EDGETYPE", InheritanceFilterDesignator.EdgeType },
            { "EDGEWIDTH", InheritanceFilterDesignator.EdgeWidth },
            { "EDGECOLR", InheritanceFilterDesignator.EdgeColor },
            { "EDGEVIS", InheritanceFilterDesignator.EdgeVisibility },
            { "EDGECLIPMODE", InheritanceFilterDesignator.EdgeClippingMode },
            { "FILLREFPT", InheritanceFilterDesignator.FillReferencePoint },
            { "PATSIZE", InheritanceFilterDesignator.PatternSize },
            { "AUXCOLR", InheritanceFilterDesignator.AuxiliaryColor },
            { "TRANSPARENCY", InheritanceFilterDesignator.Transparency },
            { "LINEATTR", InheritanceFilterDesignator.LineAttributes },
            { "MARKERATTR", InheritanceFilterDesignator.MarkerAttributes },
            { "TEXPRESANDPLACEMATTR", InheritanceFilterDesignator.TextPresentationAndPlacementAttributes },
            { "TEXTPLACEMANDORIATTR", InheritanceFilterDesignator.TextPlacementAndOrientationAttributes },
            { "FILLATTR", InheritanceFilterDesignator.FillAttributes },
            { "EDGEATTR", InheritanceFilterDesignator.EdgeAttributes },
            { "PATATTR", InheritanceFilterDesignator.PatternAttributes },
            { "OUTPUTCTRL", InheritanceFilterDesignator.OutputControl },
            { "PICKID", InheritanceFilterDesignator.PickIdentifier },
            { "ALLATTRCTRL", InheritanceFilterDesignator.AllAttributesAndControl },
            // ALLINH means all attributes, control elements and ASFs
            { "ALLINH", InheritanceFilterDesignator.All },
            { "LINETYPEASF", InheritanceFilterDesignator.LineTypeAspectSourceFlag },
            { "LINEWIDTHASF", InheritanceFilterDesignator.LineWidthAspectSourceFlag },
            { "LINECOLRASF", InheritanceFilterDesignator.LineColorAspectSourceFlag },
            { "MARKERTYPEASF", InheritanceFilterDesignator.MarkerTypeAspectSourceFlag },
            { "MARKERSIZEASF", InheritanceFilterDesignator.MarkerSizeAspectSourceFlag },
            { "MARKERCOLRASF", InheritanceFilterDesignator.MarkerColorAspectSourceFlag },
            { "TEXTFONTINDEXASF", InheritanceFilterDesignator.TextFontIndexAspectSourceFlag },
            { "TEXTPRECASF", InheritanceFilterDesignator.TextPrecisionAspectSourceFlag },
            { "CHAREXPANASF", InheritanceFilterDesignator.CharacterExpansionFactorAspectSourceFlag },
            { "CHARSPACEASF", InheritanceFilterDesignator.CharacterSpacingAspectSourceFlag },
            { "TEXTCOLRASF", InheritanceFilterDesignator.TextColorAspectSourceFlag },
            { "INTSTYLEASF", InheritanceFilterDesignator.InteriorStyleAspectSourceFlag },
            { "FILLCOLRASF", InheritanceFilterDesignator.FillColorAspectSourceFlag },
            { "HATCHINDEXASF", InheritanceFilterDesignator.HatchIndexAspectSourceFlag },
            { "PATINDEXASF", InheritanceFilterDesignator.PatternIndexAspectSourceFlag },
            { "EDGETYPEASF", InheritanceFilterDesignator.EdgeTypeAspectSourceFlag },
            { "EDGEWIDTHASF", InheritanceFilterDesignator.EdgeWidthAspectSourceFlag },
            { "EDGECOLRASF", InheritanceFilterDesignator.EdgeColorAspectSourceFlag },
            { "ALLLINE", InheritanceFilterDesignator.LineAspectSourceFlags },
            { "ALLMARKER", InheritanceFilterDesignator.MarkerAspectSourceFlags },
            { "ALLTEXT", InheritanceFilterDesignator.TextAspectSourceFlags },
            { "ALLFILL", InheritanceFilterDesignator.FillAspectSourceFlags },
            { "ALLEDGE", InheritanceFilterDesignator.EdgeAspectSourceFlags },
            { "ALL", InheritanceFilterDesignator.AllAspectSourceFlags },
            { "MITRELIMIT", InheritanceFilterDesignator.MiterLimit },
            { "LINECAP", InheritanceFilterDesignator.LineCap },
            { "LINEJOIN", InheritanceFilterDesignator.LineJoin },
            { "LINETYPECONT", InheritanceFilterDesignator.LineTypeContinuation },
            { "LINETYPEINITOFFSET", InheritanceFilterDesignator.LineTypeInitialOffset },
            { "TEXTSCORETYPE", InheritanceFilterDesignator.TextScoreType },
            { "RESTRTEXTTYPE", InheritanceFilterDesignator.RestrictedTextType },
            { "INTERPOLATEDINTERIOR", InheritanceFilterDesignator.InterpolatedInterior },
            { "EDGECAP", InheritanceFilterDesignator.EdgeCap },
            { "EDGEJOIN", InheritanceFilterDesignator.EdgeJoin },
            { "EDGETYPECONT", InheritanceFilterDesignator.EdgeTypeContinuation },
            { "EDGETYPEINITOFFSET", InheritanceFilterDesignator.EdgeTypeInitialOffset },
            { "SYMBOLLIBINDEX", InheritanceFilterDesignator.SymbolLibraryIndex },
            { "SYMBOLCOLR", InheritanceFilterDesignator.SymbolColor },
            { "SYMBOLSIZE", InheritanceFilterDesignator.SymbolSize },
            { "SYMBOLORI", InheritanceFilterDesignator.SymbolOrientation },
            { "SYMBOLATTR", InheritanceFilterDesignator.SymbolAttributes },
        };
        private static InheritanceFilterDesignator ParseFilterDesignator(string token)
        {
            InheritanceFilterDesignator designator;
            if (!FilterDesignatorMapping.TryGetValue(token.ToUpperInvariant(), out designator))
                designator = 0;
            return designator;
        }
        private static InheritanceFilterSetting ParseFilterSetting(string token)
        {
            // assume "state list" unless its "segment"
            if (token.ToUpperInvariant() == "SEG")
                return InheritanceFilterSetting.Segment;
            return InheritanceFilterSetting.StateList;
        }
    }
}
