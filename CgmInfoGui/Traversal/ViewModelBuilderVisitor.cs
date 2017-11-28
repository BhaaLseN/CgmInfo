using System;
using System.Linq;
using CgmInfo.Commands;
using CgmInfo.Commands.ApplicationStructureDescriptor;
using CgmInfo.Commands.Attributes;
using CgmInfo.Commands.Delimiter;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.Escape;
using CgmInfo.Commands.External;
using CgmInfo.Commands.GraphicalPrimitives;
using CgmInfo.Commands.MetafileDescriptor;
using CgmInfo.Commands.PictureDescriptor;
using CgmInfo.Commands.Segment;
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

        public void AcceptMetafileDescriptorSegmentPriorityExtent(SegmentPriorityExtent segmentPriorityExtent, MetafileContext parameter)
        {
            parameter.AddMetafileDescriptorNode("SEGMENT PRIORITY EXTENT: {0} to {1}",
                segmentPriorityExtent.MinimumPriorityValue,
                segmentPriorityExtent.MaximumPriorityValue);
        }

        public void AcceptMetafileDescriptorFontProperties(FontProperties fontProperties, MetafileContext parameter)
        {
            var fontPropNode = parameter.AddMetafileDescriptorNode("FONT PROPERTIES [{0} elements]", fontProperties.Properties.Length);
            fontPropNode.Nodes.AddRange(fontProperties.Properties.Select(p => new SimpleNode(string.Format("{0} ({1}), priority {2}", p.Indicator, p.Name, p.Priority))
            {
                new SimpleNode(string.Format("Priority: {0}", p.Priority)),
                new SimpleNode(string.Format("{0}: {1}", p.Record.Type, string.Join(", ", p.Record.Values))),
            }));
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
        public void AcceptGraphicalPrimitiveDisjointPolyline(DisjointPolyline disjointPolyline, MetafileContext parameter)
        {
            var node = parameter.AddNode("DISJOINT POLYLINE: {0} points", disjointPolyline.Points.Length);
            node.Nodes.AddRange(disjointPolyline.Points.Select(p => new SimpleNode(p.ToString())));
        }
        public void AcceptGraphicalPrimitivePolymarker(Polymarker polymarker, MetafileContext parameter)
        {
            var node = parameter.AddNode("POLYMARKER: {0} points", polymarker.Points.Length);
            node.Nodes.AddRange(polymarker.Points.Select(p => new SimpleNode(p.ToString())));
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
        public void AcceptGraphicalPrimitivePolygonSet(PolygonSet polygonSet, MetafileContext parameter)
        {
            var node = parameter.AddNode("POLYGON SET: {0} points", polygonSet.Points.Length);
            node.Nodes.AddRange(polygonSet.Points.Select((p, i) => new SimpleNode(string.Format("{0} ({1})", p, polygonSet.Flags[i]))));
        }
        public void AcceptGraphicalPrimitiveCellArray(CellArray cellArray, MetafileContext parameter)
        {
            var cellArrayNode = parameter.AddNode("CELL ARRAY: {0} by {1}", cellArray.NX, cellArray.NY);
            cellArrayNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Corner Point P: {0}", cellArray.CornerPointP)),
                new SimpleNode(string.Format("Corner Point Q: {0}", cellArray.CornerPointQ)),
                new SimpleNode(string.Format("Corner Point R: {0}", cellArray.CornerPointR)),
            });
            for (int y = 0; y < cellArray.NY; y++)
            {
                var rowNode = new SimpleNode(string.Format("Row {0}", y));
                for (int x = 0; x < cellArray.NX; x++)
                {
                    rowNode.Nodes.Add(new SimpleNode(cellArray.Colors[y * cellArray.NX + x].ToString()));
                }
                cellArrayNode.Nodes.Add(rowNode);
            }
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
        public void AcceptGraphicalPrimitiveCircularArc3Point(CircularArc3Point circularArc3Point, MetafileContext parameter)
        {
            var circularArcNode = parameter.AddNode("CIRCULAR ARC 3 POINT: {0} to {1} to {2}",
                circularArc3Point.Start, circularArc3Point.Intermediate, circularArc3Point.End);
            circularArcNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Start: {0}", circularArc3Point.Start)),
                new SimpleNode(string.Format("Intermediate: {0}", circularArc3Point.Intermediate)),
                new SimpleNode(string.Format("End: {0}", circularArc3Point.End)),
            });
        }
        public void AcceptGraphicalPrimitiveCircularArc3PointClose(CircularArc3PointClose circularArc3PointClose, MetafileContext parameter)
        {
            var circularArcNode = parameter.AddNode("CIRCULAR ARC 3 POINT CLOSE: {0} to {1} to {2} ({3})",
                circularArc3PointClose.Start, circularArc3PointClose.Intermediate, circularArc3PointClose.End, circularArc3PointClose.Closure);
            circularArcNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Start: {0}", circularArc3PointClose.Start)),
                new SimpleNode(string.Format("Intermediate: {0}", circularArc3PointClose.Intermediate)),
                new SimpleNode(string.Format("End: {0}", circularArc3PointClose.End)),
                new SimpleNode(string.Format("Arc Closure: {0}", circularArc3PointClose.Closure)),
            });
        }
        public void AcceptGraphicalPrimitiveCircularArcCenter(CircularArcCenter circularArcCenter, MetafileContext parameter)
        {
            var circularArcNode = parameter.AddNode("CIRCULAR ARC CENTRE: {0} by {0}", circularArcCenter.Radius);
            circularArcNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Center: {0}", circularArcCenter.Center)),
                new SimpleNode(string.Format("Start: {0}", circularArcCenter.Start)),
                new SimpleNode(string.Format("End: {0}", circularArcCenter.End)),
                new SimpleNode(string.Format("Radius: {0}", circularArcCenter.Radius)),
            });
        }
        public void AcceptGraphicalPrimitiveCircularArcCenterClose(CircularArcCenterClose circularArcCenterClose, MetafileContext parameter)
        {
            var circularArcNode = parameter.AddNode("CIRCULAR ARC CENTRE CLOSE: {0} by {0} ({1})", circularArcCenterClose.Radius, circularArcCenterClose.Closure);
            circularArcNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Center: {0}", circularArcCenterClose.Center)),
                new SimpleNode(string.Format("Start: {0}", circularArcCenterClose.Start)),
                new SimpleNode(string.Format("End: {0}", circularArcCenterClose.End)),
                new SimpleNode(string.Format("Radius: {0}", circularArcCenterClose.Radius)),
                new SimpleNode(string.Format("Arc Closure: {0}", circularArcCenterClose.Closure)),
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
        public void AcceptGraphicalPrimitiveEllipticalArcClose(EllipticalArcClose ellipticalArcClose, MetafileContext parameter)
        {
            var ellipseNode = parameter.AddNode("ELLIPTICAL ARC CLOSE: {0} ({1})", ellipticalArcClose.Center, ellipticalArcClose.Closure);
            ellipseNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Center: {0}", ellipticalArcClose.Center)),
                new SimpleNode(string.Format("First Conjugate Diameter: {0}", ellipticalArcClose.FirstConjugateDiameter)),
                new SimpleNode(string.Format("Second Conjugate Diameter: {0}", ellipticalArcClose.SecondConjugateDiameter)),
                new SimpleNode(string.Format("Start: {0}", ellipticalArcClose.Start)),
                new SimpleNode(string.Format("End: {0}", ellipticalArcClose.End)),
                new SimpleNode(string.Format("Arc Closure: {0}", ellipticalArcClose.Closure)),
            });
        }
        public void AcceptGraphicalPrimitiveCircularArcCenterReversed(CircularArcCenterReversed circularArcCenterReversed, MetafileContext parameter)
        {
            var circularArcNode = parameter.AddNode("CIRCULAR ARC CENTRE REVERSED: {0} by {0}", circularArcCenterReversed.Radius);
            circularArcNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Center: {0}", circularArcCenterReversed.Center)),
                new SimpleNode(string.Format("Start: {0}", circularArcCenterReversed.Start)),
                new SimpleNode(string.Format("End: {0}", circularArcCenterReversed.End)),
                new SimpleNode(string.Format("Radius: {0}", circularArcCenterReversed.Radius)),
            });
        }
        public void AcceptGraphicalPrimitiveConnectingEdge(ConnectingEdge connectingEdge, MetafileContext parameter)
        {
            parameter.AddNode("CONNECTING EDGE");
        }
        public void AcceptGraphicalPrimitiveHyperbolicArc(HyperbolicArc hyperbolicArc, MetafileContext parameter)
        {
            var arcNode = parameter.AddNode("HYPERBOLIC ARC: {0}", hyperbolicArc.Center);
            arcNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Center: {0}", hyperbolicArc.Center)),
                new SimpleNode(string.Format("Traverse Radius End Point: {0}", hyperbolicArc.TraverseRadiusEndPoint)),
                new SimpleNode(string.Format("Conjugate Radius End Point: {0}", hyperbolicArc.ConjugateRadiusEndPoint)),
                new SimpleNode(string.Format("Start: {0}", hyperbolicArc.Start)),
                new SimpleNode(string.Format("End: {0}", hyperbolicArc.End)),
            });
        }
        public void AcceptGraphicalPrimitiveParabolicArc(ParabolicArc parabolicArc, MetafileContext parameter)
        {
            var arcNode = parameter.AddNode("PARABOLIC ARC: {0}", parabolicArc.TangentIntersectionPoint);
            arcNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Tangent Intersection Point: {0}", parabolicArc.TangentIntersectionPoint)),
                new SimpleNode(string.Format("Start: {0}", parabolicArc.Start)),
                new SimpleNode(string.Format("End: {0}", parabolicArc.End)),
            });
        }
        public void AcceptGraphicalPrimitiveNonUniformBSpline(NonUniformBSpline nonUniformBSpline, MetafileContext parameter)
        {
            var splineNode = parameter.AddNode("NON-UNIFORM B-SPLINE: {0} ({1} points)",
                nonUniformBSpline.SplineOrder, nonUniformBSpline.ControlPoints.Length);
            var controlPointsNode = new SimpleNode(string.Format("Control Points [{0} elements]", nonUniformBSpline.ControlPoints.Length));
            controlPointsNode.Nodes.AddRange(nonUniformBSpline.ControlPoints.Select(n => new SimpleNode(n.ToString())));
            var knotsNode = new SimpleNode(string.Format("Knots [{0} elements]", nonUniformBSpline.Knots.Length));
            knotsNode.Nodes.AddRange(nonUniformBSpline.Knots.Select(n => new SimpleNode(n.ToString())));
            splineNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Spline Order: {0}", nonUniformBSpline.SplineOrder)),
                controlPointsNode,
                knotsNode,
                new SimpleNode(string.Format("Start: {0}", nonUniformBSpline.Start)),
                new SimpleNode(string.Format("End: {0}", nonUniformBSpline.End)),
            });
        }
        public void AcceptGraphicalPrimitiveNonUniformRationalBSpline(NonUniformRationalBSpline nonUniformRationalBSpline, MetafileContext parameter)
        {
            var splineNode = parameter.AddNode("NON-UNIFORM RATIONAL B-SPLINE: {0} ({1} points)",
                nonUniformRationalBSpline.SplineOrder, nonUniformRationalBSpline.ControlPoints.Length);
            var controlPointsNode = new SimpleNode(string.Format("Control Points [{0} elements]", nonUniformRationalBSpline.ControlPoints.Length));
            controlPointsNode.Nodes.AddRange(nonUniformRationalBSpline.ControlPoints.Select(n => new SimpleNode(n.ToString())));
            var knotsNode = new SimpleNode(string.Format("Knots [{0} elements]", nonUniformRationalBSpline.Knots.Length));
            knotsNode.Nodes.AddRange(nonUniformRationalBSpline.Knots.Select(n => new SimpleNode(n.ToString())));
            var weightsNode = new SimpleNode(string.Format("Weights [{0} elements]", nonUniformRationalBSpline.Weights.Length));
            weightsNode.Nodes.AddRange(nonUniformRationalBSpline.Weights.Select(n => new SimpleNode(n.ToString())));
            splineNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Spline Order: {0}", nonUniformRationalBSpline.SplineOrder)),
                controlPointsNode,
                knotsNode,
                weightsNode,
                new SimpleNode(string.Format("Start: {0}", nonUniformRationalBSpline.Start)),
                new SimpleNode(string.Format("End: {0}", nonUniformRationalBSpline.End)),
            });
        }
        public void AcceptGraphicalPrimitivePolybezier(Polybezier polybezier, MetafileContext parameter)
        {
            var bezierNode = parameter.AddNode("POLYBEZIER: {0} ({1}) [{2} elements]", polybezier.ContinuityIndicator, polybezier.Name, polybezier.PointSequences.Length);
            bezierNode.Nodes.AddRange(polybezier.PointSequences.Select(n => new SimpleNode(n.ToString())));
        }
        public void AcceptGraphicalPrimitiveBitonalTile(BitonalTile bitonalTile, MetafileContext parameter)
        {
            var bitonalTileNode = parameter.AddNode("BITONAL TILE: {0} ({1})", bitonalTile.CompressionType, bitonalTile.CompressionTypeName);
            bitonalTileNode.Nodes.Add(new SimpleNode("Row Padding Indicator: " + bitonalTile.RowPaddingIndicator));
            bitonalTileNode.Nodes.Add(new SimpleNode("Cell Background Color: " + bitonalTile.CellBackgroundColor));
            bitonalTileNode.Nodes.Add(new SimpleNode("Cell Foreground Color: " + bitonalTile.CellForegroundColor));
            if (bitonalTile.Parameters != null)
            {
                var parameterNode = new SimpleNode($"Parameters [{bitonalTile.Parameters.Elements.Count()}]");
                parameterNode.Nodes.AddRange(bitonalTile.Parameters.Elements.Select(n => new SimpleNode(n.ToString())));
                bitonalTileNode.Nodes.Add(parameterNode);
            }
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
        public void AcceptAttributeMarkerSize(MarkerSize markerSize, MetafileContext parameter)
        {
            parameter.AddNode("MARKER SIZE: {0}", markerSize.Size);
        }
        public void AcceptAttributeMarkerColor(MarkerColor markerColor, MetafileContext parameter)
        {
            parameter.AddNode("MARKER COLOUR: {0}", markerColor.Color);
        }
        public void AcceptAttributeTextBundleIndex(TextBundleIndex textBundleIndex, MetafileContext parameter)
        {
            parameter.AddNode("TEXT BUNDLE INDEX: {0}", textBundleIndex.Index);
        }
        public void AcceptAttributeTextFontIndex(TextFontIndex textFontIndex, MetafileContext parameter)
        {
            parameter.AddNode("TEXT FONT INDEX: {0}", textFontIndex.Index);
        }
        public void AcceptAttributeTextPrecision(TextPrecision textPrecision, MetafileContext parameter)
        {
            parameter.AddNode("TEXT PRECISION: {0}", textPrecision.Precision);
        }
        public void AcceptAttributeCharacterExpansionFactor(CharacterExpansionFactor characterExpansionFactor, MetafileContext parameter)
        {
            parameter.AddNode("CHARACTER EXPANSION FACTOR: {0}", characterExpansionFactor.Factor);
        }
        public void AcceptAttributeCharacterSpacing(CharacterSpacing characterSpacing, MetafileContext parameter)
        {
            parameter.AddNode("CHARACTER SPACING: {0}", characterSpacing.AdditionalIntercharacterSpace);
        }
        public void AcceptAttributeTextColor(TextColor textColor, MetafileContext parameter)
        {
            parameter.AddNode("TEXT COLOUR: {0}", textColor.Color);
        }
        public void AcceptAttributeCharacterHeight(CharacterHeight characterHeight, MetafileContext parameter)
        {
            parameter.AddNode("CHARACTER HEIGHT: {0}", characterHeight.Height);
        }
        public void AcceptAttributeCharacterOrientation(CharacterOrientation characterOrientation, MetafileContext parameter)
        {
            var charOrientNode = parameter.AddNode("CHARACTER ORIENTATION: {0}/{1}", characterOrientation.Up, characterOrientation.Base);
            charOrientNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Up: {0}", characterOrientation.Up)),
                new SimpleNode(string.Format("Base: {0}", characterOrientation.Base)),
            });
        }
        public void AcceptAttributeTextPath(TextPath textPath, MetafileContext parameter)
        {
            parameter.AddNode("TEXT PATH: {0}", textPath.Path);
        }
        public void AcceptAttributeTextAlignment(TextAlignment textAlignment, MetafileContext parameter)
        {
            var textAlignNode = parameter.AddNode("TEXT ALIGNMENT: {0}/{1}", textAlignment.Horizontal, textAlignment.Vertical);
            textAlignNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Horizontal: {0}", textAlignment.Horizontal)),
                new SimpleNode(string.Format("Vertical: {0}", textAlignment.Vertical)),
            });
            if (textAlignment.Horizontal == HorizontalTextAlignment.Continuous)
                textAlignNode.Add(new SimpleNode(string.Format("Horizontal Continuous Alignment: {0}", textAlignment.HorizontalContinuousAlignment)));
            if (textAlignment.Vertical == VerticalTextAlignment.Continuous)
                textAlignNode.Add(new SimpleNode(string.Format("Vertical Continuous Alignment: {0}", textAlignment.VerticalContinuousAlignment)));
        }
        public void AcceptAttributeCharacterSetIndex(CharacterSetIndex characterSetIndex, MetafileContext parameter)
        {
            parameter.AddNode("CHARACTER SET INDEX: {0}", characterSetIndex.Index);
        }
        public void AcceptAttributeAlternateCharacterSetIndex(AlternateCharacterSetIndex alternateCharacterSetIndex, MetafileContext parameter)
        {
            parameter.AddNode("ALTERNATE CHARACTER SET INDEX: {0}", alternateCharacterSetIndex.Index);
        }
        public void AcceptAttributeFillBundleIndex(FillBundleIndex fillBundleIndex, MetafileContext parameter)
        {
            parameter.AddNode("FILL BUNDLE INDEX: {0}", fillBundleIndex.Index);
        }
        public void AcceptAttributeInteriorStyle(InteriorStyle interiorStyle, MetafileContext parameter)
        {
            parameter.AddNode("INTERIOR STYLE: {0}", interiorStyle.Style);
        }
        public void AcceptAttributeFillColor(FillColor fillColor, MetafileContext parameter)
        {
            parameter.AddNode("FILL COLOUR: {0}", fillColor.Color);
        }
        public void AcceptAttributeHatchIndex(HatchIndex hatchIndex, MetafileContext parameter)
        {
            parameter.AddNode("HATCH INDEX: {0} ({1})", hatchIndex.Index, hatchIndex.Name);
        }
        public void AcceptAttributePatternIndex(PatternIndex patternIndex, MetafileContext parameter)
        {
            parameter.AddNode("PATTERN INDEX: {0}", patternIndex.Index);
        }
        public void AcceptAttributeEdgeBundleIndex(EdgeBundleIndex edgeBundleIndex, MetafileContext parameter)
        {
            parameter.AddNode("EDGE BUNDLE INDEX: {0}", edgeBundleIndex.Index);
        }
        public void AcceptAttributeEdgeType(EdgeType edgeType, MetafileContext parameter)
        {
            parameter.AddNode("EDGE TYPE: {0} ({1})", edgeType.Index, edgeType.Name);
        }
        public void AcceptAttributeEdgeWidth(EdgeWidth edgeWidth, MetafileContext parameter)
        {
            parameter.AddNode("EDGE WIDTH: {0}", edgeWidth.Width);
        }
        public void AcceptAttributeEdgeColor(EdgeColor edgeColor, MetafileContext parameter)
        {
            parameter.AddNode("EDGE COLOUR: {0}", edgeColor.Color);
        }
        public void AcceptAttributeEdgeVisibility(EdgeVisibility edgeVisibility, MetafileContext parameter)
        {
            parameter.AddNode("EDGE VISIBILITY: {0}", edgeVisibility.Visibility);
        }
        public void AcceptAttributeFillReferencePoint(FillReferencePoint fillReferencePoint, MetafileContext parameter)
        {
            parameter.AddNode("FILL REFERENCE POINT: {0}", fillReferencePoint.ReferencePoint);
        }
        public void AcceptAttributePatternTable(PatternTable patternTable, MetafileContext parameter)
        {
            var patternTableNode = parameter.AddNode("PATTERN TABLE: {0} ({1} by {2})",
                patternTable.Index, patternTable.Height, patternTable.Width);
            patternTableNode.Nodes.AddRange(patternTable.Colors.Select(c => new SimpleNode(c.ToString())));
        }
        public void AcceptAttributePatternSize(PatternSize patternSize, MetafileContext parameter)
        {
            var patternSizeNode = parameter.AddNode("PATTERN SIZE: {0} by {1}", patternSize.Height, patternSize.Width);
            patternSizeNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Height: {0}", patternSize.Height)),
                new SimpleNode(string.Format("Width: {0}", patternSize.Width)),
            });
        }
        public void AcceptAttributeColorTable(ColorTable colorTable, MetafileContext parameter)
        {
            var colorTableNode = parameter.AddNode("COLOUR TABLE: update from {0} with {1} colors", colorTable.StartIndex, colorTable.Colors.Length);
            colorTableNode.Nodes.AddRange(colorTable.Colors.Select((c, i) => new SimpleNode(string.Format("{0}: {1}", colorTable.StartIndex + i, c))));
        }
        public void AcceptAttributeAspectSourceFlags(AspectSourceFlags aspectSourceFlags, MetafileContext parameter)
        {
            var asfNode = parameter.AddNode("ASPECT SOURCE FLAGS [{0} entries]", aspectSourceFlags.Values.Count);
            asfNode.Nodes.AddRange(aspectSourceFlags.Values.Select(kvp => new SimpleNode(string.Format("{0}: {1}", kvp.Key, kvp.Value))));
        }
        public void AcceptAttributePickIdentifier(PickIdentifier pickIdentifier, MetafileContext parameter)
        {
            parameter.AddNode("PICK IDENTIFIER: {0}", pickIdentifier.Identifier);
        }
        public void AcceptAttributeLineCap(LineCap lineCap, MetafileContext parameter)
        {
            var lineCapNode = parameter.AddNode("LINE CAP: line {0} ({1}), dash {2} ({3})",
                lineCap.LineCapIndicator, lineCap.LineCapName,
                lineCap.DashCapIndicator, lineCap.DashCapName);
            lineCapNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Line Cap Indicator: {0} ({1})", lineCap.LineCapIndicator, lineCap.LineCapName)),
                new SimpleNode(string.Format("Dash Cap Indicator: {0} ({1})", lineCap.DashCapIndicator, lineCap.DashCapName)),
            });
        }
        public void AcceptAttributeLineJoin(LineJoin lineJoin, MetafileContext parameter)
        {
            parameter.AddNode("LINE JOIN: {0} ({1})", lineJoin.Index, lineJoin.Name);
        }
        public void AcceptAttributeLineTypeContinuation(LineTypeContinuation lineTypeContinuation, MetafileContext parameter)
        {
            parameter.AddNode("LINE TYPE CONTINUATION: {0} ({1})", lineTypeContinuation.Index, lineTypeContinuation.Name);
        }
        public void AcceptAttributeLineTypeInitialOffset(LineTypeInitialOffset lineTypeInitialOffset, MetafileContext parameter)
        {
            parameter.AddNode("LINE TYPE INITIAL OFFSET: {0}", lineTypeInitialOffset.Offset);
        }
        public void AcceptAttributeRestrictedTextType(RestrictedTextType restrictedTextType, MetafileContext parameter)
        {
            parameter.AddNode("RESTRICTED TEXT TYPE: {0} ({1})", restrictedTextType.Index, restrictedTextType.Name);
        }
        public void AcceptAttributeInterpolatedInterior(InterpolatedInterior interpolatedInterior, MetafileContext parameter)
        {
            var interpolatedInteriorNode = parameter.AddNode("INTERPOLATED INTERIOR: {0} ({1})",
                interpolatedInterior.Index, interpolatedInterior.Name);
            var referenceGeometryNode = new SimpleNode(string.Format("Reference Geometry: [{0} entries]", interpolatedInterior.ReferenceGeometry.Length));
            referenceGeometryNode.Nodes.AddRange(interpolatedInterior.ReferenceGeometry.Select(rg => new SimpleNode(rg.ToString())));
            var stageDesignatorsNode = new SimpleNode(string.Format("Stage Designators [{0} entries]", interpolatedInterior.StageDesignators.Length));
            stageDesignatorsNode.Nodes.AddRange(interpolatedInterior.StageDesignators.Select(d => new SimpleNode(d.ToString())));
            var colorSpecifiersNode = new SimpleNode(string.Format("Color Specifiers [{0} entries]", interpolatedInterior.ColorSpecifiers.Length));
            colorSpecifiersNode.Nodes.AddRange(interpolatedInterior.ColorSpecifiers.Select(c => new SimpleNode(c.ToString())));
            interpolatedInteriorNode.Nodes.AddRange(new[]
            {
                referenceGeometryNode,
                stageDesignatorsNode,
                colorSpecifiersNode,
            });
        }
        public void AcceptAttributeEdgeCap(EdgeCap edgeCap, MetafileContext parameter)
        {
            var edgeCapNode = parameter.AddNode("EDGE CAP: line {0} ({1}), dash {2} ({3})",
                edgeCap.EdgeCapIndicator, edgeCap.EdgeCapName,
                edgeCap.DashCapIndicator, edgeCap.DashCapName);
            edgeCapNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Edge Cap Indicator: {0} ({1})", edgeCap.EdgeCapIndicator, edgeCap.EdgeCapName)),
                new SimpleNode(string.Format("Dash Cap Indicator: {0} ({1})", edgeCap.DashCapIndicator, edgeCap.DashCapName)),
            });
        }
        public void AcceptAttributeEdgeJoin(EdgeJoin edgeJoin, MetafileContext parameter)
        {
            parameter.AddNode("EDGE JOIN: {0} ({1})", edgeJoin.Index, edgeJoin.Name);
        }
        public void AcceptAttributeEdgeTypeContinuation(EdgeTypeContinuation edgeTypeContinuation, MetafileContext parameter)
        {
            parameter.AddNode("EDGE TYPE CONTINUATION: {0} ({1})", edgeTypeContinuation.Index, edgeTypeContinuation.Name);
        }
        public void AcceptAttributeEdgeTypeInitialOffset(EdgeTypeInitialOffset edgeTypeInitialOffset, MetafileContext parameter)
        {
            parameter.AddNode("EDGE TYPE INITIAL OFFSET: {0}", edgeTypeInitialOffset.Offset);
        }

        public void AcceptEscapeEscape(EscapeCommand escapeCommand, MetafileContext parameter)
        {
            var esc = parameter.AddNode("ESCAPE: {0} ({1})", escapeCommand.Identifier, escapeCommand.Name);
            esc.Nodes.AddRange(escapeCommand.DataRecord.Elements.Select(e =>
            {
                var recordNode = new SimpleNode(e.Type.ToString());
                recordNode.Nodes.AddRange(e.Values.Select(v => new SimpleNode(Convert.ToString(v))));
                return recordNode;
            }));
        }

        public void AcceptExternalMessage(Message message, MetafileContext parameter)
        {
            var msgNode = parameter.AddNode("MESSAGE: {0}", message.ActionRequired);
            msgNode.Add(new SimpleNode(message.MessageString));
        }
        public void AcceptExternalApplicationData(ApplicationData applicationData, MetafileContext parameter)
        {
            var appData = parameter.AddNode("APPLICATION DATA: {0}", applicationData.Identifier);
            appData.Add(new SimpleNode(applicationData.DataRecord));
        }

        public void AcceptSegmentCopySegment(CopySegment copySegment, MetafileContext parameter)
        {
            var copySegmentNode = parameter.AddNode("COPY SEGMENT: {0}", copySegment.SegmentIdentifier);
            copySegmentNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("Transformation applied: {0}", copySegment.TransformationApplication)),
                new SimpleNode(string.Format("X Scale: {0}", copySegment.Matrix.Elements[0])),
                new SimpleNode(string.Format("X Rotation: {0}", copySegment.Matrix.Elements[1])),
                new SimpleNode(string.Format("Y Rotation: {0}", copySegment.Matrix.Elements[2])),
                new SimpleNode(string.Format("Y Scale: {0}", copySegment.Matrix.Elements[3])),
                new SimpleNode(string.Format("X Translation: {0}", copySegment.Matrix.Elements[4])),
                new SimpleNode(string.Format("Y Translation: {0}", copySegment.Matrix.Elements[5])),
            });
        }
        public void AcceptSegmentInheritanceFilter(InheritanceFilter inheritanceFilter, MetafileContext parameter)
        {
            var inheritanceFilterNode = parameter.AddNode("INHERITANCE FILTER [{0} entries]", inheritanceFilter.Items.Length);
            inheritanceFilterNode.Nodes.AddRange(inheritanceFilter.Items.Select(i => new SimpleNode(string.Format("{0}: {1}", i.Designator, i.Setting))));
        }
        public void AcceptSegmentClipInheritance(ClipInheritance clipInheritance, MetafileContext parameter)
        {
            parameter.AddNode("CLIP INHERITANCE: {0}", clipInheritance.InheritanceType);
        }
        public void AcceptSegmentSegmentTransformation(SegmentTransformation segmentTransformation, MetafileContext parameter)
        {
            var segmentTransformationNode = parameter.AddNode("SEGMENT TRANSFORMATION: {0}", segmentTransformation.SegmentIdentifier);
            segmentTransformationNode.Nodes.AddRange(new[]
            {
                new SimpleNode(string.Format("X Scale: {0}", segmentTransformation.Matrix.Elements[0])),
                new SimpleNode(string.Format("X Rotation: {0}", segmentTransformation.Matrix.Elements[1])),
                new SimpleNode(string.Format("Y Rotation: {0}", segmentTransformation.Matrix.Elements[2])),
                new SimpleNode(string.Format("Y Scale: {0}", segmentTransformation.Matrix.Elements[3])),
                new SimpleNode(string.Format("X Translation: {0}", segmentTransformation.Matrix.Elements[4])),
                new SimpleNode(string.Format("Y Translation: {0}", segmentTransformation.Matrix.Elements[5])),
            });
        }
        public void AcceptSegmentSegmentHighlighting(SegmentHighlighting segmentHighlighting, MetafileContext parameter)
        {
            parameter.AddNode("SEGMENT HIGHLIGHTING: {0} -> {1}", segmentHighlighting.SegmentIdentifier, segmentHighlighting.Highlighting);
        }
        public void AcceptSegmentSegmentDisplayPriority(SegmentDisplayPriority segmentDisplayPriority, MetafileContext parameter)
        {
            parameter.AddNode("SEGMENT DISPLAY PRIORITY: {0} -> {1}", segmentDisplayPriority.SegmentIdentifier, segmentDisplayPriority.Priority);
        }
        public void AcceptSegmentSegmentPickPriority(SegmentPickPriority segmentPickPriority, MetafileContext parameter)
        {
            parameter.AddNode("SEGMENT PICK PRIORITY: {0} -> {1}", segmentPickPriority.SegmentIdentifier, segmentPickPriority.Priority);
        }

        public void AcceptApplicationStructureDescriptorAttribute(ApplicationStructureAttribute applicationStructureAttribute, MetafileContext parameter)
        {
            var aps = new APSAttributeNode(applicationStructureAttribute);
            parameter.AddNode(aps);
        }

        public void AcceptUnsupportedCommand(UnsupportedCommand unsupportedCommand, MetafileContext parameter)
        {
            parameter.AddUnsupportedNode(unsupportedCommand);
        }
    }
}
