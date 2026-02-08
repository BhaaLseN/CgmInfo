using CgmInfo.Commands.Enums;
using CgmInfo.Commands.External;

namespace CgmInfo.TextEncoding
{
    internal static class ExternalReader
    {
        public static Message Message(MetafileReader reader)
        {
            return new Message(ParseActionRequired(reader.ReadEnum()), reader.ReadString());
        }
        public static ApplicationData ApplicationData(MetafileReader reader)
        {
            return new ApplicationData(reader.ReadInteger(), reader.ReadString());
        }

        private static ActionRequired ParseActionRequired(string token)
        {
            // assume "no action" unless action is required
            if (token.ToUpperInvariant() == "ACTION")
                return ActionRequired.Action;
            return ActionRequired.NoAction;
        }
    }
}
