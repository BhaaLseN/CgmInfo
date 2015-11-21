using CgmInfo.Commands.Enums;
using CgmInfo.Commands.Segment;

namespace CgmInfo.TextEncoding
{
    internal static class SegmentReader
    {
        public static CopySegment CopySegment(MetafileReader reader)
        {
            return new CopySegment(reader.ReadName(), reader.ReadMatrix(), ParseTransformationApplication(reader.ReadEnum()));
        }

        private static SegmentTransformationApplication ParseTransformationApplication(string token)
        {
            // assume "no" unless its "yes"
            if (token.ToUpperInvariant() == "YES")
                return SegmentTransformationApplication.Yes;
            return SegmentTransformationApplication.No;
        }
    }
}
