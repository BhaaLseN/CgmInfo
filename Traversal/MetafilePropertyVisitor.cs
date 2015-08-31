using CgmInfo.Commands.Delimiter;
using CgmInfo.Commands.MetafileDescriptor;

namespace CgmInfo.Traversal
{
    internal class MetafilePropertyVisitor : CommandVisitor<MetafileProperties>
    {
        public override void AcceptDelimiterBeginMetafile(BeginMetafile beginMetafile, MetafileProperties parameter)
        {
            parameter.Name = beginMetafile.Name;
        }
        public override void AcceptMetafileDescriptorMetafileVersion(MetafileVersion metafileVersion, MetafileProperties parameter)
        {
            parameter.Version = metafileVersion.Version;
        }
    }
}
