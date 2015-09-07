using CgmInfo.Commands.MetafileDescriptor;

namespace CgmInfo.TextEncoding
{
    internal static class ControlElementReader
    {
        public static VdcIntegerPrecision VdcIntegerPrecision(MetafileReader reader)
        {
            return new VdcIntegerPrecision(TextEncodingHelper.GetBitPrecision(reader.ReadInteger(), reader.ReadInteger()));
        }
    }
}
