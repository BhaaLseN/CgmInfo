using System;
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
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Traversal
{
    public class ViewModelBuilderVisitor : ICommandVisitor<MetafileContext>
    {
        public void AcceptDelimiterBeginMetafile(BeginMetafile beginMetafile, MetafileContext parameter)
        {
            parameter.BeginLevel(new MetafileViewModel(beginMetafile.Name));
        }

        public void AcceptDelimiterEndMetafile(EndMetafile endMetafile, MetafileContext parameter)
        {
            parameter.EndLevel("END METAFILE");
        }

        public void AcceptDelimiterBeginPicture(BeginPicture beginPicture, MetafileContext parameter)
        {
            parameter.BeginLevel(new PictureViewModel(beginPicture.Name));
        }

        public void AcceptDelimiterBeginPictureBody(BeginPictureBody beginPictureBody, MetafileContext parameter)
        {
            parameter.AddNode("BEGIN PICTURE BODY");
        }

        public void AcceptDelimiterEndPicture(EndPicture endPicture, MetafileContext parameter)
        {
            parameter.EndLevel("END PICTURE");
        }

        public void AcceptDelimiterBeginSegment(BeginSegment beginSegment, MetafileContext parameter)
        {
            parameter.BeginLevel(new SegmentViewModel(beginSegment.Identifier));
        }

        public void AcceptDelimiterEndSegment(EndSegment endSegment, MetafileContext parameter)
        {
            parameter.EndLevel("END SEGMENT");
        }

        public void AcceptDelimiterBeginFigure(BeginFigure beginFigure, MetafileContext parameter)
        {
            parameter.BeginLevel("BEGIN FIGURE");
        }

        public void AcceptDelimiterEndFigure(EndFigure endFigure, MetafileContext parameter)
        {
            parameter.EndLevel("END FIGURE");
        }

        public void AcceptDelimiterBeginProtectionRegion(BeginProtectionRegion beginProtectionRegion, MetafileContext parameter)
        {
            parameter.BeginLevel("BEGIN PROTECTION REGION: {0}", beginProtectionRegion.RegionIndex);
        }

        public void AcceptDelimiterEndProtectionRegion(EndProtectionRegion endProtectionRegion, MetafileContext parameter)
        {
            parameter.EndLevel("END PROTECTION REGION");
        }
        public void AcceptDelimiterBeginCompoundLine(BeginCompoundLine beginCompoundLine, MetafileContext parameter)
        {
            parameter.BeginLevel("BEGIN COMPOUND LINE");
        }

        public void AcceptDelimiterEndCompoundLine(EndCompoundLine endCompoundLine, MetafileContext parameter)
        {
            parameter.EndLevel("END COMPOUND LINE");
        }
        public void AcceptDelimiterBeginCompoundTextPath(BeginCompoundTextPath beginCompoundTextPath, MetafileContext parameter)
        {
            parameter.BeginLevel("BEGIN COMPOUND TEXT PATH");
        }

        public void AcceptDelimiterEndCompoundTextPath(EndCompoundTextPath endCompoundTextPath, MetafileContext parameter)
        {
            parameter.EndLevel("END COMPOUND TEXT PATH");
        }

        public void AcceptDelimiterBeginTileArray(BeginTileArray beginTileArray, MetafileContext parameter)
        {
            parameter.BeginLevel(new TileArrayViewModel(beginTileArray));
        }

        public void AcceptDelimiterEndTileArray(EndTileArray endTileArray, MetafileContext parameter)
        {
            parameter.EndLevel("END TILE ARRAY");
        }

        public void AcceptDelimiterBeginApplicationStructure(BeginApplicationStructure beginApplicationStructure, MetafileContext parameter)
        {
            parameter.BeginLevel(new ApplicationStructureViewModel(beginApplicationStructure));
        }

        public void AcceptDelimiterBeginApplicationStructureBody(BeginApplicationStructureBody beginApplicationStructureBody, MetafileContext parameter)
        {
            parameter.AddNode("BEGIN APPLICATION STRUCTURE BODY");
        }

        public void AcceptDelimiterEndApplicationStructure(EndApplicationStructure endApplicationStructure, MetafileContext parameter)
        {
            parameter.EndLevel("END APPLICATION STRUCTURE");
        }

        public void AcceptMetafileDescriptorColorIndexPrecision(ColorIndexPrecision colorIndexPrecision, MetafileContext parameter)
        {
            parameter.AddMetafileDescriptorNode("COLOUR INDEX PRECISION: {0} bit", colorIndexPrecision.Precision);
        }

        public void AcceptMetafileDescriptorColorModel(ColorModelCommand colorModel, MetafileContext parameter)
        {
            parameter.AddMetafileDescriptorNode("COLOUR MODEL: {0}", colorModel.ColorModel);
        }

        public void AcceptMetafileDescriptorColorPrecision(ColorPrecision colorPrecision, MetafileContext parameter)
        {
            parameter.AddMetafileDescriptorNode("COLOUR PRECISION: {0} bit", colorPrecision.Precision);
        }

        public void AcceptMetafileDescriptorColorValueExtent(ColorValueExtent colorValueExtent, MetafileContext parameter)
        {
            var extentNode = parameter.AddMetafileDescriptorNode("COLOUR VALUE EXTENT: Color Space {0}", colorValueExtent.ColorSpace);
            if (colorValueExtent.ColorSpace == ColorSpace.CIE)
            {
                extentNode.Nodes.AddRange(new[]
                {
                    new SimpleNode(string.Format("First Scale: {0}", colorValueExtent.FirstScale)),
                    new SimpleNode(string.Format("First Offset: {0}", colorValueExtent.FirstOffset)),
                    new SimpleNode(string.Format("Second Scale: {0}", colorValueExtent.SecondScale)),
                    new SimpleNode(string.Format("Second Offset: {0}", colorValueExtent.SecondOffset)),
                    new SimpleNode(string.Format("Third Scale: {0}", colorValueExtent.ThirdScale)),
                    new SimpleNode(string.Format("Third Offset: {0}", colorValueExtent.ThirdOffset)),
                });
            }
            else if (colorValueExtent.ColorSpace != ColorSpace.Unknown) // RGB or CMYK
            {
                extentNode.Nodes.AddRange(new[]
                {
                    new SimpleNode(string.Format("Minimum: {0}", colorValueExtent.Minimum)),
                    new SimpleNode(string.Format("Maximum: {0}", colorValueExtent.Maximum)),
                });
            }
        }

        public void AcceptMetafileDescriptorIndexPrecision(IndexPrecision indexPrecision, MetafileContext parameter)
        {
            parameter.AddMetafileDescriptorNode("INDEX PRECISION: {0} bit", indexPrecision.Precision);
        }

        public void AcceptMetafileDescriptorIntegerPrecision(IntegerPrecision integerPrecision, MetafileContext parameter)
        {
            parameter.AddMetafileDescriptorNode("INTEGER PRECISION: {0} bit", integerPrecision.Precision);
        }

        public void AcceptMetafileDescriptorMaximumColorIndex(MaximumColorIndex maximumColorIndex, MetafileContext parameter)
        {
            parameter.AddMetafileDescriptorNode("MAXIMUM COLOUR INDEX: {0}", maximumColorIndex.Index);
        }

        public void AcceptMetafileDescriptorMetafileDescription(MetafileDescription metafileDescription, MetafileContext parameter)
        {
            parameter.AddDescriptorNode(new MetafileDescriptionViewModel(metafileDescription.Description));
        }

        public void AcceptMetafileDescriptorMetafileVersion(MetafileVersion metafileVersion, MetafileContext parameter)
        {
            parameter.AddMetafileDescriptorNode("METAFILE VERSION: {0}", metafileVersion.Version);
        }

        public void AcceptMetafileDescriptorNamePrecision(NamePrecision namePrecision, MetafileContext parameter)
        {
            parameter.AddMetafileDescriptorNode("NAME PRECISION: {0} bit", namePrecision.Precision);
        }

        public void AcceptMetafileDescriptorRealPrecision(RealPrecision realPrecision, MetafileContext parameter)
        {
            var realNode = parameter.AddMetafileDescriptorNode("REAL PRECISION: {0}", realPrecision.RepresentationForm);
            realNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Exponent Width: {0} bit", realPrecision.ExponentWidth)),
                new SimpleNode(string.Format("Fraction Width: {0} bit", realPrecision.FractionWidth)),
            });
        }

        public void AcceptMetafileDescriptorVdcType(VdcType vdcType, MetafileContext parameter)
        {
            parameter.AddMetafileDescriptorNode("VDC TYPE: {0}", vdcType.Specification);
        }

        public void AcceptMetafileDescriptorMetafileElementsList(MetafileElementsList metafileElementsList, MetafileContext parameter)
        {
            var metafileElementsListNode = parameter.AddMetafileDescriptorNode("METAFILE ELEMENTS LIST [{0} entries]", metafileElementsList.Elements.Count());
            metafileElementsListNode.Nodes.AddRange(metafileElementsList.Elements.Select(entry => new SimpleNode(entry)));
        }

        public void AcceptMetafileDescriptorMetafileDefaultsReplacement(MetafileDefaultsReplacement metafileDefaultsReplacement, MetafileContext parameter)
        {
            var metafileElementsListNode = parameter.AddMetafileDescriptorNode("METAFILE DEFAULTS REPLACEMENT [{0} entries]", metafileDefaultsReplacement.Commands.Count());
            parameter.BeginLevel(metafileElementsListNode, true);
            foreach (var command in metafileDefaultsReplacement.Commands)
                command.Accept(this, parameter);
            parameter.EndLevel();
        }

        public void AcceptMetafileDescriptorFontList(FontList fontList, MetafileContext parameter)
        {
            parameter.AddDescriptorNode(new FontListViewModel(fontList));
        }

        public void AcceptMetafileDescriptorCharacterSetList(CharacterSetList characterSetList, MetafileContext parameter)
        {
            var characterSetNode = parameter.AddMetafileDescriptorNode("CHARACTER SET LIST [{0} entries]", characterSetList.Entries.Count());
            characterSetNode.Nodes.AddRange(
                characterSetList.Entries.Select(entry => new SimpleNode(string.Format("{0} (Tail {1})",
                    entry.CharacterSetType,
                    string.Join(", ", entry.DesignationSequenceTail.Select(c => ((int)c).ToString("x2")))))));
        }

        public void AcceptMetafileDescriptorCharacterCodingAnnouncer(CharacterCodingAnnouncer characterCodingAnnouncer, MetafileContext parameter)
        {
            parameter.AddMetafileDescriptorNode("CHARACTER CODING ANNOUNCER: {0}", characterCodingAnnouncer.CharacterCodingAnnouncerType);
        }

        public void AcceptMetafileDescriptorMaximumVdcExtent(MaximumVdcExtent maximumVdcExtent, MetafileContext parameter)
        {
            var maxVdcNode = parameter.AddMetafileDescriptorNode("MAXIMUM VDC EXTENT: {0} by {1}",
                Math.Abs(maximumVdcExtent.SecondCorner.X - maximumVdcExtent.FirstCorner.X),
                Math.Abs(maximumVdcExtent.SecondCorner.Y - maximumVdcExtent.FirstCorner.Y));
            maxVdcNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("First Corner: {0}", maximumVdcExtent.FirstCorner)),
                new SimpleNode(string.Format("Second Corner: {0}", maximumVdcExtent.SecondCorner)),
            });
        }

        public void AcceptPictureDescriptorScalingMode(ScalingMode scalingMode, MetafileContext parameter)
        {
            var scalingModeNode = parameter.AddNode("SCALING MODE: {0}", scalingMode.ScalingModeType);
            if (scalingMode.ScalingModeType == ScalingModeType.Metric)
                scalingModeNode.Add(new SimpleNode(string.Format("Factor: {0}", scalingMode.MetricScalingFactor)));
        }
        public void AcceptPictureDescriptorColorSelectionMode(ColorSelectionMode colorSelectionMode, MetafileContext parameter)
        {
            parameter.AddNode("COLOUR SELECTION MODE: {0}", colorSelectionMode.ColorMode);
        }
        public void AcceptPictureDescriptorLineWidthSpecificationMode(LineWidthSpecificationMode lineWidthSpecificationMode, MetafileContext parameter)
        {
            parameter.AddNode("LINE WIDTH SPECIFICATION MODE: {0}", lineWidthSpecificationMode.WidthSpecificationMode);
        }
        public void AcceptPictureDescriptorMarkerSizeSpecificationMode(MarkerSizeSpecificationMode markerSizeSpecificationMode, MetafileContext parameter)
        {
            parameter.AddNode("MARKER SIZE SPECIFICATION MODE: {0}", markerSizeSpecificationMode.WidthSpecificationMode);
        }
        public void AcceptPictureDescriptorEdgeWidthSpecificationMode(EdgeWidthSpecificationMode edgeWidthSpecificationMode, MetafileContext parameter)
        {
            parameter.AddNode("EDGE WIDTH SPECIFICATION MODE: {0}", edgeWidthSpecificationMode.WidthSpecificationMode);
        }
        public void AcceptPictureDescriptorVdcExtent(VdcExtent vdcExtent, MetafileContext parameter)
        {
            var maxVdcNode = parameter.AddNode("VDC EXTENT: {0} by {1}",
                Math.Abs(vdcExtent.SecondCorner.X - vdcExtent.FirstCorner.X),
                Math.Abs(vdcExtent.SecondCorner.Y - vdcExtent.FirstCorner.Y));
            maxVdcNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("First Corner: {0}", vdcExtent.FirstCorner)),
                new SimpleNode(string.Format("Second Corner: {0}", vdcExtent.SecondCorner)),
            });
        }

        public void AcceptPictureDescriptorBackgroundColor(BackgroundColor backgroundColor, MetafileContext parameter)
        {
            parameter.AddNode("BACKGROUND COLOUR: {0}", backgroundColor.Color);
        }

        public void AcceptPictureDescriptorDeviceViewport(DeviceViewport deviceViewport, MetafileContext parameter)
        {
            var deviceViewportNode = parameter.AddNode("DEVICE VIEWPORT: {0} by {1}",
                Math.Abs(deviceViewport.SecondCorner.X - deviceViewport.FirstCorner.X),
                Math.Abs(deviceViewport.SecondCorner.Y - deviceViewport.FirstCorner.Y));
            deviceViewportNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("First Corner: {0}", deviceViewport.FirstCorner)),
                new SimpleNode(string.Format("Second Corner: {0}", deviceViewport.SecondCorner)),
            });
        }

        public void AcceptPictureDescriptorDeviceViewportSpecificationMode(DeviceViewportSpecificationMode deviceViewportSpecificationMode, MetafileContext parameter)
        {
            var specificationModeNode = parameter.AddNode("DEVICE VIEWPORT SPECIFICATION MODE: {0}", deviceViewportSpecificationMode.SpecificationMode);
            if (deviceViewportSpecificationMode.SpecificationMode == DeviceViewportSpecificationModeType.MillimetersWithScaleFactor)
                specificationModeNode.Add(new SimpleNode(string.Format("Factor: {0}", deviceViewportSpecificationMode.ScaleFactor)));
        }

        public void AcceptPictureDescriptorInteriorStyleSpecificationMode(InteriorStyleSpecificationMode interiorStyleSpecificationMode, MetafileContext parameter)
        {
            parameter.AddNode("INTERIOR STYLE SPECIFICATION MODE: {0}", interiorStyleSpecificationMode.WidthSpecificationMode);
        }

        public void AcceptControlVdcIntegerPrecision(VdcIntegerPrecision vdcIntegerPrecision, MetafileContext parameter)
        {
            parameter.AddNode("VDC INTEGER PRECISION: {0} bit", vdcIntegerPrecision.Precision);
        }

        public void AcceptPictureDescriptorLineAndEdgeTypeDefinition(LineAndEdgeTypeDefinition lineAndEdgeTypeDefinition, MetafileContext parameter)
        {
            var lineAndEdgeTypeDefinitionNode = parameter.AddNode("LINE AND EDGE TYPE DEFINITION: {0}", lineAndEdgeTypeDefinition.LineType);
            lineAndEdgeTypeDefinitionNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Line Type Index: {0}", lineAndEdgeTypeDefinition.LineType)),
                new SimpleNode(string.Format("Dash Cycle Repeat Length: {0}", lineAndEdgeTypeDefinition.DashCycleRepeatLength)),
            });
            var entries = new SimpleNode(string.Format("Dash Elements [{0} elements]", lineAndEdgeTypeDefinition.DashElements.Length));
            entries.Nodes.AddRange(lineAndEdgeTypeDefinition.DashElements.Select(i => new SimpleNode(i.ToString())));
            lineAndEdgeTypeDefinitionNode.Nodes.Add(entries);
        }

        public void AcceptPictureDescriptorHatchStyleDefinition(HatchStyleDefinition hatchStyleDefinition, MetafileContext parameter)
        {
            var hatchStyleDefinitionNode = parameter.AddNode("HATCH STYLE DEFINITION: {0}", hatchStyleDefinition.HatchIndex);
            hatchStyleDefinitionNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Hatch Index: {0}", hatchStyleDefinition.HatchIndex)),
                new SimpleNode(string.Format("Style Indicator: {0}", hatchStyleDefinition.StyleIndicator)),
                new SimpleNode(string.Format("Hatch Direction Start: {0}", hatchStyleDefinition.HatchDirectionStart)),
                new SimpleNode(string.Format("Hatch Direction End: {0}", hatchStyleDefinition.HatchDirectionEnd)),
                new SimpleNode(string.Format("Duty Cycle Length: {0}", hatchStyleDefinition.DutyCycleLength)),
            });
            var gapWidths = new SimpleNode(string.Format("Gap Widths [{0} elements]", hatchStyleDefinition.GapWidths.Length));
            gapWidths.Nodes.AddRange(hatchStyleDefinition.GapWidths.Select(i => new SimpleNode(i.ToString())));
            hatchStyleDefinitionNode.Nodes.Add(gapWidths);
            var lineTypes = new SimpleNode(string.Format("Line Types [{0} elements]", hatchStyleDefinition.LineTypes.Length));
            lineTypes.Nodes.AddRange(hatchStyleDefinition.LineTypes.Select(i => new SimpleNode(i.ToString())));
            hatchStyleDefinitionNode.Nodes.Add(lineTypes);
        }

        public void AcceptPictureDescriptorGeometricPatternDefinition(GeometricPatternDefinition geometricPatternDefinition, MetafileContext parameter)
        {
            var lineAndEdgeTypeDefinitionNode = parameter.AddNode("GEOMETRIC PATTERN DEFINITION: {0}", geometricPatternDefinition.GeometricPatternIndex);
            lineAndEdgeTypeDefinitionNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Geometric Pattern Index: {0}", geometricPatternDefinition.GeometricPatternIndex)),
                new SimpleNode(string.Format("Segment Identifier: {0}", geometricPatternDefinition.SegmentIdentifier)),
                new SimpleNode(string.Format("First Corner: {0}", geometricPatternDefinition.FirstCorner)),
                new SimpleNode(string.Format("Second Corner: {0}", geometricPatternDefinition.SecondCorner)),
            });
        }

        public void AcceptControlVdcRealPrecision(VdcRealPrecision vdcRealPrecision, MetafileContext parameter)
        {
            var realNode = parameter.AddNode("VDC REAL PRECISION: {0}", vdcRealPrecision.RepresentationForm);
            realNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Exponent Width: {0} bit", vdcRealPrecision.ExponentWidth)),
                new SimpleNode(string.Format("Fraction Width: {0} bit", vdcRealPrecision.FractionWidth)),
            });
        }

        public void AcceptControlAuxiliaryColor(AuxiliaryColor auxiliaryColor, MetafileContext parameter)
        {
            parameter.AddNode("AUXILIARY COLOR: {0}", auxiliaryColor.Color);
        }

        public void AcceptControlTransparency(Transparency transparency, MetafileContext parameter)
        {
            parameter.AddNode("TRANSPARENCY: {0}", transparency.Indicator);
        }

        public void AcceptControlClipRectangle(ClipRectangle clipRectangle, MetafileContext parameter)
        {
            var clipRectNode = parameter.AddNode("CLIP RECTANGLE: {0} by {1}",
                Math.Abs(clipRectangle.SecondCorner.X - clipRectangle.FirstCorner.X),
                Math.Abs(clipRectangle.SecondCorner.Y - clipRectangle.FirstCorner.Y));
            clipRectNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("First Corner: {0}", clipRectangle.FirstCorner)),
                new SimpleNode(string.Format("Second Corner: {0}", clipRectangle.SecondCorner)),
            });
        }

        public void AcceptControlClipIndicator(ClipIndicator clipIndicator, MetafileContext parameter)
        {
            parameter.AddNode("CLIP INDICATOR: {0}", clipIndicator.Indicator);
        }

        public void AcceptControlLineClippingMode(LineClippingMode lineClippingMode, MetafileContext parameter)
        {
            parameter.AddNode("LINE CLIPPING MODE: {0}", lineClippingMode.Mode);
        }

        public void AcceptControlMarkerClippingMode(MarkerClippingMode markerClippingMode, MetafileContext parameter)
        {
            parameter.AddNode("MARKER CLIPPING MODE: {0}", markerClippingMode.Mode);
        }

        public void AcceptControlEdgeClippingMode(EdgeClippingMode edgeClippingMode, MetafileContext parameter)
        {
            parameter.AddNode("EDGE CLIPPING MODE: {0}", edgeClippingMode.Mode);
        }

        public void AcceptControlNewRegion(NewRegion newRegion, MetafileContext parameter)
        {
            parameter.AddNode("NEW REGION");
        }

        public void AcceptControlSavePrimitiveContext(SavePrimitiveContext savePrimitiveContext, MetafileContext parameter)
        {
            parameter.AddNode("SAVE PRIMITIVE CONTEXT: {0}", savePrimitiveContext.ContextName);
        }

        public void AcceptControlRestorePrimitiveContext(RestorePrimitiveContext restorePrimitiveContext, MetafileContext parameter)
        {
            parameter.AddNode("RESTORE PRIMITIVE CONTEXT: {0}", restorePrimitiveContext.ContextName);
        }

        public void AcceptControlProtectionRegionIndicator(ProtectionRegionIndicator protectionRegionIndicator, MetafileContext parameter)
        {
            parameter.AddNode("PROTECTION REGION INDICATOR: {0} ({1})", protectionRegionIndicator.Index, protectionRegionIndicator.Indicator);
        }

        public void AcceptControlGeneralizedTextPathMode(GeneralizedTextPathMode generalizedTextPathMode, MetafileContext parameter)
        {
            parameter.AddNode("GENERALIZED TEXT PATH MODE: {0}", generalizedTextPathMode.Mode);
        }

        public void AcceptControlMiterLimit(MiterLimit miterLimit, MetafileContext parameter)
        {
            parameter.AddNode("MITRE LIMIT: {0}", miterLimit.Limit);
        }

        public void AcceptGraphicalPrimitivePolyline(Polyline polyline, MetafileContext parameter)
        {
            var node = parameter.AddNode("POLYLINE: {0} points", polyline.Points.Length);
            node.Nodes.AddRange(polyline.Points.Select(p => new SimpleNode(p.ToString())));
        }
        public void AcceptGraphicalPrimitiveText(TextCommand text, MetafileContext parameter)
        {
            var node = parameter.AddNode("TEXT: '{0}'{1}", text.Text, text.Final == FinalFlag.Final ? " (final)" : "");
            node.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Position [{0}]", text.Position))
                {
                    new SimpleNode(string.Format("X: {0}", text.Position.X)),
                    new SimpleNode(string.Format("Y: {0}", text.Position.Y)),
                },
            });
        }
        public void AcceptGraphicalPrimitiveRestrictedText(RestrictedText restrictedText, MetafileContext parameter)
        {
            var node = parameter.AddNode("RESTRICTED TEXT: '{0}'{1}", restrictedText.Text, restrictedText.Final == FinalFlag.Final ? " (final)" : "");
            node.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Position [{0}]", restrictedText.Position))
                {
                    new SimpleNode(string.Format("X: {0}", restrictedText.Position.X)),
                    new SimpleNode(string.Format("Y: {0}", restrictedText.Position.Y)),
                },
                new SimpleNode(string.Format("Bounding Box [{0}; {1}]", restrictedText.DeltaWidth, restrictedText.DeltaHeight))
                {
                    new SimpleNode(string.Format("Delta Width: {0}", restrictedText.DeltaWidth)),
                    new SimpleNode(string.Format("Delta Height: {0}", restrictedText.DeltaHeight)),
                },
            });
        }
        public void AcceptGraphicalPrimitiveAppendText(AppendText appendText, MetafileContext parameter)
        {
            parameter.AddNode("APPEND TEXT: '{0}'{1}", appendText.Text, appendText.Final == FinalFlag.Final ? " (final)" : "");
        }
        public void AcceptGraphicalPrimitivePolygon(Polygon polygon, MetafileContext parameter)
        {
            var node = parameter.AddNode("POLYGON: {0} points", polygon.Points.Length);
            node.Nodes.AddRange(polygon.Points.Select(p => new SimpleNode(p.ToString())));
        }
        public void AcceptGraphicalPrimitiveRectangle(Rectangle rectangle, MetafileContext parameter)
        {
            var rectNode = parameter.AddNode("RECTANGLE: {0} by {1}",
                Math.Abs(rectangle.SecondCorner.X - rectangle.FirstCorner.X),
                Math.Abs(rectangle.SecondCorner.Y - rectangle.FirstCorner.Y));
            rectNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("First Corner: {0}", rectangle.FirstCorner)),
                new SimpleNode(string.Format("Second Corner: {0}", rectangle.SecondCorner)),
            });
        }
        public void AcceptGraphicalPrimitiveCircle(Circle circle, MetafileContext parameter)
        {
            var circleNode = parameter.AddNode("CIRCLE: {0} by {0}", circle.Radius);
            circleNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Center: {0}", circle.Center)),
                new SimpleNode(string.Format("Radius: {0}", circle.Radius)),
            });
        }
        public void AcceptGraphicalPrimitiveCircularArcCenter(CircularArcCenter circularArcCenter, MetafileContext parameter)
        {
            var circlarArcNode = parameter.AddNode("CIRCULAR ARC CENTRE: {0} by {0}", circularArcCenter.Radius);
            circlarArcNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Center: {0}", circularArcCenter.Center)),
                new SimpleNode(string.Format("Start: {0}", circularArcCenter.Start)),
                new SimpleNode(string.Format("End: {0}", circularArcCenter.End)),
                new SimpleNode(string.Format("Radius: {0}", circularArcCenter.Radius)),
            });
        }
        public void AcceptGraphicalPrimitiveEllipse(Ellipse ellipse, MetafileContext parameter)
        {
            var ellipseNode = parameter.AddNode("ELLIPSE: {0}", ellipse.Center);
            ellipseNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Center: {0}", ellipse.Center)),
                new SimpleNode(string.Format("First Conjugate Diameter: {0}", ellipse.FirstConjugateDiameter)),
                new SimpleNode(string.Format("Second Conjugate Diameter: {0}", ellipse.SecondConjugateDiameter)),
            });
        }
        public void AcceptGraphicalPrimitiveEllipticalArc(EllipticalArc ellipticalArc, MetafileContext parameter)
        {
            var ellipseNode = parameter.AddNode("ELLIPTICAL ARC: {0}", ellipticalArc.Center);
            ellipseNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Center: {0}", ellipticalArc.Center)),
                new SimpleNode(string.Format("First Conjugate Diameter: {0}", ellipticalArc.FirstConjugateDiameter)),
                new SimpleNode(string.Format("Second Conjugate Diameter: {0}", ellipticalArc.SecondConjugateDiameter)),
                new SimpleNode(string.Format("Start: {0}", ellipticalArc.Start)),
                new SimpleNode(string.Format("End: {0}", ellipticalArc.End)),
            });
        }

        public void AcceptAttributeLineBundleIndex(LineBundleIndex lineBundleIndex, MetafileContext parameter)
        {
            parameter.AddNode("LINE BUNDLE INDEX: {0}", lineBundleIndex.Index);
        }
        public void AcceptAttributeLineType(LineType lineType, MetafileContext parameter)
        {
            parameter.AddNode("LINE TYPE: {0} ({1})", lineType.Index, lineType.Name);
        }
        public void AcceptAttributeLineWidth(LineWidth lineWidth, MetafileContext parameter)
        {
            parameter.AddNode("LINE WIDTH: {0}", lineWidth.Width);
        }
        public void AcceptAttributeLineColor(LineColor lineColor, MetafileContext parameter)
        {
            parameter.AddNode("LINE COLOUR: {0}", lineColor.Color);
        }
        public void AcceptAttributeMarkerBundleIndex(MarkerBundleIndex markerBundleIndex, MetafileContext parameter)
        {
            parameter.AddNode("MARKER BUNDLE INDEX: {0}", markerBundleIndex.Index);
        }
        public void AcceptAttributeMarkerType(MarkerType markerType, MetafileContext parameter)
        {
            parameter.AddNode("MARKER TYPE: {0} ({1})", markerType.Index, markerType.Name);
        }

        public void AcceptApplicationStructureDescriptorAttribute(ApplicationStructureAttribute applicationStructureAttribute, MetafileContext parameter)
        {
            var aps = parameter.AddNode("{0}", applicationStructureAttribute.AttributeType);
            aps.Nodes.AddRange(applicationStructureAttribute.DataRecord.Elements.SelectMany(e => e.Values).Select(e => new SimpleNode(Convert.ToString(e))));
        }

        public void AcceptUnsupportedCommand(UnsupportedCommand unsupportedCommand, MetafileContext parameter)
        {
            parameter.AddUnsupportedNode(unsupportedCommand);
        }
    }
}
