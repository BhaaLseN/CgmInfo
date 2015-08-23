using CgmInfo.Commands.ApplicationStructureDescriptor;

namespace CgmInfo.BinaryEncoding
{
    internal static class ApplicationStructureDescriptorReader
    {
        public static ApplicationStructureAttribute ReadApplicationStructureAttribute(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (string fixed) application structure attribute type [ISO/IEC 8632-3 8.11]
            // P2: (structured data record) data record
            return new ApplicationStructureAttribute(reader.ReadString(), reader.ReadStructuredDataRecord());
        }
    }
}
