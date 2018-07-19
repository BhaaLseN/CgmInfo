using System;
using CgmInfo.Commands.Delimiter;

namespace CgmInfoGui.ViewModels.Nodes
{
    public class TileArrayViewModel : NodeBase
    {
        private readonly string _displayName;

        public TileArrayViewModel(BeginTileArray beginTileArray)
        {
            // TODO: this is probably wrong for non-metric scaling modes.
            // FIXME: this is certainly wrong for metric scaling modes with scale != 1.0.
            double dpiX = Math.Round(beginTileArray.PathDirectionCellSize * 25.4, 0);
            double dpiY = Math.Round(beginTileArray.LineDirectionCellSize * 25.4, 0);
            string dpiEstimate;
            if (Math.Abs(dpiX - dpiY) < 1.0)
                dpiEstimate = dpiX + " DPI";
            else
                dpiEstimate = $"X: {dpiX} DPI, Y: {dpiY} DPI";
            _displayName = string.Format(
                "BEGIN TILE ARRAY: {0} by {1} tiles, {2} by {3} cells each (estimated {4})",
                beginTileArray.PathDirectionTileCount, beginTileArray.LineDirectionTileCount,
                beginTileArray.PathDirectionCellCount, beginTileArray.LineDirectionCellCount,
                dpiEstimate);

            Descriptor = new SimpleNode("TILE ARRAY DESCRIPTOR")
            {
                new SimpleNode(string.Format("Position: {0}", beginTileArray.Position)),
                new SimpleNode(string.Format("Cell Path Direction: {0}", beginTileArray.CellPathDirection)),
                new SimpleNode(string.Format("Line Progression Direction: {0}", beginTileArray.LineProgressionDirection)),
                new SimpleNode(string.Format("Number of Tiles in Path Direction: {0}", beginTileArray.PathDirectionTileCount)),
                new SimpleNode(string.Format("Number of Tiles in Line Direction: {0}", beginTileArray.LineDirectionTileCount)),
                new SimpleNode(string.Format("Number of Cells per Tile in Path Direction: {0}", beginTileArray.PathDirectionCellCount)),
                new SimpleNode(string.Format("Number of Cells per Tile in Line Direction: {0}", beginTileArray.LineDirectionCellCount)),
                new SimpleNode(string.Format("Cell Size in Path Direction: {0}", beginTileArray.PathDirectionCellSize)),
                new SimpleNode(string.Format("Cell Size in Line Direction: {0}", beginTileArray.LineDirectionCellSize)),
                new SimpleNode(string.Format("Image Offset in Path Direction: {0}", beginTileArray.PathDirectionImageOffset)),
                new SimpleNode(string.Format("Image Offset in Line Direction: {0}", beginTileArray.LineDirectionImageOffset)),
                new SimpleNode(string.Format("Image Number of Cells per Tile in Path Direction: {0}", beginTileArray.PathDirectionImageCellCount)),
                new SimpleNode(string.Format("Image Number of Cells per Tile in Line Direction: {0}", beginTileArray.LineDirectionImageCellCount)),
            };
            Nodes.Add(Descriptor);
        }
        public SimpleNode Descriptor { get; }

        public override string DisplayName
        {
            get { return _displayName; }
        }
    }
}
