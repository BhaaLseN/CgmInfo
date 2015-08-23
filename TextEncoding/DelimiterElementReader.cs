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

        public static BeginProtectionRegion BeginProtectionRegion(MetafileReader reader)
        {
            return new BeginProtectionRegion(reader.ReadIndex());
        }

        public static EndProtectionRegion EndProtectionRegion(MetafileReader reader)
        {
            return new EndProtectionRegion();
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

        public static BeginTileArray BeginTileArray(MetafileReader reader)
        {
            return new BeginTileArray(
                reader.ReadVdc(), reader.ReadVdc(),
                ParseCellPathDirection(reader.ReadEnum()),
                ParseLineProgressionDirection(reader.ReadEnum()),
                reader.ReadInteger(), reader.ReadInteger(),
                reader.ReadInteger(), reader.ReadInteger(),
                reader.ReadReal(), reader.ReadReal(),
                reader.ReadInteger(), reader.ReadInteger(),
                reader.ReadInteger(), reader.ReadInteger());
        }
        private static int ParseCellPathDirection(string token)
        {
            // assume 0 degrees direction unless it matches any of the other possibilities
            if (token == "90")
                return 1;
            else if (token == "180")
                return 2;
            else if (token == "270")
                return 3;
            return 0;
        }
        private static int ParseLineProgressionDirection(string token)
        {
            // assume clockwise 90 degrees progression unless it is counter-clockwise 270 degrees
            if (token == "270")
                return 1;
            return 0;
        }

        public static EndTileArray EndTileArray(MetafileReader reader)
        {
            return new EndTileArray();
        }

        public static BeginApplicationStructure BeginApplicationStructure(MetafileReader reader)
        {
            return new BeginApplicationStructure(reader.ReadString(), reader.ReadString(), ParseInheritanceFlag(reader.ReadEnum()));
        }
        private static int ParseInheritanceFlag(string token)
        {
            // assume StateList unless the value is ApplicationStructure
            if (token.ToUpperInvariant() == "APS")
                return 1;
            return 0;
        }

        public static BeginApplicationStructureBody BeginApplicationStructureBody(MetafileReader reader)
        {
            return new BeginApplicationStructureBody();
        }

        public static EndApplicationStructure EndApplicationStructure(MetafileReader reader)
        {
            return new EndApplicationStructure();
        }
    }
}
