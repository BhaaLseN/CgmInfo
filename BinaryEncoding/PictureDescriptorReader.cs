using CgmInfo.Commands.PictureDescriptor;

namespace CgmInfo.BinaryEncoding
{
    internal static class PictureDescriptorReader
    {
        public static VdcExtent VdcExtent(MetafileReader reader, CommandHeader header)
        {
            // P1: (point) first corner
            // P2: (point) second corner
            var firstCorner = reader.ReadPoint();
            var secondCorner = reader.ReadPoint();
            return new VdcExtent(firstCorner, secondCorner);
        }
    }
}
