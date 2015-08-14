using CgmInfo.Commands.Delimiter;

namespace CgmInfo.Binary
{
    internal static class DelimiterElementReader
    {
        public static BeginMetafile BeginMetafile(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (string fixed) metafile name [ISO/IEC 8632-3 8.2]
            return new BeginMetafile(reader.ReadString());
        }

        public static EndMetafile EndMetafile(MetafileReader reader, CommandHeader commandHeader)
        {
            // END METAFILE: has no parameters. [ISO/IEC 8632-3 8.2]
            return new EndMetafile();
        }
    }
}
