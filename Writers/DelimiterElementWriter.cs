using CgmInfo.Commands;
using CgmInfo.Commands.Delimiter;

namespace CgmInfo.Writers
{
    // [ISO/IEC 8632-3 8.2]
    internal static class DelimiterElementWriter
    {
        public static void Noop(MetafileWriter writer, Command command)
        {
            // NO OP: has no parameters
        }

        public static void BeginMetafile(MetafileWriter writer, Command command)
        {
            var beginMetafile = (BeginMetafile)command;
            // P1: (string fixed) metafile name
            writer.WriteString(beginMetafile.Name);
        }

        public static void EndMetafile(MetafileWriter writer, Command command)
        {
            // END METAFILE: has no parameters
        }

        public static void BeginPicture(MetafileWriter writer, Command command)
        {
            var beginPicture = (BeginPicture)command;
            // P1: (string fixed) picture name
            writer.WriteString(beginPicture.Name);
        }

        public static void BeginPictureBody(MetafileWriter writer, Command command)
        {
            // BEGIN PICTURE BODY: has no parameters
        }

        public static void EndPicture(MetafileWriter writer, Command command)
        {
            // END PICTURE: has no parameters
        }

        public static void BeginSegment(MetafileWriter writer, Command command)
        {
            var beginSegment = (BeginSegment)command;
            // P1: (name) segment identifier
            writer.WriteName(beginSegment.Identifier);
        }

        public static void EndSegment(MetafileWriter writer, Command command)
        {
            // END SEGMENT: has no parameters
        }

        public static void BeginFigure(MetafileWriter writer, Command command)
        {
            // BEGIN FIGURE: has no parameters
        }

        public static void EndFigure(MetafileWriter writer, Command command)
        {
            // END FIGURE: has no parameters
        }

        public static void BeginProtectionRegion(MetafileWriter writer, Command command)
        {
            var beginProtectionRegion = (BeginProtectionRegion)command;
            // P1: (index) region index
            writer.WriteIndex(beginProtectionRegion.RegionIndex);
        }

        public static void EndProtectionRegion(MetafileWriter writer, Command command)
        {
            // END PROTECTION REGION: has no parameters
        }

        public static void BeginCompoundLine(MetafileWriter writer, Command command)
        {
            // BEGIN COMPOUND LINE: has no parameters
        }

        public static void EndCompoundLine(MetafileWriter writer, Command command)
        {
            // END COMPOUND LINE: has no parameters
        }

        public static void BeginCompoundTextPath(MetafileWriter writer, Command command)
        {
            // BEGIN COMPOUND TEXT PATH: has no parameters
        }

        public static void EndCompoundTextPath(MetafileWriter writer, Command command)
        {
            // END COMPOUND TEXT PATH: has no parameters
        }

        public static void BeginTileArray(MetafileWriter writer, Command command)
        {
            var beginTileArray = (BeginTileArray)command;
            // P1: (point) position
            writer.WritePoint(beginTileArray.Position);
            // P2: (enumerated) cell path direction: valid values are
            //      0 0°
            //      1 90°
            //      2 180°
            //      3 270°
            writer.WriteEnum(beginTileArray.CellPathDirection);
            // P3: (enumerated) line progression direction: valid values are
            //      0 90°
            //      1 270°
            writer.WriteEnum(beginTileArray.LineProgressionDirection);
            // P4: (integer) number of tiles in pth direction
            writer.WriteInteger(beginTileArray.PathDirectionTileCount);
            // P5: (integer) number of tiles in line direction
            writer.WriteInteger(beginTileArray.LineDirectionTileCount);
            // P6: (integer) number of cells/ tile in path direction
            writer.WriteInteger(beginTileArray.PathDirectionCellCount);
            // P7: (integer) number of cells/ tile in line direction
            writer.WriteInteger(beginTileArray.LineDirectionCellCount);
            // P8: (real) cell size in path direction
            writer.WriteReal(beginTileArray.PathDirectionCellSize);
            // P9: (real) cell size in line direction
            writer.WriteReal(beginTileArray.LineDirectionCellSize);
            // P10: (integer) image offset in path direction
            writer.WriteInteger(beginTileArray.PathDirectionImageOffset);
            // P11: (integer) image offset in line direction
            writer.WriteInteger(beginTileArray.LineDirectionImageOffset);
            // P12: (integer) image number of cells in path direction
            writer.WriteInteger(beginTileArray.PathDirectionImageCellCount);
            // P13: (integer) image number of cells in line direction
            writer.WriteInteger(beginTileArray.LineDirectionImageCellCount);
        }

        public static void EndTileArray(MetafileWriter writer, Command command)
        {
            // END TILE ARRAY: has no parameters
        }

        public static void BeginApplicationStructure(MetafileWriter writer, Command command)
        {
            var beginApplicationStructure = (BeginApplicationStructure)command;
            // P1: (string fixed) application structure identifier
            writer.WriteString(beginApplicationStructure.Identifier);
            // P2: (string fixed) application structure type
            writer.WriteString(beginApplicationStructure.Type);
            // P3: (enumerated) inheritance flag: valid values are
            //      0 STATELIST
            //      1 APPLICATION STRUCTURE
            writer.WriteEnum(beginApplicationStructure.Inheritance);
        }

        public static void BeginApplicationStructureBody(MetafileWriter writer, Command command)
        {
            // BEGIN APPLICATION STRUCTURE BODY: has no parameters
        }

        public static void EndApplicationStructure(MetafileWriter writer, Command command)
        {
            // END APPLICATION STRUCTURE: has no parameters
        }
    }
}
