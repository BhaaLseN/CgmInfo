using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class BitonalTile : Command
    {
        public BitonalTile(int compressionType, int rowPaddingIndicator, MetafileColor cellBackgroundColor, MetafileColor cellForegroundColor, StructuredDataRecord parameters, byte[] compressedCells)
            : base(4, 28)
        {
            CompressionType = compressionType;
            CompressionTypeName = MetafileCompressionTypes.GetName(compressionType);
            RowPaddingIndicator = rowPaddingIndicator;
            CellBackgroundColor = cellBackgroundColor;
            CellForegroundColor = cellForegroundColor;
            Parameters = parameters;
            CompressedCells = compressedCells;
        }

        public int CompressionType { get; }
        public string CompressionTypeName { get; }
        public int RowPaddingIndicator { get; }
        public MetafileColor CellBackgroundColor { get; }
        public MetafileColor CellForegroundColor { get; }
        public StructuredDataRecord Parameters { get; }
        public byte[] CompressedCells { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveBitonalTile(this, parameter);
        }
    }
}
