using CgmInfo.Commands;
using CgmInfo.Commands.Escape;

namespace CgmInfo.Writers
{
    // [ISO/IEC 8632-3 8.8]
    internal static class EscapeWriter
    {
        public static void Escape(MetafileWriter writer, Command command)
        {
            var escape = (EscapeCommand)command;
            // P1: (integer) escape identifier
            writer.WriteInteger(escape.Identifier);
            // P2: (data record) escape data record; data records are bound as strings in this encoding.
            writer.WriteStructuredDataRecord(escape.DataRecord);
        }
    }
}
