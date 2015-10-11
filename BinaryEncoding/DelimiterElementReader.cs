using CgmInfo.Commands.Delimiter;
using CgmInfo.Commands.Enums;

namespace CgmInfo.BinaryEncoding
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
            // END METAFILE: has no parameters [ISO/IEC 8632-3 8.2]
            return new EndMetafile();
        }

        public static BeginPicture BeginPicture(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (string fixed) picture name [ISO/IEC 8632-3 8.2]
            return new BeginPicture(reader.ReadString());
        }

        public static BeginPictureBody BeginPictureBody(MetafileReader reader, CommandHeader commandHeader)
        {
            // BEGIN PICTURE BODY: has no parameters [ISO/IEC 8632-3 8.2]
            return new BeginPictureBody();
        }

        public static EndPicture EndPicture(MetafileReader reader, CommandHeader commandHeader)
        {
            // END PICTURE: has no parameters [ISO/IEC 8632-3 8.2]
            return new EndPicture();
        }

        public static BeginSegment BeginSegment(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (name) segment identifier [ISO/IEC 8632-3 8.2]
            return new BeginSegment(reader.ReadName());
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
            // P1: (index) region index [ISO/IEC 8632-3 8.2]
            return new BeginProtectionRegion(reader.ReadIndex());
        }

        public static EndProtectionRegion EndProtectionRegion(MetafileReader reader, CommandHeader commandHeader)
        {
            // END PROTECTION REGION: has no parameters [ISO/IEC 8632-3 8.2]
            return new EndProtectionRegion();
        }

        public static BeginCompoundLine BeginCompoundLine(MetafileReader reader, CommandHeader commandHeader)
        {
            // BEGIN COMPOUND LINE: has no parameters [ISO/IEC 8632-3 8.2]
            return new BeginCompoundLine();
        }

        public static EndCompoundLine EndCompoundLine(MetafileReader reader, CommandHeader commandHeader)
        {
            // END COMPOUND LINE: has no parameters [ISO/IEC 8632-3 8.2]
            return new EndCompoundLine();
        }

        public static BeginCompoundTextPath BeginCompoundTextPath(MetafileReader reader, CommandHeader commandHeader)
        {
            // BEGIN COMPOUND TEXT PATH: has no parameters [ISO/IEC 8632-3 8.2]
            return new BeginCompoundTextPath();
        }

        public static EndCompoundTextPath EndCompoundTextPath(MetafileReader reader, CommandHeader commandHeader)
        {
            // END COMPOUND TEXT PATH: has no parameters [ISO/IEC 8632-3 8.2]
            return new EndCompoundTextPath();
        }

        public static BeginTileArray BeginTileArray(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (point) position [ISO/IEC 8632-3 8.2]
            // P2: (enumerated) cell path direction: valid values are
            //      0 0°
            //      1 90°
            //      2 180°
            //      3 270°
            // P3: (enumerated) line progression direction: valid values are
            //      0 90°
            //      1 270°
            // P4: (integer) number of tiles in pth direction
            // P5: (integer) number of tiles in line direction
            // P6: (integer) number of cells/ tile in path direction
            // P7: (integer) number of cells/ tile in line direction
            // P8: (real) cell size in path direction
            // P9: (real) cell size in line direction
            // P10: (integer) image offset in path direction
            // P11: (integer) image offset in line direction
            // P12: (integer) image number of cells in path direction
            // P13: (integer) image number of cells in line direction
            return new BeginTileArray(
                reader.ReadPoint(),
                reader.ReadEnum<CellPathDirection>(),
                reader.ReadEnum<LineProgressionDirection>(),
                reader.ReadInteger(), reader.ReadInteger(),
                reader.ReadInteger(), reader.ReadInteger(),
                reader.ReadReal(), reader.ReadReal(),
                reader.ReadInteger(), reader.ReadInteger(),
                reader.ReadInteger(), reader.ReadInteger());
        }

        public static EndTileArray EndTileArray(MetafileReader reader, CommandHeader commandHeader)
        {
            // END TILE ARRAY: has no parameters [ISO/IEC 8632-3 8.2]
            return new EndTileArray();
        }

        public static BeginApplicationStructure BeginApplicationStructure(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (string fixed) application structure identifier [ISO/IEC 8632-3 8.2]
            // P2: (string fixed) application structure type
            // P3: (enumerated) inheritance flag: valid values are
            //      0 STATELIST
            //      1 APPLICATION STRUCTURE
            return new BeginApplicationStructure(reader.ReadString(), reader.ReadString(), reader.ReadEnum<InheritanceFlag>());
        }

        public static BeginApplicationStructureBody BeginApplicationStructureBody(MetafileReader reader, CommandHeader commandHeader)
        {
            // BEGIN APPLICATION STRUCTURE BODY: has no parameters [ISO/IEC 8632-3 8.2]
            return new BeginApplicationStructureBody();
        }

        public static EndApplicationStructure EndApplicationStructure(MetafileReader reader, CommandHeader commandHeader)
        {
            // END APPLICATION STRUCTURE: has no parameters [ISO/IEC 8632-3 8.2]
            return new EndApplicationStructure();
        }
    }
}
