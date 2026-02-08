namespace CgmInfo.Commands.Enums
{
    public enum ClippingMode
    {
        [TextToken("LOCUS")]
        Locus = 0,
        [TextToken("SHAPE")]
        Shape = 1,
        [TextToken("LOCUSTHENSHAPE")]
        LocusThenShape = 2,
    }
}
