using CgmInfo.Commands.Delimiter;

namespace CgmInfo.TextEncoding
{
    internal static class DelimiterElementReader
    {
        public static BeginMetafile BeginMetafile(MetafileReader reader)
        {
            return new BeginMetafile(reader.ReadString());
        }

        public static EndMetafile EndMetafile(MetafileReader reader)
        {
            return new EndMetafile();
        }

        public static BeginPicture BeginPicture(MetafileReader reader)
        {
            return new BeginPicture(reader.ReadString());
        }

        public static BeginPictureBody BeginPictureBody(MetafileReader reader)
        {
            return new BeginPictureBody();
        }

        public static EndPicture EndPicture(MetafileReader reader)
        {
            return new EndPicture();
        }

        public static BeginSegment BeginSegment(MetafileReader reader)
        {
            return new BeginSegment(reader.ReadString());
        }

        public static EndSegment EndSegment(MetafileReader reader)
        {
            return new EndSegment();
        }

        public static BeginFigure BeginFigure(MetafileReader reader)
        {
            return new BeginFigure();
        }

        public static EndFigure EndFigure(MetafileReader reader)
        {
            return new EndFigure();
        }

        public static BeginCompoundLine BeginCompoundLine(MetafileReader reader)
        {
            return new BeginCompoundLine();
        }

        public static EndCompoundLine EndCompoundLine(MetafileReader reader)
        {
            return new EndCompoundLine();
        }

        public static BeginCompoundTextPath BeginCompoundTextPath(MetafileReader reader)
        {
            return new BeginCompoundTextPath();
        }

        public static EndCompoundTextPath EndCompoundTextPath(MetafileReader reader)
        {
            return new EndCompoundTextPath();
        }

    }
}
