using CgmInfo.Commands.ApplicationStructureDescriptor;

namespace CgmInfo.TextEncoding
{
    internal static class ApplicationStructureDescriptorReader
    {
        public static ApplicationStructureAttribute ApplicationStructureAttribute(MetafileReader reader)
        {
            string attributeType = reader.ReadString();
            return new ApplicationStructureAttribute(attributeType, reader.ReadStructuredDataRecord());
        }
    }
}
