using CgmInfo.Commands;
using CgmInfo.Commands.Delimiter;
using CgmInfo.Commands.MetafileDescriptor;

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

        // metafile descriptor
        void AcceptMetafileDescriptorMetafileVersion(MetafileVersion metafileVersion, T parameter);
        void AcceptMetafileDescriptorMetafileDescription(MetafileDescription metafileDescription, T parameter);
        void AcceptMetafileDescriptorVdcType(VdcType vdcType, T parameter);
        void AcceptMetafileDescriptorIntegerPrecision(IntegerPrecision integerPrecision, T parameter);
        void AcceptMetafileDescriptorRealPrecision(RealPrecision realPrecision, T parameter);
        void AcceptMetafileDescriptorIndexPrecision(IndexPrecision indexPrecision, T parameter);
        void AcceptMetafileDescriptorColorPrecision(ColorPrecision colorPrecision, T parameter);
        void AcceptMetafileDescriptorColorIndexPrecision(ColorIndexPrecision colorIndexPrecision,T parameter);
        void AcceptMetafileDescriptorMaximumColorIndex(MaximumColorIndex maximumColorIndex, T parameter);
        void AcceptMetafileDescriptorColorValueExtent(ColorValueExtent colorValueExtent, T parameter);
        void AcceptMetafileDescriptorFontList(FontList fontList, T parameter);
        void AcceptMetafileDescriptorNamePrecision(NamePrecision namePrecision, T parameter);
        void AcceptMetafileDescriptorMaximumVdcExtent(MaximumVdcExtent maximumVdcExtent, T parameter);
        void AcceptMetafileDescriptorColorModel(ColorModelCommand colorModel, T parameter);
    }
}
