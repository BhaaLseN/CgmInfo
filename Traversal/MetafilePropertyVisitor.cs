using CgmInfo.Commands.Delimiter;

namespace CgmInfo.Traversal
{
    internal class MetafilePropertyVisitor : CommandVisitor<MetafileProperties>
    {
        public override void AcceptDelimiterBeginMetafile(BeginMetafile beginMetafile, MetafileProperties parameter)
        {
            parameter.Name = beginMetafile.Name;
        }
    }
}
