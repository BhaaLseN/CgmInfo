using CgmInfo.Commands.MetafileDescriptor;

namespace CgmInfo.Binary
{
    internal static class MetafileDescriptorReader
    {
        public static MetafileVersion MetafileVersion(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) metafile version number: valid values are 1, 2, 3, 4 [ISO/IEC 8632-3 8.3]
            return new MetafileVersion(reader.ReadInteger(commandHeader.ParameterListLength));
        }

        public static MetafileDescription MetafileDescription(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (string fixed) metafile description string [ISO/IEC 8632-3 8.3]
            return new MetafileDescription(reader.ReadString());
        }
    }
}
