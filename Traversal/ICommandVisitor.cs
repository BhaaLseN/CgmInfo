using CgmInfo.Commands;
using CgmInfo.Commands.ApplicationStructureDescriptor;
using CgmInfo.Commands.Attributes;
using CgmInfo.Commands.Delimiter;
using CgmInfo.Commands.GraphicalPrimitives;
using CgmInfo.Commands.MetafileDescriptor;
using CgmInfo.Commands.PictureDescriptor;

namespace CgmInfo.Traversal
{
    public interface ICommandVisitor<T>
    {
        void AcceptUnsupportedCommand(UnsupportedCommand unsupportedCommand, T parameter);

        // delimiters
        void AcceptDelimiterBeginMetafile(BeginMetafile beginMetafile, T parameter);
        void AcceptDelimiterEndMetafile(EndMetafile endMetafile, T parameter);
        void AcceptDelimiterBeginPicture(BeginPicture beginPicture, T parameter);
        void AcceptDelimiterBeginPictureBody(BeginPictureBody beginPictureBody, T parameter);
        void AcceptDelimiterEndPicture(EndPicture endPicture, T parameter);
        void AcceptDelimiterBeginSegment(BeginSegment beginSegment, T parameter);
        void AcceptDelimiterEndSegment(EndSegment endSegment, T parameter);
        void AcceptDelimiterBeginFigure(BeginFigure beginFigure, T parameter);
        void AcceptDelimiterEndFigure(EndFigure endFigure, T parameter);
        void AcceptDelimiterBeginProtectionRegion(BeginProtectionRegion beginProtectionRegion, T parameter);
        void AcceptDelimiterEndProtectionRegion(EndProtectionRegion endProtectionRegion, T parameter);
        void AcceptDelimiterBeginCompoundLine(BeginCompoundLine beginCompoundLine, T parameter);
        void AcceptDelimiterEndCompoundLine(EndCompoundLine endCompoundLine, T parameter);
        void AcceptDelimiterBeginCompoundTextPath(BeginCompoundTextPath beginCompoundTextPath, T parameter);
        void AcceptDelimiterEndCompoundTextPath(EndCompoundTextPath endCompoundTextPath, T parameter);
        void AcceptDelimiterBeginTileArray(BeginTileArray beginTileArray, T parameter);
        void AcceptDelimiterEndTileArray(EndTileArray endTileArray, T parameter);
        void AcceptDelimiterBeginApplicationStructure(BeginApplicationStructure beginApplicationStructure, T parameter);
        void AcceptDelimiterBeginApplicationStructureBody(BeginApplicationStructureBody beginApplicationStructureBody, T parameter);
        void AcceptDelimiterEndApplicationStructure(EndApplicationStructure endApplicationStructure, T parameter);

        // metafile descriptor
        void AcceptMetafileDescriptorMetafileVersion(MetafileVersion metafileVersion, T parameter);
        void AcceptMetafileDescriptorMetafileDescription(MetafileDescription metafileDescription, T parameter);
        void AcceptMetafileDescriptorVdcType(VdcType vdcType, T parameter);
        void AcceptMetafileDescriptorIntegerPrecision(IntegerPrecision integerPrecision, T parameter);
        void AcceptMetafileDescriptorRealPrecision(RealPrecision realPrecision, T parameter);
        void AcceptMetafileDescriptorIndexPrecision(IndexPrecision indexPrecision, T parameter);
        void AcceptMetafileDescriptorColorPrecision(ColorPrecision colorPrecision, T parameter);
        void AcceptMetafileDescriptorColorIndexPrecision(ColorIndexPrecision colorIndexPrecision, T parameter);
        void AcceptMetafileDescriptorMaximumColorIndex(MaximumColorIndex maximumColorIndex, T parameter);
        void AcceptMetafileDescriptorColorValueExtent(ColorValueExtent colorValueExtent, T parameter);
        void AcceptMetafileDescriptorMetafileElementsList(MetafileElementsList metafileElementsList, T parameter);
        void AcceptMetafileDescriptorMetafileDefaultsReplacement(MetafileDefaultsReplacement metafileDefaultsReplacement, T parameter);
        void AcceptMetafileDescriptorFontList(FontList fontList, T parameter);
        void AcceptMetafileDescriptorCharacterSetList(CharacterSetList characterSetList, T parameter);
        void AcceptMetafileDescriptorCharacterCodingAnnouncer(CharacterCodingAnnouncer characterCodingAnnouncer, T parameter);
        void AcceptMetafileDescriptorNamePrecision(NamePrecision namePrecision, T parameter);
        void AcceptMetafileDescriptorMaximumVdcExtent(MaximumVdcExtent maximumVdcExtent, T parameter);
        void AcceptMetafileDescriptorColorModel(ColorModelCommand colorModel, T parameter);

