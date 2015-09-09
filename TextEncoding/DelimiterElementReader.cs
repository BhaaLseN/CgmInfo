using CgmInfo.Commands.Delimiter;
using CgmInfo.Commands.Enums;

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
                reader.ReadPoint(),
                ParseCellPathDirection(reader.ReadEnum()),
                ParseLineProgressionDirection(reader.ReadEnum()),
                reader.ReadInteger(), reader.ReadInteger(),
                reader.ReadInteger(), reader.ReadInteger(),
                reader.ReadReal(), reader.ReadReal(),
                reader.ReadInteger(), reader.ReadInteger(),
                reader.ReadInteger(), reader.ReadInteger());
        }
        private static CellPathDirection ParseCellPathDirection(string token)
        {
            // assume 0 degrees direction unless it matches any of the other possibilities
            if (token == "90")
                return CellPathDirection.Down_90deg;
            else if (token == "180")
                return CellPathDirection.Left_180deg;
            else if (token == "270")
                return CellPathDirection.Up_270deg;
            return CellPathDirection.Right_0deg;
        }
        private static LineProgressionDirection ParseLineProgressionDirection(string token)
        {
            // assume clockwise 90 degrees progression unless it is counter-clockwise 270 degrees
            if (token == "270")
                return LineProgressionDirection.CounterClockwise_270deg;
            return LineProgressionDirection.Clockwise_90deg;
        }

        public static EndTileArray EndTileArray(MetafileReader reader)
        {
            return new EndTileArray();
        }

        public static BeginApplicationStructure BeginApplicationStructure(MetafileReader reader)
        {
            return new BeginApplicationStructure(reader.ReadString(), reader.ReadString(), ParseInheritanceFlag(reader.ReadEnum()));
        }
        private static InheritanceFlag ParseInheritanceFlag(string token)
        {
            // assume StateList unless the value is ApplicationStructure
            if (token.ToUpperInvariant() == "APS")
                return InheritanceFlag.ApplicationStructure;
            return InheritanceFlag.StateList;
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
