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

        public static BeginSegment BeginSegment(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (name) segment identifier [ISO/IEC 8632-3 8.2]
            return new BeginSegment(reader.ReadString());
        }

        public static EndSegment EndSegment(MetafileReader reader, CommandHeader commandHeader)
        {
            // END SEGMENT: has no parameters [ISO/IEC 8632-3 8.2]
            return new EndSegment();
        }

        public static BeginFigure BeginFigure(MetafileReader reader, CommandHeader commandHeader)
        {
            // BEGIN FIGURE: has no parameters [ISO/IEC 8632-3 8.2]
            return new BeginFigure();
        }

        public static EndFigure EndFigure(MetafileReader reader, CommandHeader commandHeader)
        {
            // END FIGURE: has no parameters [ISO/IEC 8632-3 8.2]
            return new EndFigure();
        }

        public static BeginProtectionRegion BeginProtectionRegion(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (index) region index. [ISO/IEC 8632-3 8.2]
            return new BeginProtectionRegion(reader.ReadIndex());
        }

        public static EndProtectionRegion EndProtectionRegion(MetafileReader reader, CommandHeader commandHeader)
        {
            // END PROTECTION REGION: has no parameters. [ISO/IEC 8632-3 8.2]
            return new EndProtectionRegion();
        }
    }
}
