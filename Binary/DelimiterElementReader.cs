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

        public static BeginPicture BeginPicture(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (string fixed) picture name [ISO/IEC 8632-3 8.2]
            return new BeginPicture(reader.ReadString());
        }

        public static BeginPictureBody BeginPictureBody(MetafileReader reader, CommandHeader commandHeader)
        {
            // BEGIN PICTURE BODY: has no parameters. [ISO/IEC 8632-3 8.2]
            return new BeginPictureBody();
        }

        public static EndPicture EndPicture(MetafileReader reader, CommandHeader commandHeader)
        {
            // END PICTURE: has no parameters. [ISO/IEC 8632-3 8.2]
            return new EndPicture();
        }
    }
}
