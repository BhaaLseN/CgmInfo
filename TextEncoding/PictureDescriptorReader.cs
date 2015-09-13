using CgmInfo.Commands.PictureDescriptor;

namespace CgmInfo.TextEncoding
{
    internal static class PictureDescriptorReader
    {
        public static VdcExtent VdcExtent(MetafileReader reader)
        {
            var firstCorner = reader.ReadPoint();
            var secondCorner = reader.ReadPoint();
            return new VdcExtent(firstCorner, secondCorner);
        }
    }
}
