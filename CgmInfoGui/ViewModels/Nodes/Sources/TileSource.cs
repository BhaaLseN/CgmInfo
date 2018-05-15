using System;
using CgmInfo.Commands;
using CgmInfo.Commands.GraphicalPrimitives;

namespace CgmInfoGui.ViewModels.Nodes.Sources
{
    internal sealed class TileSource : ITileSource
    {
        private readonly Tile _tile;

        public TileSource(Tile tile)
        {
            _tile = tile ?? throw new ArgumentNullException(nameof(tile));
        }

        public int CompressionType => _tile.CompressionType;
        public byte[] CompressedCells => _tile.CompressedCells;
        public StructuredDataRecord Parameters => _tile.Parameters;
    }
}
