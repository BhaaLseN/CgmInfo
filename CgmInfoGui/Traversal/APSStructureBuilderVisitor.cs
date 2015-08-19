using System;
using System.Linq;
using CgmInfo.Commands;
using CgmInfo.Commands.ApplicationStructureDescriptor;
using CgmInfo.Commands.Delimiter;
using CgmInfo.Commands.GraphicalPrimitives;
using CgmInfo.Commands.MetafileDescriptor;
using CgmInfo.Traversal;
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Traversal
{
    public class APSStructureBuilderVisitor : ICommandVisitor<APSStructureContext>
    {
        public void AcceptApplicationStructureDescriptorAttribute(ApplicationStructureAttribute applicationStructureAttribute, APSStructureContext parameter)
        {
            parameter.AddAttributeNode(applicationStructureAttribute);
        }

        public void AcceptDelimiterBeginApplicationStructure(BeginApplicationStructure beginApplicationStructure, APSStructureContext parameter)
        {
            parameter.BeginLevel("{0} '{1}'", beginApplicationStructure.Type, beginApplicationStructure.Identifier);
        }

        public void AcceptDelimiterBeginApplicationStructureBody(BeginApplicationStructureBody beginApplicationStructureBody, APSStructureContext parameter)
        {
            // left blank for now, but we never know if/when we'd need it.
        }

        public void AcceptDelimiterEndApplicationStructure(EndApplicationStructure endApplicationStructure, APSStructureContext parameter)
        {
            parameter.EndLevel();
        }

        #region Unused Methods
        public void AcceptDelimiterBeginCompoundLine(BeginCompoundLine beginCompoundLine, APSStructureContext parameter)
        {
        }

        public void AcceptDelimiterBeginCompoundTextPath(BeginCompoundTextPath beginCompoundTextPath, APSStructureContext parameter)
        {
        }

        public void AcceptDelimiterBeginFigure(BeginFigure beginFigure, APSStructureContext parameter)
        {
        }

        public void AcceptDelimiterBeginMetafile(BeginMetafile beginMetafile, APSStructureContext parameter)
        {
        }

        public void AcceptDelimiterBeginPicture(BeginPicture beginPicture, APSStructureContext parameter)
        {
        }

        public void AcceptDelimiterBeginPictureBody(BeginPictureBody beginPictureBody, APSStructureContext parameter)
        {
        }

        public void AcceptDelimiterBeginProtectionRegion(BeginProtectionRegion beginProtectionRegion, APSStructureContext parameter)
        {
        }

        public void AcceptDelimiterBeginSegment(BeginSegment beginSegment, APSStructureContext parameter)
        {
        }

        public void AcceptDelimiterBeginTileArray(BeginTileArray beginTileArray, APSStructureContext parameter)
        {
        }

        public void AcceptDelimiterEndCompoundLine(EndCompoundLine endCompoundLine, APSStructureContext parameter)
        {
        }

        public void AcceptDelimiterEndCompoundTextPath(EndCompoundTextPath endCompoundTextPath, APSStructureContext parameter)
        {
        }

        public void AcceptDelimiterEndFigure(EndFigure endFigure, APSStructureContext parameter)
        {
        }

        public void AcceptDelimiterEndMetafile(EndMetafile endMetafile, APSStructureContext parameter)
        {
        }

        public void AcceptDelimiterEndPicture(EndPicture endPicture, APSStructureContext parameter)
        {
        }

        public void AcceptDelimiterEndProtectionRegion(EndProtectionRegion endProtectionRegion, APSStructureContext parameter)
        {
        }

        public void AcceptDelimiterEndSegment(EndSegment endSegment, APSStructureContext parameter)
        {
        }

        public void AcceptDelimiterEndTileArray(EndTileArray endTileArray, APSStructureContext parameter)
        {
        }

        public void AcceptGraphicalPrimitiveAppendText(AppendText appendText, APSStructureContext parameter)
        {
        }

        public void AcceptGraphicalPrimitiveRestrictedText(RestrictedText restrictedText, APSStructureContext parameter)
        {
        }

        public void AcceptGraphicalPrimitiveText(TextCommand text, APSStructureContext parameter)
        {
        }

        public void AcceptMetafileDescriptorCharacterCodingAnnouncer(CharacterCodingAnnouncer characterCodingAnnouncer, APSStructureContext parameter)
        {
        }

        public void AcceptMetafileDescriptorCharacterSetList(CharacterSetList characterSetList, APSStructureContext parameter)
        {
        }

        public void AcceptMetafileDescriptorColorIndexPrecision(ColorIndexPrecision colorIndexPrecision, APSStructureContext parameter)
        {
        }

        public void AcceptMetafileDescriptorColorModel(ColorModelCommand colorModel, APSStructureContext parameter)
        {
        }

        public void AcceptMetafileDescriptorColorPrecision(ColorPrecision colorPrecision, APSStructureContext parameter)
        {
        }

        public void AcceptMetafileDescriptorColorValueExtent(ColorValueExtent colorValueExtent, APSStructureContext parameter)
        {
        }

        public void AcceptMetafileDescriptorFontList(FontList fontList, APSStructureContext parameter)
        {
        }

        public void AcceptMetafileDescriptorIndexPrecision(IndexPrecision indexPrecision, APSStructureContext parameter)
        {
        }

        public void AcceptMetafileDescriptorIntegerPrecision(IntegerPrecision integerPrecision, APSStructureContext parameter)
        {
        }

        public void AcceptMetafileDescriptorMaximumColorIndex(MaximumColorIndex maximumColorIndex, APSStructureContext parameter)
        {
        }

        public void AcceptMetafileDescriptorMaximumVdcExtent(MaximumVdcExtent maximumVdcExtent, APSStructureContext parameter)
        {
        }

        public void AcceptMetafileDescriptorMetafileDescription(MetafileDescription metafileDescription, APSStructureContext parameter)
        {
        }

        public void AcceptMetafileDescriptorMetafileVersion(MetafileVersion metafileVersion, APSStructureContext parameter)
        {
        }

        public void AcceptMetafileDescriptorNamePrecision(NamePrecision namePrecision, APSStructureContext parameter)
        {
        }

        public void AcceptMetafileDescriptorRealPrecision(RealPrecision realPrecision, APSStructureContext parameter)
        {
        }

        public void AcceptMetafileDescriptorVdcType(VdcType vdcType, APSStructureContext parameter)
        {
        }

        public void AcceptUnsupportedCommand(UnsupportedCommand unsupportedCommand, APSStructureContext parameter)
        {
        }
        #endregion
    }
}
