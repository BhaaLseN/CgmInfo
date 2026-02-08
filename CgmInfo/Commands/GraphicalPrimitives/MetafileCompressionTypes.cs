using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public static class MetafileCompressionTypes
    {
        public static IReadOnlyDictionary<int, string> KnownCompressionTypes { get; } = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // compression types originally part of ISO/IEC 8632:1999
            { 0, "Null Background" },
            { 1, "Null Foreground" },
            { 2, "T6" },
            { 3, "T4 1-dimensional" },
            { 4, "T4 2-dimensional" },
            { 5, "Bitmap (uncompressed)" },
            { 6, "Run Length" },
            // line types later registered with the ISO/IEC 9973 Items Register
            { 7, "Baseline JPEG" },
            { 8, "LZW" },
            { 9, "PNG Compression Method 0" },
        });
        public static string GetName(int index)
        {
            if (KnownCompressionTypes.TryGetValue(index, out string name))
                return name;

            return "Reserved";
        }
    }
}
