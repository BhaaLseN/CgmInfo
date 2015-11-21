using CgmInfo.Commands.Escape;

namespace CgmInfo.BinaryEncoding
{
    // [ISO/IEC 8632-3 8.8]
    internal static class EscapeReader
    {
        public static EscapeCommand Escape(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) escape identifier
            // P2: (data record) escape data record; data records are bound as strings in this encoding.
            return new EscapeCommand(reader.ReadInteger(), ApplicationStructureDescriptorReader.ReadStructuredDataRecord(reader));
        }
    }
}
