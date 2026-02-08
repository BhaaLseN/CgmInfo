namespace CgmInfo.Commands.Enums
{
    public enum InteriorStyleType
    {
        [TextToken("HOLLOW")]
        Hollow = 0,
        [TextToken("SOLID")]
        Solid = 1,
        [TextToken("PAT")]
        Pattern = 2,
        [TextToken("HATCH")]
        Hatch = 3,
        [TextToken("EMPTY")]
        Empty = 4,
        [TextToken("GEOPAT")]
        GeometricPattern = 5,
        [TextToken("INTERP")]
        Interpolated = 6,
    }
}
