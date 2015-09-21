using CgmInfo.Commands.Enums;
using CgmInfo.Commands.PictureDescriptor;

namespace CgmInfo.TextEncoding
{
    internal static class PictureDescriptorReader
    {
        public static ScalingMode ScalingMode(MetafileReader reader)
        {
            return new ScalingMode(ParseScalingMode(reader.ReadEnum()), reader.ReadReal());
        }

        public static VdcExtent VdcExtent(MetafileReader reader)
        {
            var firstCorner = reader.ReadPoint();
            var secondCorner = reader.ReadPoint();
            return new VdcExtent(firstCorner, secondCorner);
        }

        private static ScalingModeType ParseScalingMode(string token)
        {
            // assume abstract; unless its metric
            if (token.ToUpperInvariant() == "METRIC")
                return ScalingModeType.Metric;
            return ScalingModeType.Abstract;
        }
    }
}
