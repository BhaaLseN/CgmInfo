using CgmInfo.Commands;

namespace CgmInfoGui.ViewModels.Nodes.Sources
{
    public interface ITileSource
    {
        int CompressionType { get; }
        byte[] CompressedCells { get; }
        StructuredDataRecord Parameters { get; }
    }
}
