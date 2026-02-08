namespace CgmInfo.Commands.Enums
{
    public enum AspectSourceFlagsType
    {
        [TextToken("LINETYPE")]
        LineType = 0,
        [TextToken("LINEWIDTH")]
        LineWidth = 1,
        [TextToken("LINECOLR")]
        LineColor = 2,
        [TextToken("MARKERTYPE")]
        MarkerType = 3,
        [TextToken("MARKERSIZE")]
        MarkerSize = 4,
        [TextToken("MARKERCOLR")]
        MarkerColor = 5,
        [TextToken("TEXTFONTINDEX")]
        TextFontIndex = 6,
        [TextToken("TEXTPREC")]
        TextPrecision = 7,
        [TextToken("CHAREXPAN")]
        CharacterExpansionFactor = 8,
        [TextToken("CHARSPACE")]
        CharacterSpacing = 9,
        [TextToken("TEXTCOLR")]
        TextColor = 10,
        [TextToken("INTSTYLE")]
        InteriorStyle = 11,
        [TextToken("FILLCOLR")]
        FillColor = 12,
        [TextToken("HATCHINDEX")]
        HatchIndex = 13,
        [TextToken("PATINDEX")]
        PatternIndex = 14,
        [TextToken("EDGETYPE")]
        EdgeType = 15,
        [TextToken("EDGEWIDTH")]
        EdgeWidth = 16,
        [TextToken("EDGECOLR")]
        EdgeColor = 17,
    }
}
