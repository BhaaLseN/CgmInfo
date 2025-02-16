using System;
using CgmInfo.Commands;
using CgmInfo.Commands.Delimiter;
using CgmInfo.Commands.GraphicalPrimitives;

namespace CgmInfoGui.ViewModels.Nodes.Sources;

internal sealed class TileSource : ITileSource
{
    private readonly Tile _tile;
    private readonly BeginTileArray _tileArray;

    public TileSource(Tile tile, BeginTileArray tileArray)
    {
        _tile = tile ?? throw new ArgumentNullException(nameof(tile));
        _tileArray = tileArray ?? throw new ArgumentNullException(nameof(tileArray));
        DpiX = (int)Math.Round(tileArray.PathDirectionCellSize * 25.4);
        DpiY = (int)Math.Round(tileArray.LineDirectionCellSize * 25.4);
    }

    public bool IsBlackAndWhite => false;

    public int CompressionType => _tile.CompressionType;
    public byte[] CompressedCells => _tile.CompressedCells;
    public StructuredDataRecord Parameters => _tile.Parameters;

    public int Width => _tileArray.PathDirectionImageCellCount;
    public int Height => _tileArray.LineDirectionImageCellCount;
    public int DpiX { get; }
    public int DpiY { get; }
    public int ColorPrecision => _tile.CellColorPrecision;
}
