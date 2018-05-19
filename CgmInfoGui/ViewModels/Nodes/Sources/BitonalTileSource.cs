using System;
using CgmInfo.Commands;
using CgmInfo.Commands.Delimiter;
using CgmInfo.Commands.GraphicalPrimitives;

namespace CgmInfoGui.ViewModels.Nodes.Sources
{
    internal sealed class BitonalTileSource : ITileSource
    {
        private readonly BitonalTile _bitonalTile;
        private readonly BeginTileArray _tileArray;

        public BitonalTileSource(BitonalTile bitonalTile, BeginTileArray tileArray)
        {
            _bitonalTile = bitonalTile ?? throw new ArgumentNullException(nameof(bitonalTile));
            _tileArray = tileArray ?? throw new ArgumentNullException(nameof(tileArray));
            DpiX = (int)Math.Round(tileArray.PathDirectionCellSize * 25.4);
            DpiY = (int)Math.Round(tileArray.LineDirectionCellSize * 25.4);
        }

        public bool IsBlackAndWhite => true;

        public int CompressionType => _bitonalTile.CompressionType;
        public byte[] CompressedCells => _bitonalTile.CompressedCells;
        public StructuredDataRecord Parameters => _bitonalTile.Parameters;

        public int Width => _tileArray.PathDirectionImageCellCount;
        public int Height => _tileArray.LineDirectionImageCellCount;
        public int DpiX { get; }
        public int DpiY { get; }
        public int ColorPrecision => 1; // we're black/white; one bit of precision is enough.
    }
}
