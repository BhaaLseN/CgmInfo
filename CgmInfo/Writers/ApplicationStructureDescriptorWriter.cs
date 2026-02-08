using CgmInfo.Commands;
using CgmInfo.Commands.ApplicationStructureDescriptor;

namespace CgmInfo.Writers
{
    // [ISO/IEC 8632-3 8.11]
    internal static class ApplicationStructureDescriptorWriter
    {
        public static void ApplicationStructureAttribute(MetafileWriter writer, Command command)
        {
            var applicationStructureAttribute = (ApplicationStructureAttribute)command;
            // P1: (string fixed) application structure attribute type
            writer.WriteString(applicationStructureAttribute.AttributeType);
            // P2: (structured data record) data record
            writer.WriteStructuredDataRecord(applicationStructureAttribute.DataRecord);
        }
    }
}
