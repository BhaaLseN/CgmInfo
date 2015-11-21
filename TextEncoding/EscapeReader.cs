using CgmInfo.Commands.Escape;

namespace CgmInfo.TextEncoding
{
    internal static class EscapeReader
    {
        public static EscapeCommand Escape(MetafileReader reader)
        {
            return new EscapeCommand(reader.ReadInteger(), ApplicationStructureDescriptorReader.ParseStructuredDataRecord(reader.ReadString()));
        }
    }
}
