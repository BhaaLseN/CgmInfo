using CgmInfo.Commands.Enums;
using CgmInfo.Commands.External;

namespace CgmInfo.BinaryEncoding
{
    // [ISO/IEC 8632-3 8.8]
    internal static class ExternalReader
    {
        public static Message Message(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) action-required flag: valid values are
            //      0 noaction
            //      1 action
            // P2: (string fixed) message string 
            return new Message(reader.ReadEnum<ActionRequired>(), reader.ReadString());
        }

        public static ApplicationData ApplicationData(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) identifier
            // P2: (data record) application data record; data records are bound as strings in this encoding.
            return new ApplicationData(reader.ReadInteger(), reader.ReadString());
        }
    }
}
