using System;
using CgmInfo.Commands;
using CgmInfo.Commands.GraphicalPrimitives;

namespace CgmInfoGui.ViewModels.Nodes.Sources
{
    internal sealed class BitonalTileSource : ITileSource
    {
        private readonly BitonalTile _bitonalTile;

        public BitonalTileSource(BitonalTile bitonalTile)
        {
            _bitonalTile = bitonalTile ?? throw new ArgumentNullException(nameof(bitonalTile));
        }

        public int CompressionType => _bitonalTile.CompressionType;
        public byte[] CompressedCells => _bitonalTile.CompressedCells;
        public StructuredDataRecord Parameters => _bitonalTile.Parameters;
    }
}
