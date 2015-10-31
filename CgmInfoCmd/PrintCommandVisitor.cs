using System.Linq;
using CgmInfo.Commands;
using CgmInfo.Commands.ApplicationStructureDescriptor;
using CgmInfo.Commands.Attributes;
using CgmInfo.Commands.Delimiter;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.GraphicalPrimitives;
using CgmInfo.Commands.MetafileDescriptor;
using CgmInfo.Commands.PictureDescriptor;
using CgmInfo.Traversal;

namespace CgmInfoCmd
{
    public class PrintCommandVisitor : ICommandVisitor<PrintContext>
    {
        public void AcceptDelimiterBeginMetafile(BeginMetafile beginMetafile, PrintContext parameter)
        {
            parameter.WriteLine("{0} - {1}", parameter.FileName, beginMetafile.Name);
            parameter.BeginLevel();
        }
        public void AcceptDelimiterEndMetafile(EndMetafile endMetafile, PrintContext parameter)
        {
            parameter.EndLevel();
        }
        public void AcceptDelimiterBeginPicture(BeginPicture beginPicture, PrintContext parameter)
        {
            parameter.WriteLine("Begin Picture: '{0}'", beginPicture.Name);
            parameter.BeginLevel();
        }
        public void AcceptDelimiterBeginPictureBody(BeginPictureBody beginPictureBody, PrintContext parameter)
        {
            parameter.WriteLine("Begin Picture Body");
        }
        public void AcceptDelimiterEndPicture(EndPicture endPicture, PrintContext parameter)
        {
            parameter.EndLevel();
        }
        public void AcceptDelimiterBeginSegment(BeginSegment beginSegment, PrintContext parameter)
        {
            parameter.WriteLine("Begin Segment: '{0}'", beginSegment.Identifier);
            parameter.BeginLevel();
        }
        public void AcceptDelimiterEndSegment(EndSegment endSegment, PrintContext parameter)
        {
            parameter.EndLevel();
        }
        public void AcceptDelimiterBeginFigure(BeginFigure beginFigure, PrintContext parameter)
        {
            parameter.WriteLine("Begin Figure");
            parameter.BeginLevel();
        }
        public void AcceptDelimiterEndFigure(EndFigure endFigure, PrintContext parameter)
        {
            parameter.EndLevel();
        }
        public void AcceptDelimiterBeginProtectionRegion(BeginProtectionRegion beginProtectionRegion, PrintContext parameter)
        {
            parameter.WriteLine("Begin Protection Region: {0}", beginProtectionRegion.RegionIndex);
            parameter.BeginLevel();
        }
        public void AcceptDelimiterEndProtectionRegion(EndProtectionRegion endProtectionRegion, PrintContext parameter)
        {
            parameter.EndLevel();
        }
        public void AcceptDelimiterBeginCompoundLine(BeginCompoundLine beginCompoundLine, PrintContext parameter)
        {
            parameter.WriteLine("Begin Compound Line");
            parameter.BeginLevel();
        }
        public void AcceptDelimiterEndCompoundLine(EndCompoundLine endCompoundLine, PrintContext parameter)
        {
            parameter.EndLevel();
        }
        public void AcceptDelimiterBeginCompoundTextPath(BeginCompoundTextPath beginCompoundTextPath, PrintContext parameter)
        {
            parameter.WriteLine("Begin Compound Text Path");
            parameter.BeginLevel();
        }
        public void AcceptDelimiterEndCompoundTextPath(EndCompoundTextPath endCompoundTextPath, PrintContext parameter)
        {
            parameter.EndLevel();
        }
        public void AcceptDelimiterBeginTileArray(BeginTileArray beginTileArray, PrintContext parameter)
        {
            parameter.WriteLine("Begin Tile Array: {0} by {1} tiles at {2} ({3} by {4} cells per tile)",
                beginTileArray.PathDirectionTileCount, beginTileArray.LineDirectionTileCount,
                beginTileArray.Position,
                beginTileArray.PathDirectionCellCount, beginTileArray.LineDirectionCellCount);
            parameter.BeginLevel();
        }
        public void AcceptDelimiterEndTileArray(EndTileArray endTileArray, PrintContext parameter)
        {
            parameter.EndLevel();
        }
        public void AcceptDelimiterBeginApplicationStructure(BeginApplicationStructure beginApplicationStructure, PrintContext parameter)
        {
            parameter.WriteLine("Begin Application Structure: {0} '{1}'", beginApplicationStructure.Type, beginApplicationStructure.Identifier);
            parameter.BeginLevel();
        }
        public void AcceptDelimiterBeginApplicationStructureBody(BeginApplicationStructureBody beginApplicationStructureBody, PrintContext parameter)
        {
            parameter.WriteLine("Begin Application Structure Body");
        }
        public void AcceptDelimiterEndApplicationStructure(EndApplicationStructure endApplicationStructure, PrintContext parameter)
        {
            parameter.EndLevel();
        }
        public void AcceptMetafileDescriptorMetafileVersion(MetafileVersion metafileVersion, PrintContext parameter)
        {
            parameter.WriteLine("Metafile Version: {0}", metafileVersion.Version);
        }
        public void AcceptMetafileDescriptorMetafileDescription(MetafileDescription metafileDescription, PrintContext parameter)
        {
            parameter.WriteLine("Metafile Description: {0}", metafileDescription.Description);
        }
        public void AcceptMetafileDescriptorVdcType(VdcType vdcType, PrintContext parameter)
        {
            parameter.WriteLine("VDC Type: {0}", vdcType.Specification);
        }
        public void AcceptMetafileDescriptorIntegerPrecision(IntegerPrecision integerPrecision, PrintContext parameter)
        {
            parameter.WriteLine("Integer Precision: {0} bit", integerPrecision.Precision);
        }
        public void AcceptMetafileDescriptorRealPrecision(RealPrecision realPrecision, PrintContext parameter)
        {
            if (realPrecision.Specification == RealPrecisionSpecification.Unsupported)
                parameter.WriteLine("Real Precision: Unsupported ({0}, {1} bit Exponent width, {2} bit Fraction width)",
                    realPrecision.RepresentationForm, realPrecision.ExponentWidth, realPrecision.FractionWidth);
            else
                parameter.WriteLine("Real Precision: {0}", realPrecision.Specification);
        }
        public void AcceptMetafileDescriptorIndexPrecision(IndexPrecision indexPrecision, PrintContext parameter)
        {
            parameter.WriteLine("Index Precision: {0} bit", indexPrecision.Precision);
        }
        public void AcceptMetafileDescriptorColorPrecision(ColorPrecision colorPrecision, PrintContext parameter)
        {
            parameter.WriteLine("Color Precision: {0} bit", colorPrecision.Precision);
        }
        public void AcceptMetafileDescriptorColorIndexPrecision(ColorIndexPrecision colorIndexPrecision, PrintContext parameter)
        {
            parameter.WriteLine("Color Index Precision: {0} bit", colorIndexPrecision.Precision);
        }
        public void AcceptMetafileDescriptorMaximumColorIndex(MaximumColorIndex maximumColorIndex, PrintContext parameter)
        {
            parameter.WriteLine("Maximum Color Index: {0}", maximumColorIndex.Index);
        }
        public void AcceptMetafileDescriptorColorValueExtent(ColorValueExtent colorValueExtent, PrintContext parameter)
        {
            if (colorValueExtent.ColorSpace == ColorSpace.Unknown)
                parameter.WriteLine("Color Value Extent: Unknown Color Space");
            else if (colorValueExtent.ColorSpace == ColorSpace.CIE)
                parameter.WriteLine("Color Value Extent: CIE {0}+{1}/{2}+{3}/{4}+{5}",
                    colorValueExtent.FirstScale, colorValueExtent.FirstOffset,
                    colorValueExtent.SecondScale, colorValueExtent.SecondOffset,
                    colorValueExtent.ThirdScale, colorValueExtent.ThirdOffset);
            else // RGB or CMYK
                parameter.WriteLine("Color Value Extent: {0} {1}/{2}",
                    colorValueExtent.ColorSpace, colorValueExtent.Minimum, colorValueExtent.Maximum);
        }
        public void AcceptMetafileDescriptorMetafileElementsList(MetafileElementsList metafileElementsList, PrintContext parameter)
        {
            parameter.WriteLine("Metafile Elements List: {0} entries", metafileElementsList.Elements.Count());
            parameter.BeginLevel();
            foreach (string metafileElement in metafileElementsList.Elements)
                parameter.WriteLine(metafileElement);
            parameter.EndLevel();
        }
        public void AcceptMetafileDescriptorMetafileDefaultsReplacement(MetafileDefaultsReplacement metafileDefaultsReplacement, PrintContext parameter)
        {
            parameter.WriteLine("Metafile Defaults Replacement: {0} entries", metafileDefaultsReplacement.Commands.Count());
            parameter.BeginLevel();
            foreach (var command in metafileDefaultsReplacement.Commands)
                command.Accept(this, parameter);
            parameter.EndLevel();
        }
        public void AcceptMetafileDescriptorFontList(FontList fontList, PrintContext parameter)
        {
            parameter.WriteLine("Font List: {0} entries", fontList.Fonts.Count());
            parameter.BeginLevel();
            foreach (string font in fontList.Fonts)
                parameter.WriteLine(font);
            parameter.EndLevel();
        }
        public void AcceptMetafileDescriptorCharacterSetList(CharacterSetList characterSetList, PrintContext parameter)
        {
            parameter.WriteLine("Character Set List: {0} entries", characterSetList.Entries.Count());
            parameter.BeginLevel();
            foreach (var entry in characterSetList.Entries)
                parameter.WriteLine("{0} (Tail {1})",
                    entry.CharacterSetType,
                    string.Join(", ", entry.DesignationSequenceTail.Select(c => ((int)c).ToString("x2"))));
            parameter.EndLevel();
        }
        public void AcceptMetafileDescriptorCharacterCodingAnnouncer(CharacterCodingAnnouncer characterCodingAnnouncer, PrintContext parameter)
        {
            parameter.WriteLine("Character Coding Announcer: {0}", characterCodingAnnouncer.CharacterCodingAnnouncerType);
        }
        public void AcceptMetafileDescriptorNamePrecision(NamePrecision namePrecision, PrintContext parameter)
        {
            parameter.WriteLine("Name Precision: {0} bit", namePrecision.Precision);
        }
        public void AcceptMetafileDescriptorMaximumVdcExtent(MaximumVdcExtent maximumVdcExtent, PrintContext parameter)
        {
            parameter.WriteLine("Maximum VDC Extent: {0} - {1}", maximumVdcExtent.FirstCorner, maximumVdcExtent.SecondCorner);
        }
        public void AcceptMetafileDescriptorColorModel(ColorModelCommand colorModel, PrintContext parameter)
        {
            parameter.WriteLine("Color Model: {0}", colorModel.ColorModel);
        }

