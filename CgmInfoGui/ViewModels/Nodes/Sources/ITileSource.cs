using CgmInfo.Commands;

namespace CgmInfoGui.ViewModels.Nodes.Sources;

public interface ITileSource
{
    bool IsBlackAndWhite { get; }
    int CompressionType { get; }
    byte[] CompressedCells { get; }
    StructuredDataRecord Parameters { get; }

    // Width/Height in Pixels
    int Width { get; }
    int Height { get; }
    int DpiX { get; }
    int DpiY { get; }
    int ColorPrecision { get; }
}
