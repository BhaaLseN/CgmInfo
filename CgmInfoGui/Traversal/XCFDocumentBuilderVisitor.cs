using CgmInfo.Commands;
using CgmInfo.Commands.ApplicationStructureDescriptor;
using CgmInfo.Commands.Delimiter;
using CgmInfo.Commands.GraphicalPrimitives;
using CgmInfo.Commands.MetafileDescriptor;
using CgmInfo.Traversal;

namespace CgmInfoGui.Traversal
{
    public class XCFDocumentBuilderVisitor : ICommandVisitor<XCFDocumentContext>
    {
        public void AcceptApplicationStructureDescriptorAttribute(ApplicationStructureAttribute applicationStructureAttribute, XCFDocumentContext parameter)
        {
            parameter.AddAPSAttribute(applicationStructureAttribute);
        }

        public void AcceptDelimiterBeginApplicationStructure(BeginApplicationStructure beginApplicationStructure, XCFDocumentContext parameter)
        {
            parameter.AddAPSElement(beginApplicationStructure);
        }

        public void AcceptDelimiterBeginApplicationStructureBody(BeginApplicationStructureBody beginApplicationStructureBody, XCFDocumentContext parameter)
        {
            // left blank for now, but we never know if/when we'd need it.
        }

        public void AcceptDelimiterEndApplicationStructure(EndApplicationStructure endApplicationStructure, XCFDocumentContext parameter)
        {
        }

        #region Unused Methods
        public void AcceptDelimiterBeginCompoundLine(BeginCompoundLine beginCompoundLine, XCFDocumentContext parameter)
        {
        }

        public void AcceptDelimiterBeginCompoundTextPath(BeginCompoundTextPath beginCompoundTextPath, XCFDocumentContext parameter)
        {
        }

        public void AcceptDelimiterBeginFigure(BeginFigure beginFigure, XCFDocumentContext parameter)
        {
        }

        public void AcceptDelimiterBeginMetafile(BeginMetafile beginMetafile, XCFDocumentContext parameter)
        {
        }

        public void AcceptDelimiterBeginPicture(BeginPicture beginPicture, XCFDocumentContext parameter)
        {
        }

        public void AcceptDelimiterBeginPictureBody(BeginPictureBody beginPictureBody, XCFDocumentContext parameter)
        {
        }

        public void AcceptDelimiterBeginProtectionRegion(BeginProtectionRegion beginProtectionRegion, XCFDocumentContext parameter)
        {
        }

        public void AcceptDelimiterBeginSegment(BeginSegment beginSegment, XCFDocumentContext parameter)
        {
        }

        public void AcceptDelimiterBeginTileArray(BeginTileArray beginTileArray, XCFDocumentContext parameter)
        {
        }

        public void AcceptDelimiterEndCompoundLine(EndCompoundLine endCompoundLine, XCFDocumentContext parameter)
        {
        }

        public void AcceptDelimiterEndCompoundTextPath(EndCompoundTextPath endCompoundTextPath, XCFDocumentContext parameter)
        {
        }

        public void AcceptDelimiterEndFigure(EndFigure endFigure, XCFDocumentContext parameter)
        {
        }

        public void AcceptDelimiterEndMetafile(EndMetafile endMetafile, XCFDocumentContext parameter)
        {
        }

        public void AcceptDelimiterEndPicture(EndPicture endPicture, XCFDocumentContext parameter)
        {
        }

        public void AcceptDelimiterEndProtectionRegion(EndProtectionRegion endProtectionRegion, XCFDocumentContext parameter)
        {
        }

        public void AcceptDelimiterEndSegment(EndSegment endSegment, XCFDocumentContext parameter)
        {
        }

        public void AcceptDelimiterEndTileArray(EndTileArray endTileArray, XCFDocumentContext parameter)
        {
        }

        public void AcceptGraphicalPrimitiveAppendText(AppendText appendText, XCFDocumentContext parameter)
        {
        }

        public void AcceptGraphicalPrimitiveRestrictedText(RestrictedText restrictedText, XCFDocumentContext parameter)
        {
        }

        public void AcceptGraphicalPrimitiveText(TextCommand text, XCFDocumentContext parameter)
        {
        }

        public void AcceptMetafileDescriptorCharacterCodingAnnouncer(CharacterCodingAnnouncer characterCodingAnnouncer, XCFDocumentContext parameter)
        {
        }

        public void AcceptMetafileDescriptorCharacterSetList(CharacterSetList characterSetList, XCFDocumentContext parameter)
        {
        }

        public void AcceptMetafileDescriptorColorIndexPrecision(ColorIndexPrecision colorIndexPrecision, XCFDocumentContext parameter)
        {
        }

        public void AcceptMetafileDescriptorColorModel(ColorModelCommand colorModel, XCFDocumentContext parameter)
        {
        }

        public void AcceptMetafileDescriptorColorPrecision(ColorPrecision colorPrecision, XCFDocumentContext parameter)
        {
        }

        public void AcceptMetafileDescriptorColorValueExtent(ColorValueExtent colorValueExtent, XCFDocumentContext parameter)
        {
        }

        public void AcceptMetafileDescriptorFontList(FontList fontList, XCFDocumentContext parameter)
        {
        }

        public void AcceptMetafileDescriptorIndexPrecision(IndexPrecision indexPrecision, XCFDocumentContext parameter)
        {
        }

        public void AcceptMetafileDescriptorIntegerPrecision(IntegerPrecision integerPrecision, XCFDocumentContext parameter)
        {
        }

        public void AcceptMetafileDescriptorMaximumColorIndex(MaximumColorIndex maximumColorIndex, XCFDocumentContext parameter)
        {
        }

        public void AcceptMetafileDescriptorMaximumVdcExtent(MaximumVdcExtent maximumVdcExtent, XCFDocumentContext parameter)
        {
        }

        public void AcceptMetafileDescriptorMetafileDescription(MetafileDescription metafileDescription, XCFDocumentContext parameter)
        {
        }

        public void AcceptMetafileDescriptorMetafileVersion(MetafileVersion metafileVersion, XCFDocumentContext parameter)
        {
        }

        public void AcceptMetafileDescriptorNamePrecision(NamePrecision namePrecision, XCFDocumentContext parameter)
        {
        }

        public void AcceptMetafileDescriptorRealPrecision(RealPrecision realPrecision, XCFDocumentContext parameter)
        {
        }

        public void AcceptMetafileDescriptorVdcType(VdcType vdcType, XCFDocumentContext parameter)
        {
        }

        public void AcceptUnsupportedCommand(UnsupportedCommand unsupportedCommand, XCFDocumentContext parameter)
        {
        }
        #endregion
    }
}
