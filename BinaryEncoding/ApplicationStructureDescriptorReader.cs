using CgmInfo.Commands.ApplicationStructureDescriptor;

namespace CgmInfo.BinaryEncoding
{
    // [ISO/IEC 8632-3 8.11]
    internal static class ApplicationStructureDescriptorReader
    {
        public static ApplicationStructureAttribute ApplicationStructureAttribute(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (string fixed) application structure attribute type
            // P2: (structured data record) data record
            return new ApplicationStructureAttribute(reader.ReadString(), reader.ReadStructuredDataRecord());
        }
    }
}