        public void AcceptPictureDescriptorScalingMode(ScalingMode scalingMode, PrintContext parameter)
        {
            if (scalingMode.ScalingModeType == ScalingModeType.Metric)
                parameter.WriteLine("Scaling Mode: {0} (Factor {1})", scalingMode.ScalingModeType, scalingMode.MetricScalingFactor);
            else
                parameter.WriteLine("Scaling Mode: {0}", scalingMode.ScalingModeType);
        }
        public void AcceptPictureDescriptorColorSelectionMode(ColorSelectionMode colorSelectionMode, PrintContext parameter)
        {
            parameter.WriteLine("Color Selection Mode: {0}", colorSelectionMode.ColorMode);
        }
        public void AcceptPictureDescriptorLineWidthSpecificationMode(LineWidthSpecificationMode lineWidthSpecificationMode, PrintContext parameter)
        {
            parameter.WriteLine("Line Width Specification Mode: {0}", lineWidthSpecificationMode.WidthSpecificationMode);
        }
        public void AcceptPictureDescriptorMarkerSizeSpecificationMode(MarkerSizeSpecificationMode markerSizeSpecificationMode, PrintContext parameter)
        {
            parameter.WriteLine("Marker Size Specification Mode: {0}", markerSizeSpecificationMode.WidthSpecificationMode);
        }
        public void AcceptPictureDescriptorEdgeWidthSpecificationMode(EdgeWidthSpecificationMode edgeWidthSpecificationMode, PrintContext parameter)
        {
            parameter.WriteLine("Edge Width Specification Mode: {0}", edgeWidthSpecificationMode.WidthSpecificationMode);
        }
        public void AcceptPictureDescriptorVdcExtent(VdcExtent vdcExtent, PrintContext parameter)
        {
            parameter.WriteLine("VDC Extent: {0} - {1}", vdcExtent.FirstCorner, vdcExtent.SecondCorner);
        }
        public void AcceptPictureDescriptorBackgroundColor(BackgroundColor backgroundColor, PrintContext parameter)
        {
            parameter.WriteLine("Background Color: {0}", backgroundColor.Color);
        }
        public void AcceptPictureDescriptorDeviceViewport(DeviceViewport deviceViewport, PrintContext parameter)
        {
            parameter.WriteLine("Device Viewport: {0} - {1}", deviceViewport.FirstCorner, deviceViewport.SecondCorner);
        }
        public void AcceptPictureDescriptorDeviceViewportSpecificationMode(DeviceViewportSpecificationMode deviceViewportSpecificationMode, PrintContext parameter)
        {
            if (deviceViewportSpecificationMode.SpecificationMode == DeviceViewportSpecificationModeType.MillimetersWithScaleFactor)
                parameter.WriteLine("Scaling Mode: {0} (Factor {1})", deviceViewportSpecificationMode.SpecificationMode, deviceViewportSpecificationMode.ScaleFactor);
            else
                parameter.WriteLine("Scaling Mode: {0}", deviceViewportSpecificationMode.SpecificationMode);
        }
        public void AcceptPictureDescriptorInteriorStyleSpecificationMode(InteriorStyleSpecificationMode interiorStyleSpecificationMode, PrintContext parameter)
        {
            parameter.WriteLine("Interior Style Specification Mode: {0}", interiorStyleSpecificationMode.WidthSpecificationMode);
        }
        public void AcceptPictureDescriptorLineAndEdgeTypeDefinition(LineAndEdgeTypeDefinition lineAndEdgeTypeDefinition, PrintContext parameter)
        {
            parameter.WriteLine("Line and Edge Type Definition: {0} (dash cycle repeat length: {1}, {2} elements)",
                lineAndEdgeTypeDefinition.LineType, lineAndEdgeTypeDefinition.DashCycleRepeatLength, lineAndEdgeTypeDefinition.DashElements.Length);
        }
        public void AcceptPictureDescriptorHatchStyleDefinition(HatchStyleDefinition hatchStyleDefinition, PrintContext parameter)
        {
            parameter.WriteLine("Hatch Style Definition: {0} (duty cycle length: {1}, {2} gaps, from {3} to {4})",
                hatchStyleDefinition.HatchIndex, hatchStyleDefinition.DutyCycleLength, hatchStyleDefinition.GapWidths.Length,
                hatchStyleDefinition.HatchDirectionStart, hatchStyleDefinition.HatchDirectionEnd);
        }
        public void AcceptPictureDescriptorGeometricPatternDefinition(GeometricPatternDefinition geometricPatternDefinition, PrintContext parameter)
        {
            parameter.WriteLine("Geometric Pattern Definition: {0} (segment {1}, from {2} to {3})",
                geometricPatternDefinition.GeometricPatternIndex, geometricPatternDefinition.SegmentIdentifier,
                geometricPatternDefinition.FirstCorner, geometricPatternDefinition.SecondCorner);
        }