        // picture descriptor
        void AcceptPictureDescriptorScalingMode(ScalingMode scalingMode, T parameter);
        void AcceptPictureDescriptorColorSelectionMode(ColorSelectionMode colorSelectionMode, T parameter);
        void AcceptPictureDescriptorLineWidthSpecificationMode(LineWidthSpecificationMode lineWidthSpecificationMode, T parameter);
        void AcceptPictureDescriptorMarkerSizeSpecificationMode(MarkerSizeSpecificationMode markerSizeSpecificationMode, T parameter);
        void AcceptPictureDescriptorEdgeWidthSpecificationMode(EdgeWidthSpecificationMode edgeWidthSpecificationMode, T parameter);
        void AcceptPictureDescriptorVdcExtent(VdcExtent vdcExtent, T parameter);
        void AcceptPictureDescriptorBackgroundColor(BackgroundColor backgroundColor, T parameter);
        void AcceptPictureDescriptorDeviceViewport(DeviceViewport deviceViewport, T parameter);
        void AcceptPictureDescriptorDeviceViewportSpecificationMode(DeviceViewportSpecificationMode deviceViewportSpecificationMode, T parameter);
        void AcceptPictureDescriptorInteriorStyleSpecificationMode(InteriorStyleSpecificationMode interiorStyleSpecificationMode, T parameter);
        void AcceptPictureDescriptorLineAndEdgeTypeDefinition(LineAndEdgeTypeDefinition lineAndEdgeTypeDefinition, T parameter);
        void AcceptPictureDescriptorHatchStyleDefinition(HatchStyleDefinition hatchStyleDefinition, T parameter);
        void AcceptPictureDescriptorGeometricPatternDefinition(GeometricPatternDefinition geometricPatternDefinition, T parameter);

        // control
        void AcceptControlVdcIntegerPrecision(VdcIntegerPrecision vdcIntegerPrecision, T parameter);
        void AcceptControlVdcRealPrecision(VdcRealPrecision vdcRealPrecision, T parameter);
        void AcceptControlAuxiliaryColor(AuxiliaryColor auxiliaryColor, T parameter);
        void AcceptControlTransparency(Transparency transparency, T parameter);
        void AcceptControlClipRectangle(ClipRectangle clipRectangle, T parameter);
        void AcceptControlClipIndicator(ClipIndicator clipIndicator, T parameter);
        void AcceptControlLineClippingMode(LineClippingMode lineClippingMode, T parameter);
        void AcceptControlMarkerClippingMode(MarkerClippingMode markerClippingMode, T parameter);
        void AcceptControlEdgeClippingMode(EdgeClippingMode edgeClippingMode, T parameter);
        void AcceptControlNewRegion(NewRegion newRegion, T parameter);
        void AcceptControlSavePrimitiveContext(SavePrimitiveContext savePrimitiveContext, T parameter);
        void AcceptControlRestorePrimitiveContext(RestorePrimitiveContext restorePrimitiveContext, T parameter);
        void AcceptControlProtectionRegionIndicator(ProtectionRegionIndicator protectionRegionIndicator, T parameter);
        void AcceptControlGeneralizedTextPathMode(GeneralizedTextPathMode generalizedTextPathMode, T parameter);
        void AcceptControlMiterLimit(MiterLimit miterLimit, T parameter);

