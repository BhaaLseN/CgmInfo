using CgmInfo.Commands;
using CgmInfo.Commands.ApplicationStructureDescriptor;
using CgmInfo.Commands.Attributes;
using CgmInfo.Commands.Delimiter;
using CgmInfo.Commands.Escape;
using CgmInfo.Commands.External;
using CgmInfo.Commands.GraphicalPrimitives;
using CgmInfo.Commands.MetafileDescriptor;
using CgmInfo.Commands.PictureDescriptor;
using CgmInfo.Commands.Segment;

namespace CgmInfo.Traversal
{
    /// <summary>
    /// Base class for <see cref="ICommandVisitor{T}"/> providing no-op implementations
    /// for easier/cleaner overriding of methods necessary for derived logic.
    /// </summary>
    /// <typeparam name="T">Any parameter type to be passed back by the visited classes</typeparam>
    public abstract class CommandVisitor<T> : ICommandVisitor<T>
    {
        #region Application Structure Descriptor elements
        public virtual void AcceptApplicationStructureDescriptorAttribute(ApplicationStructureAttribute applicationStructureAttribute, T parameter)
        {
            // intentionally left blank
        }
        #endregion

        #region Delimiter elements
        public virtual void AcceptDelimiterBeginApplicationStructure(BeginApplicationStructure beginApplicationStructure, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptDelimiterBeginApplicationStructureBody(BeginApplicationStructureBody beginApplicationStructureBody, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptDelimiterBeginCompoundLine(BeginCompoundLine beginCompoundLine, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptDelimiterBeginCompoundTextPath(BeginCompoundTextPath beginCompoundTextPath, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptDelimiterBeginFigure(BeginFigure beginFigure, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptDelimiterBeginMetafile(BeginMetafile beginMetafile, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptDelimiterBeginPicture(BeginPicture beginPicture, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptDelimiterBeginPictureBody(BeginPictureBody beginPictureBody, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptDelimiterBeginProtectionRegion(BeginProtectionRegion beginProtectionRegion, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptDelimiterBeginSegment(BeginSegment beginSegment, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptDelimiterBeginTileArray(BeginTileArray beginTileArray, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptDelimiterEndApplicationStructure(EndApplicationStructure endApplicationStructure, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptDelimiterEndCompoundLine(EndCompoundLine endCompoundLine, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptDelimiterEndCompoundTextPath(EndCompoundTextPath endCompoundTextPath, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptDelimiterEndFigure(EndFigure endFigure, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptDelimiterEndMetafile(EndMetafile endMetafile, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptDelimiterEndPicture(EndPicture endPicture, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptDelimiterEndProtectionRegion(EndProtectionRegion endProtectionRegion, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptDelimiterEndSegment(EndSegment endSegment, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptDelimiterEndTileArray(EndTileArray endTileArray, T parameter)
        {
            // intentionally left blank
        }
        #endregion

        #region Control elements
        public virtual void AcceptControlVdcIntegerPrecision(VdcIntegerPrecision vdcIntegerPrecision, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptControlVdcRealPrecision(VdcRealPrecision vdcRealPrecision, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptControlAuxiliaryColor(AuxiliaryColor auxiliaryColor, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptControlTransparency(Transparency transparency, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptControlClipRectangle(ClipRectangle clipRectangle, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptControlClipIndicator(ClipIndicator clipIndicator, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptControlLineClippingMode(LineClippingMode lineClippingMode, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptControlMarkerClippingMode(MarkerClippingMode markerClippingMode, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptControlEdgeClippingMode(EdgeClippingMode edgeClippingMode, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptControlNewRegion(NewRegion newRegion, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptControlSavePrimitiveContext(SavePrimitiveContext savePrimitiveContext, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptControlRestorePrimitiveContext(RestorePrimitiveContext restorePrimitiveContext, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptControlProtectionRegionIndicator(ProtectionRegionIndicator protectionRegionIndicator, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptControlGeneralizedTextPathMode(GeneralizedTextPathMode generalizedTextPathMode, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptControlMiterLimit(MiterLimit miterLimit, T parameter)
        {
            // intentionally left blank
        }
        #endregion

        #region Graphical Primitive elements
        public virtual void AcceptGraphicalPrimitivePolyline(Polyline polyline, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveDisjointPolyline(DisjointPolyline disjointPolyline, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitivePolymarker(Polymarker polymarker, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveAppendText(AppendText appendText, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveRestrictedText(RestrictedText restrictedText, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveText(TextCommand text, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitivePolygon(Polygon polygon, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitivePolygonSet(PolygonSet polygonSet, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveCellArray(CellArray cellArray, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveRectangle(Rectangle rectangle, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveCircle(Circle circle, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveCircularArc3Point(CircularArc3Point circularArc3Point, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveCircularArc3PointClose(CircularArc3PointClose circularArc3PointClose, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveCircularArcCenter(CircularArcCenter circularArcCenter, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveCircularArcCenterClose(CircularArcCenterClose circularArcCenterClose, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveEllipse(Ellipse ellipse, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveEllipticalArc(EllipticalArc ellipticalArc, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveEllipticalArcClose(EllipticalArcClose ellipticalArcClose, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveCircularArcCenterReversed(CircularArcCenterReversed circularArcCenterReversed, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveConnectingEdge(ConnectingEdge connectingEdge, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveHyperbolicArc(HyperbolicArc hyperbolicArc, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveParabolicArc(ParabolicArc parabolicArc, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveNonUniformBSpline(NonUniformBSpline nonUniformBSpline, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveNonUniformRationalBSpline(NonUniformRationalBSpline nonUniformRationalBSpline, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitivePolybezier(Polybezier polybezier, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveBitonalTile(BitonalTile bitonalTile, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptGraphicalPrimitiveTile(Tile tile, T parameter)
        {
            // intentionally left blank
        }
        #endregion

        #region Attribute elements
        public virtual void AcceptAttributeLineBundleIndex(LineBundleIndex lineBundleIndex, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeLineType(LineType lineType, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeLineWidth(LineWidth lineWidth, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeLineColor(LineColor lineColor, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeMarkerBundleIndex(MarkerBundleIndex markerBundleIndex, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeMarkerType(MarkerType markerType, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeMarkerSize(MarkerSize markerSize, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeMarkerColor(MarkerColor markerColor, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeTextBundleIndex(TextBundleIndex textBundleIndex, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeTextFontIndex(TextFontIndex textFontIndex, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeTextPrecision(TextPrecision textPrecision, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeCharacterExpansionFactor(CharacterExpansionFactor characterExpansionFactor, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeCharacterSpacing(CharacterSpacing characterSpacing, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeTextColor(TextColor textColor, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeCharacterHeight(CharacterHeight characterHeight, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeCharacterOrientation(CharacterOrientation characterOrientation, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeTextPath(TextPath textPath, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeTextAlignment(TextAlignment textAlignment, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeCharacterSetIndex(CharacterSetIndex characterSetIndex, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeAlternateCharacterSetIndex(AlternateCharacterSetIndex alternateCharacterSetIndex, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeFillBundleIndex(FillBundleIndex fillBundleIndex, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeInteriorStyle(InteriorStyle interiorStyle, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeFillColor(FillColor fillColor, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeHatchIndex(HatchIndex hatchIndex, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributePatternIndex(PatternIndex patternIndex, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeEdgeBundleIndex(EdgeBundleIndex edgeBundleIndex, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeEdgeType(EdgeType edgeType, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeEdgeWidth(EdgeWidth edgeWidth, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeEdgeColor(EdgeColor edgeColor, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeEdgeVisibility(EdgeVisibility edgeVisibility, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeFillReferencePoint(FillReferencePoint fillReferencePoint, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributePatternTable(PatternTable patternTable, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributePatternSize(PatternSize patternSize, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeColorTable(ColorTable colorTable, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeAspectSourceFlags(AspectSourceFlags aspectSourceFlags, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributePickIdentifier(PickIdentifier pickIdentifier, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeLineCap(LineCap lineCap, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeLineJoin(LineJoin lineJoin, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeLineTypeContinuation(LineTypeContinuation lineTypeContinuation, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeLineTypeInitialOffset(LineTypeInitialOffset lineTypeInitialOffset, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeRestrictedTextType(RestrictedTextType restrictedTextType, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeInterpolatedInterior(InterpolatedInterior interpolatedInterior, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeEdgeCap(EdgeCap edgeCap, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeEdgeJoin(EdgeJoin edgeJoin, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeEdgeTypeContinuation(EdgeTypeContinuation edgeTypeContinuation, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptAttributeEdgeTypeInitialOffset(EdgeTypeInitialOffset edgeTypeInitialOffset, T parameter)
        {
            // intentionally left blank
        }
        #endregion

        #region Metafile Descriptor elements
        public virtual void AcceptMetafileDescriptorCharacterCodingAnnouncer(CharacterCodingAnnouncer characterCodingAnnouncer, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptMetafileDescriptorCharacterSetList(CharacterSetList characterSetList, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptMetafileDescriptorColorIndexPrecision(ColorIndexPrecision colorIndexPrecision, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptMetafileDescriptorColorModel(ColorModelCommand colorModel, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptMetafileDescriptorColorPrecision(ColorPrecision colorPrecision, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptMetafileDescriptorColorValueExtent(ColorValueExtent colorValueExtent, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptMetafileDescriptorFontList(FontList fontList, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptMetafileDescriptorIndexPrecision(IndexPrecision indexPrecision, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptMetafileDescriptorIntegerPrecision(IntegerPrecision integerPrecision, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptMetafileDescriptorMaximumColorIndex(MaximumColorIndex maximumColorIndex, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptMetafileDescriptorMaximumVdcExtent(MaximumVdcExtent maximumVdcExtent, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptMetafileDescriptorSegmentPriorityExtent(SegmentPriorityExtent segmentPriorityExtent, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptMetafileDescriptorMetafileDescription(MetafileDescription metafileDescription, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptMetafileDescriptorMetafileVersion(MetafileVersion metafileVersion, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptMetafileDescriptorNamePrecision(NamePrecision namePrecision, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptMetafileDescriptorRealPrecision(RealPrecision realPrecision, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptMetafileDescriptorVdcType(VdcType vdcType, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptMetafileDescriptorMetafileElementsList(MetafileElementsList metafileElementsList, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptMetafileDescriptorMetafileDefaultsReplacement(MetafileDefaultsReplacement metafileDefaultsReplacement, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptMetafileDescriptorFontProperties(FontProperties fontProperties, T parameter)
        {
            // intentionally left blank
        }
        #endregion

        #region Picture Descriptor elements
        public virtual void AcceptPictureDescriptorScalingMode(ScalingMode scalingMode, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptPictureDescriptorColorSelectionMode(ColorSelectionMode colorSelectionMode, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptPictureDescriptorLineWidthSpecificationMode(LineWidthSpecificationMode lineWidthSpecificationMode, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptPictureDescriptorMarkerSizeSpecificationMode(MarkerSizeSpecificationMode markerSizeSpecificationMode, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptPictureDescriptorEdgeWidthSpecificationMode(EdgeWidthSpecificationMode edgeWidthSpecificationMode, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptPictureDescriptorVdcExtent(VdcExtent vdcExtent, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptPictureDescriptorBackgroundColor(BackgroundColor backgroundColor, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptPictureDescriptorDeviceViewport(DeviceViewport deviceViewport, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptPictureDescriptorDeviceViewportSpecificationMode(DeviceViewportSpecificationMode deviceViewportSpecificationMode, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptPictureDescriptorInteriorStyleSpecificationMode(InteriorStyleSpecificationMode interiorStyleSpecificationMode, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptPictureDescriptorLineAndEdgeTypeDefinition(LineAndEdgeTypeDefinition lineAndEdgeTypeDefinition, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptPictureDescriptorHatchStyleDefinition(HatchStyleDefinition hatchStyleDefinition, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptPictureDescriptorGeometricPatternDefinition(GeometricPatternDefinition geometricPatternDefinition, T parameter)
        {
            // intentionally left blank
        }
        #endregion

        #region Escape elements
        public virtual void AcceptEscapeEscape(EscapeCommand escapeCommand, T parameter)
        {
            // intentionally left blank
        }
        #endregion

        #region External elements
        public virtual void AcceptExternalMessage(Message message, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptExternalApplicationData(ApplicationData applicationData, T parameter)
        {
            // intentionally left blank
        }
        #endregion

        #region Segment control and segment attribute elements
        public virtual void AcceptSegmentCopySegment(CopySegment copySegment, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptSegmentInheritanceFilter(InheritanceFilter inheritanceFilter, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptSegmentClipInheritance(ClipInheritance clipInheritance, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptSegmentSegmentTransformation(SegmentTransformation segmentTransformation, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptSegmentSegmentHighlighting(SegmentHighlighting segmentHighlighting, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptSegmentSegmentDisplayPriority(SegmentDisplayPriority segmentDisplayPriority, T parameter)
        {
            // intentionally left blank
        }

        public virtual void AcceptSegmentSegmentPickPriority(SegmentPickPriority segmentPickPriority, T parameter)
        {
            // intentionally left blank
        }
        #endregion

        public virtual void AcceptUnsupportedCommand(UnsupportedCommand unsupportedCommand, T parameter)
        {
            // intentionally left blank
        }
        public virtual void AcceptInvalidCommand(InvalidCommand unsupportedCommand, T parameter)
        {
            // intentionally left blank
        }
    }
}