        public void AcceptControlVdcIntegerPrecision(VdcIntegerPrecision vdcIntegerPrecision, PrintContext parameter)
        {
            parameter.WriteLine("VDC Integer Precision: {0} bit", vdcIntegerPrecision.Precision);
        }
        public void AcceptControlVdcRealPrecision(VdcRealPrecision vdcRealPrecision, PrintContext parameter)
        {
            if (vdcRealPrecision.Specification == RealPrecisionSpecification.Unsupported)
                parameter.WriteLine("VDC Real Precision: Unsupported ({0}, {1} bit Exponent width, {2} bit Fraction width)",
                    vdcRealPrecision.RepresentationForm, vdcRealPrecision.ExponentWidth, vdcRealPrecision.FractionWidth);
            else
                parameter.WriteLine("VDC Real Precision: {0}", vdcRealPrecision.Specification);
        }
        public void AcceptControlAuxiliaryColor(AuxiliaryColor auxiliaryColor, PrintContext parameter)
        {
            parameter.WriteLine("Auxiliary Color: {0}", auxiliaryColor.Color);
        }
        public void AcceptControlTransparency(Transparency transparency, PrintContext parameter)
        {
            parameter.WriteLine("Transparency: {0}", transparency.Indicator);
        }
        public void AcceptControlClipRectangle(ClipRectangle clipRectangle, PrintContext parameter)
        {
            parameter.WriteLine("Clip Rectangle: {0} - {1}", clipRectangle.FirstCorner, clipRectangle.SecondCorner);
        }
        public void AcceptControlClipIndicator(ClipIndicator clipIndicator, PrintContext parameter)
        {
            parameter.WriteLine("Clip Indicator: {0}", clipIndicator.Indicator);
        }
        public void AcceptControlLineClippingMode(LineClippingMode lineClippingMode, PrintContext parameter)
        {
            parameter.WriteLine("Line Clipping Mode: {0}", lineClippingMode.Mode);
        }
        public void AcceptControlMarkerClippingMode(MarkerClippingMode markerClippingMode, PrintContext parameter)
        {
            parameter.WriteLine("Marker Clipping Mode: {0}", markerClippingMode.Mode);
        }
        public void AcceptControlEdgeClippingMode(EdgeClippingMode edgeClippingMode, PrintContext parameter)
        {
            parameter.WriteLine("Edge Clipping Mode: {0}", edgeClippingMode.Mode);
        }
        public void AcceptControlNewRegion(NewRegion newRegion, PrintContext parameter)
        {
            parameter.WriteLine("New Region");
        }
        public void AcceptControlSavePrimitiveContext(SavePrimitiveContext savePrimitiveContext, PrintContext parameter)
        {
            parameter.WriteLine("Save Primitive Context: {0}", savePrimitiveContext.ContextName);
        }
        public void AcceptControlRestorePrimitiveContext(RestorePrimitiveContext restorePrimitiveContext, PrintContext parameter)
        {
            parameter.WriteLine("Restore Primitive Context: {0}", restorePrimitiveContext.ContextName);
        }
        public void AcceptControlProtectionRegionIndicator(ProtectionRegionIndicator protectionRegionIndicator, PrintContext parameter)
        {
            parameter.WriteLine("Protection Region Indicator: {0} ({1})", protectionRegionIndicator.Index, protectionRegionIndicator.Indicator);
        }
        public void AcceptControlGeneralizedTextPathMode(GeneralizedTextPathMode generalizedTextPathMode, PrintContext parameter)
        {
            parameter.WriteLine("Generalized Text Path Mode: {0}", generalizedTextPathMode.Mode);
        }
        public void AcceptControlMiterLimit(MiterLimit miterLimit, PrintContext parameter)
        {
            parameter.WriteLine("Mitre Limit: {0}", miterLimit.Limit);
        }

