namespace CgmInfo.Commands.Enums
{
    public enum WidthSpecificationModeType
    {
        [TextToken("ABS")]
        Absolute = 0,
        [TextToken("SCALED")]
        Scaled = 1,
        [TextToken("FRACTIONAL")]
        Fractional = 2,
        [TextToken("MM")]
        Millimeters = 3,
    }
}
