using CgmInfo.Commands.MetafileDescriptor;

namespace CgmInfo.BinaryEncoding
{
    internal static class ControlElementReader
    {
        public static VdcIntegerPrecision VdcIntegerPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) VDC integer precision; legal values are 16, 24 or 32; the value 8 is not permitted. [ISO/IEC 8632-3 8.5]
            return new VdcIntegerPrecision(reader.ReadInteger());
        }
    }
}
