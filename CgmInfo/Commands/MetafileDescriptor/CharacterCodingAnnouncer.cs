using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("CHARCODING")]
    public class CharacterCodingAnnouncer : Command
    {
        public CharacterCodingAnnouncer(CharacterCodingAnnouncerType characterCodingAnnouncerType)
            : base(1, 15)
        {
            CharacterCodingAnnouncerType = characterCodingAnnouncerType;
        }

        public CharacterCodingAnnouncerType CharacterCodingAnnouncerType { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorCharacterCodingAnnouncer(this, parameter);
        }
    }
}
