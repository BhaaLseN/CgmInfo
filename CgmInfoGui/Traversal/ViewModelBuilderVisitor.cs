using System;
using System.Linq;
using CgmInfo.Commands;
using CgmInfo.Commands.ApplicationStructureDescriptor;
using CgmInfo.Commands.Delimiter;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.GraphicalPrimitives;
using CgmInfo.Commands.MetafileDescriptor;
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
                    new SimpleNode(string.Format("First Component: {0}", colorValueExtent.FirstComponent)),
                    new SimpleNode(string.Format("Second Component: {0}", colorValueExtent.SecondComponent)),
                    new SimpleNode(string.Format("Third Component: {0}", colorValueExtent.ThirdComponent)),
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

        public void AcceptControlVdcIntegerPrecision(VdcIntegerPrecision vdcIntegerPrecision, MetafileContext parameter)
        {
            parameter.AddNode("VDC INTEGER PRECISION: {0} bit", vdcIntegerPrecision.Precision);
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
