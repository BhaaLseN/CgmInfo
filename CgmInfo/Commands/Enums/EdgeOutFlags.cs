namespace CgmInfo.Commands.Enums
{
    public enum EdgeOutFlags
    {
        [TextToken("INVIS")]
        Invisible = 0,
        [TextToken("VIS")]
        Visible = 1,
        [TextToken("CLOSEINVIS")]
        InvisibleClose = 2,
        [TextToken("CLOSEVIS")]
        VisibleClose = 3,
    }
}
