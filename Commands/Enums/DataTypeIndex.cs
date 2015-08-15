namespace CgmInfo.Commands.Enums
{
    public enum DataTypeIndex
    {
        // Values [ISO/IEC 8632-1 Annex C, C.2.2]
        // Descriptions [ISO/IEC 8632-1 7.1, Table 10]
        StructuredDataRecord = 1,
        ColorIndex = 2,
        ColorDirect = 3,
        Name = 4,
        Enumerated = 5,
        Integer = 6,
        Reserved = 7,
        SignedInteger8bit = 8,
        SignedInteger16bit = 9,
        SignedInteger32bit = 10,
        Index = 11,
        Real = 12,
        String = 13,
        StringFixed = 14,
        ViewportCoordinate = 15,
        VDC = 16,
        ColorComponent = 17,
        UnsignedInteger8bit = 18,
        UnsignedInteger32Bit = 19,
        BitStream = 20,
        ColorList = 21,
        UnsignedInteger16bit = 22,
    }
}
