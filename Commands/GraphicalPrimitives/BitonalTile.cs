using System.Collections.Generic;
using System.Collections.ObjectModel;
using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class BitonalTile : Command
    {
        public BitonalTile(int compressionType, int rowPaddingIndicator, MetafileColor cellBackgroundColor, MetafileColor cellForegroundColor, StructuredDataRecord parameters)
            : base(4, 28)
        {
            CompressionType = compressionType;
            CompressionTypeName = GetCompressionTypeName(compressionType);
            RowPaddingIndicator = rowPaddingIndicator;
            CellBackgroundColor = cellBackgroundColor;
            CellForegroundColor = cellForegroundColor;
            Parameters = parameters;
        }

        public int CompressionType { get; }
        public string CompressionTypeName { get; }
        public int RowPaddingIndicator { get; }
        public MetafileColor CellBackgroundColor { get; }
        public MetafileColor CellForegroundColor { get; }
        public StructuredDataRecord Parameters { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveBitonalTile(this, parameter);
        }

        public static IReadOnlyDictionary<int, string> KnownCompressionTypes { get; } = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // compression types originally part of ISO/IEC 8632:1999
            { 0, "Null Background" },
            { 1, "Null Foreground" },
            { 2, "T6" },
            { 3, "1-dimensional" },
            { 4, "T4 2-dimensional" },
            { 5, "Bitmap (uncompressed)" },
            { 6, "Run Length" },
        });
        public static string GetCompressionTypeName(int index)
        {
            if (KnownCompressionTypes.TryGetValue(index, out string name))
                return name;

            return "Reserved";
        }
    }
}