        public void AcceptGraphicalPrimitivePolyline(Polyline polyline, PrintContext parameter)
        {
            parameter.WriteLine("Polyline: {0} points", polyline.Points.Length);
        }
        public void AcceptGraphicalPrimitiveText(TextCommand text, PrintContext parameter)
        {
            parameter.WriteLine("Text: '{0}' (at {1})", text.Text, text.Position);
        }
        public void AcceptGraphicalPrimitiveRestrictedText(RestrictedText restrictedText, PrintContext parameter)
        {
            parameter.WriteLine("Restricted Text: '{0}' (at {1}, +/-{2} by {3})",
                restrictedText.Text, restrictedText.Position, restrictedText.DeltaWidth, restrictedText.DeltaHeight);
        }
        public void AcceptGraphicalPrimitiveAppendText(AppendText appendText, PrintContext parameter)
        {
            parameter.WriteLine("Append Text: '{0}'", appendText.Text);
        }
        public void AcceptGraphicalPrimitivePolygon(Polygon polygon, PrintContext parameter)
        {
            parameter.WriteLine("Polygon: {0} points", polygon.Points.Length);
        }
        public void AcceptGraphicalPrimitiveRectangle(Rectangle rectangle, PrintContext parameter)
        {
            parameter.WriteLine("Rectangle: {0} - {1}", rectangle.FirstCorner, rectangle.SecondCorner);
        }
        public void AcceptGraphicalPrimitiveCircle(Circle circle, PrintContext parameter)
        {
            parameter.WriteLine("Circle: {0} @ {1}", circle.Center, circle.Radius);
        }
        public void AcceptGraphicalPrimitiveCircularArcCenter(CircularArcCenter circularArcCenter, PrintContext parameter)
        {
            parameter.WriteLine("Circular Arc Center: {0} @ {1} ({2} to {3})", circularArcCenter.Center, circularArcCenter.Radius, circularArcCenter.Start, circularArcCenter.End);
        }
        public void AcceptGraphicalPrimitiveEllipse(Ellipse ellipse, PrintContext parameter)
        {
            parameter.WriteLine("Ellipse: {0} @ {1} - {2}", ellipse.Center, ellipse.FirstConjugateDiameter, ellipse.SecondConjugateDiameter);
        }
        public void AcceptGraphicalPrimitiveEllipticalArc(EllipticalArc ellipticalArc, PrintContext parameter)
        {
            parameter.WriteLine("Elliptical Arc: {0} @ {1} - {2} ({3} to {4})",
                ellipticalArc.Center, ellipticalArc.FirstConjugateDiameter, ellipticalArc.SecondConjugateDiameter, ellipticalArc.Start, ellipticalArc.End);
        }

