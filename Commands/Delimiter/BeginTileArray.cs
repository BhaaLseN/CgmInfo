using System.Drawing;
using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    public class BeginTileArray : Command
    {
        public BeginTileArray(double positionX, double positionY,
            CellPathDirection cellPathDirection, LineProgressionDirection lineProgressionDirection,
            int pathDirectionTileCount, int lineDirectionTileCount,
            int pathDirectionCellCount, int lineDirectionCellCount,
            double pathDirectionCellSize, double lineDirectionCellSize,
            int pathDirectionImageOffset, int lineDirectionImageOffset,
            int pathDirectionImageCellCount, int lineDirectionImageCellCount)
            : base(0, 19)
        {
            Position = new PointF((float)positionX, (float)positionY);
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

        public PointF Position { get; private set; }
        public CellPathDirection CellPathDirection { get; private set; }
        public LineProgressionDirection LineProgressionDirection { get; private set; }
        public int PathDirectionTileCount { get; private set; }
        public int LineDirectionTileCount { get; private set; }
        public int LineDirectionImageCellCount { get; private set; }
        public int PathDirectionImageCellCount { get; private set; }
        public int LineDirectionImageOffset { get; private set; }
        public int PathDirectionImageOffset { get; private set; }
        public double LineDirectionCellSize { get; private set; }
        public double PathDirectionCellSize { get; private set; }
        public int LineDirectionCellCount { get; private set; }
        public int PathDirectionCellCount { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterBeginTileArray(this, parameter);
        }
    }
}