        // graphical primitives
        void AcceptGraphicalPrimitivePolyline(Polyline polyline, T parameter);
        void AcceptGraphicalPrimitiveText(TextCommand text, T parameter);
        void AcceptGraphicalPrimitiveRestrictedText(RestrictedText restrictedText, T parameter);
        void AcceptGraphicalPrimitiveAppendText(AppendText appendText, T parameter);
        void AcceptGraphicalPrimitivePolygon(Polygon polygon, T parameter);
        void AcceptGraphicalPrimitiveRectangle(Rectangle rectangle, T parameter);
        void AcceptGraphicalPrimitiveCircle(Circle circle, T parameter);
        void AcceptGraphicalPrimitiveCircularArcCenter(CircularArcCenter circularArcCenter, T parameter);
        void AcceptGraphicalPrimitiveEllipse(Ellipse ellipse, T parameter);
        void AcceptGraphicalPrimitiveEllipticalArc(EllipticalArc ellipticalArc, T parameter);

        // attributes
        void AcceptAttributeLineBundleIndex(LineBundleIndex lineBundleIndex, T parameter);
        void AcceptAttributeLineType(LineType lineType, T parameter);
        void AcceptAttributeLineWidth(LineWidth lineWidth, T parameter);
        void AcceptAttributeLineColor(LineColor lineColor, T parameter);
        void AcceptAttributeMarkerBundleIndex(MarkerBundleIndex markerBundleIndex, T parameter);
        void AcceptAttributeMarkerType(MarkerType markerType, T parameter);
        void AcceptAttributeMarkerSize(MarkerSize markerSize, T parameter);
        void AcceptAttributeMarkerColor(MarkerColor markerColor, T parameter);
        void AcceptAttributeTextBundleIndex(TextBundleIndex textBundleIndex, T parameter);
        void AcceptAttributeTextFontIndex(TextFontIndex textFontIndex, T parameter);
        void AcceptAttributeTextPrecision(TextPrecision textPrecision, T parameter);
        void AcceptAttributeCharacterExpansionFactor(CharacterExpansionFactor characterExpansionFactor, T parameter);
        void AcceptAttributeCharacterSpacing(CharacterSpacing characterSpacing, T parameter);
        void AcceptAttributeTextColor(TextColor textColor, T parameter);
        void AcceptAttributeCharacterHeight(CharacterHeight characterHeight, T parameter);
        void AcceptAttributeCharacterOrientation(CharacterOrientation characterOrientation, T parameter);
        void AcceptAttributeTextPath(TextPath textPath, T parameter);
        void AcceptAttributeTextAlignment(TextAlignment textAlignment, T parameter);
        void AcceptAttributeCharacterSetIndex(CharacterSetIndex characterSetIndex, T parameter);
        void AcceptAttributeAlternateCharacterSetIndex(AlternateCharacterSetIndex alternateCharacterSetIndex, T parameter);
        void AcceptAttributeFillBundleIndex(FillBundleIndex fillBundleIndex, T parameter);
        void AcceptAttributeInteriorStyle(InteriorStyle interiorStyle, T parameter);
        void AcceptAttributeFillColor(FillColor fillColor, T parameter);
        void AcceptAttributeHatchIndex(HatchIndex hatchIndex, T parameter);
        void AcceptAttributePatternIndex(PatternIndex patternIndex, T parameter);
        void AcceptAttributeEdgeBundleIndex(EdgeBundleIndex edgeBundleIndex, T parameter);
        void AcceptAttributeEdgeType(EdgeType edgeType, T parameter);
        void AcceptAttributeEdgeWidth(EdgeWidth edgeWidth, T parameter);
        void AcceptAttributeEdgeColor(EdgeColor edgeColor, T parameter);
        void AcceptAttributeEdgeVisibility(EdgeVisibility edgeVisibility, T parameter);
        void AcceptAttributeFillReferencePoint(FillReferencePoint fillReferencePoint, T parameter);
        void AcceptAttributePatternTable(PatternTable patternTable, T parameter);
        void AcceptAttributePatternSize(PatternSize patternSize, T parameter);
        void AcceptAttributeColorTable(ColorTable colorTable, T parameter);
        void AcceptAttributeAspectSourceFlags(AspectSourceFlags aspectSourceFlags, T parameter);
        void AcceptAttributePickIdentifier(PickIdentifier pickIdentifier, T parameter);
        void AcceptAttributeLineCap(LineCap lineCap, T parameter);

        // application structure descriptor
        void AcceptApplicationStructureDescriptorAttribute(ApplicationStructureAttribute applicationStructureAttribute, T parameter);
    }
}