        public void AcceptAttributeLineBundleIndex(LineBundleIndex lineBundleIndex, PrintContext parameter)
        {
            parameter.WriteLine("Line Bundle Index: {0}", lineBundleIndex.Index);
        }
        public void AcceptAttributeLineType(LineType lineType, PrintContext parameter)
        {
            parameter.WriteLine("Line Type: {0} ({1})", lineType.Index, lineType.Name);
        }
        public void AcceptAttributeLineWidth(LineWidth lineWidth, PrintContext parameter)
        {
            parameter.WriteLine("Line Width: {0}", lineWidth.Width);
        }
        public void AcceptAttributeLineColor(LineColor lineColor, PrintContext parameter)
        {
            parameter.WriteLine("Line Color: {0}", lineColor.Color);
        }
        public void AcceptAttributeMarkerBundleIndex(MarkerBundleIndex markerBundleIndex, PrintContext parameter)
        {
            parameter.WriteLine("Marker Bundle Index: {0}", markerBundleIndex.Index);
        }
        public void AcceptAttributeMarkerType(MarkerType markerType, PrintContext parameter)
        {
            parameter.WriteLine("Marker Type: {0} ({1})", markerType.Index, markerType.Name);
        }
        public void AcceptAttributeMarkerSize(MarkerSize markerSize, PrintContext parameter)
        {
            parameter.WriteLine("Marker Size: {0}", markerSize.Size);
        }
        public void AcceptAttributeMarkerColor(MarkerColor markerColor, PrintContext parameter)
        {
            parameter.WriteLine("Marker Color: {0}", markerColor.Color);
        }
        public void AcceptAttributeTextBundleIndex(TextBundleIndex textBundleIndex, PrintContext parameter)
        {
            parameter.WriteLine("Text Bundle Index: {0}", textBundleIndex.Index);
        }

        public void AcceptApplicationStructureDescriptorAttribute(ApplicationStructureAttribute applicationStructureAttribute, PrintContext parameter)
        {
            parameter.WriteLine("Attribute: {0} '{1}'", applicationStructureAttribute.AttributeType, applicationStructureAttribute.DataRecord);
        }

        public void AcceptUnsupportedCommand(UnsupportedCommand unsupportedCommand, PrintContext parameter)
        {
            // do nothing; otherwise we'd probably spam the command line
        }
    }
}
