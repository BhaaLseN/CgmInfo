using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.Delimiter
{
    [TextToken("BEGTILEARRAY")]
    public class BeginTileArray : Command
    {
        public BeginTileArray(MetafilePoint position,
            CellPathDirection cellPathDirection, LineProgressionDirection lineProgressionDirection,
            int pathDirectionTileCount, int lineDirectionTileCount,
            int pathDirectionCellCount, int lineDirectionCellCount,
            double pathDirectionCellSize, double lineDirectionCellSize,
            int pathDirectionImageOffset, int lineDirectionImageOffset,
            int pathDirectionImageCellCount, int lineDirectionImageCellCount)
            : base(0, 19)
        {
            Position = position;
            CellPathDirection = cellPathDirection;
            LineProgressionDirection = lineProgressionDirection;

            PathDirectionTileCount = pathDirectionTileCount;
            LineDirectionTileCount = lineDirectionTileCount;

            PathDirectionCellCount = pathDirectionCellCount;
            LineDirectionCellCount = lineDirectionCellCount;

            PathDirectionCellSize = pathDirectionCellSize;
            LineDirectionCellSize = lineDirectionCellSize;

            PathDirectionImageOffset = pathDirectionImageOffset;
            LineDirectionImageOffset = lineDirectionImageOffset;

            PathDirectionImageCellCount = pathDirectionImageCellCount;
            LineDirectionImageCellCount = lineDirectionImageCellCount;
        }

        public MetafilePoint Position { get; }
        public CellPathDirection CellPathDirection { get; }
        public LineProgressionDirection LineProgressionDirection { get; }
        public int PathDirectionTileCount { get; }
        public int LineDirectionTileCount { get; }
        public int LineDirectionImageCellCount { get; }
        public int PathDirectionImageCellCount { get; }
        public int LineDirectionImageOffset { get; }
        public int PathDirectionImageOffset { get; }
        public double LineDirectionCellSize { get; }
        public double PathDirectionCellSize { get; }
        public int LineDirectionCellCount { get; }
        public int PathDirectionCellCount { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterBeginTileArray(this, parameter);
        }
    }
}
