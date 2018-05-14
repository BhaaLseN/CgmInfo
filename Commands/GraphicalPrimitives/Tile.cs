using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class Tile : Command
    {
        public Tile(int compressionType, int rowPaddingIndicator, int cellColorPrecision, StructuredDataRecord parameters)
            : base(4, 29)
        {
            CompressionType = compressionType;
            CompressionTypeName = MetafileCompressionTypes.GetName(compressionType);
            RowPaddingIndicator = rowPaddingIndicator;
            CellColorPrecision = cellColorPrecision;
            Parameters = parameters;
        }

        public int CompressionType { get; }
        public string CompressionTypeName { get; }
        public int RowPaddingIndicator { get; }
        public int CellColorPrecision { get; }
        public StructuredDataRecord Parameters { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveTile(this, parameter);
        }
    }
}
