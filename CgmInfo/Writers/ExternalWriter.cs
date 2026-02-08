using CgmInfo.Commands;
using CgmInfo.Commands.External;

namespace CgmInfo.Writers
{
    // [ISO/IEC 8632-3 8.9]
    internal static class ExternalWriter
    {
        public static void Message(MetafileWriter writer, Command command)
        {
            var message = (Message)command;
            // P1: (enumerated) action-required flag: valid values are
            //      0 noaction
            //      1 action
            writer.WriteEnum(message.ActionRequired);
            // P2: (string fixed) message string
            writer.WriteString(message.MessageString);
        }

        public static void ApplicationData(MetafileWriter writer, Command command)
        {
            var applicationData = (ApplicationData)command;
            // P1: (integer) identifier
            writer.WriteInteger(applicationData.Identifier);
            // P2: (data record) application data record; data records are bound as strings in this encoding.
            writer.WriteString(applicationData.DataRecord);
        }
    }
}
