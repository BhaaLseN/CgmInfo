using System.Linq;
using CgmInfo.Commands.Delimiter;
using CgmInfo.Commands.MetafileDescriptor;
using CgmInfo.Utilities;

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
        public override void AcceptMetafileDescriptorMetafileDescription(MetafileDescription metafileDescription, MetafileProperties parameter)
        {
            var keys = MetafileDescriptionParser.ParseDescription(metafileDescription.Description);
            if (keys.Any())
            {
                if (!keys.TryGetValue("ProfileId", out string profileId))
                    profileId = "ISO/IEC 8632";
                if (!keys.TryGetValue("ProfileEd", out string profileEd))
                    profileEd = "(guess)";

                parameter.Profile = string.Format("{0} {1}", profileId, profileEd);
            }
            else
            {
                parameter.Profile = "ISO/IEC 8632 (guess)";
            }
        }
    }
}
