namespace CgmInfo.Commands.Enums
{
    public enum CharacterSetType
    {
        [TextToken("STD94")]
        GSet94Characters = 0,
        [TextToken("STD96")]
        GSet96Characters = 1,
        [TextToken("STD94MULTIBYTE")]
        GSet94CharactersMultibyte = 2,
        [TextToken("STD96MULTIBYTE")]
        GSet96CharactersMultibyte = 3,
        [TextToken("COMPLETECODE")]
        CompleteCode = 4,
    }
}
