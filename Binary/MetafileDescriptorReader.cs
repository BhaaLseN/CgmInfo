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

        public static VdcType VdcType(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) VDC TYPE: valid values are [ISO/IEC 8632-3 8.3]
            //      0 VDC values specified in integers
            //      1 VDC values specified in reals
            return new VdcType(reader.ReadInteger(commandHeader.ParameterListLength));
        }

        public static IntegerPrecision IntegerPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) integer precision: valid values are 8, 16, 24 or 32 [ISO/IEC 8632-3 8.3]
            return new IntegerPrecision(reader.ReadInteger(commandHeader.ParameterListLength));
        }

        public static RealPrecision RealPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (enumerated) form of representation for real values: valid values are [ISO/IEC 8632-3 8.3]
            //      0 floating point format
            //      1 fixed point format
            // P2: (integer) field width for exponent or whole part(including 1 bit for sign)
            // P3: (integer) field width for fraction or fractional part
            return new RealPrecision(reader.ReadInteger(2), reader.ReadInteger(2), reader.ReadInteger(2));
        }

        public static IndexPrecision IndexPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) Index precision: valid values are 8,16,24,32 [ISO/IEC 8632-3 8.3]
            return new IndexPrecision(reader.ReadInteger(commandHeader.ParameterListLength));
        }

        public static ColorPrecision ColorPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) Colour precision: valid values are 8,16,24,32 [ISO/IEC 8632-3 8.3]
            return new ColorPrecision(reader.ReadInteger(commandHeader.ParameterListLength));
        }

        public static ColorIndexPrecision ColorIndexPrecision(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (integer) Colour index precision: valid values are 8,16,24,32 [ISO/IEC 8632-3 8.3]
            return new ColorIndexPrecision(reader.ReadInteger(commandHeader.ParameterListLength));
        }

        public static MaximumColorIndex MaximumColorIndex(MetafileReader reader, CommandHeader commandHeader)
        {
            // P1: (colour index) maximum colour index that may be encountered in the metafile. [ISO/IEC 8632-3 8.3]
            return new MaximumColorIndex(reader.ReadInteger(commandHeader.ParameterListLength));
        }
    }
}
