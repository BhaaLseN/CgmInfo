using CgmInfo.Commands;
using CgmInfo.Commands.Delimiter;
using CgmInfo.Commands.Enums;
using CgmInfo.Commands.MetafileDescriptor;
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

        public void AcceptUnsupportedCommand(UnsupportedCommand unsupportedCommand, PrintContext parameter)
        {
            // do nothing; otherwise we'd probably spam the command line
        }
    }
}
