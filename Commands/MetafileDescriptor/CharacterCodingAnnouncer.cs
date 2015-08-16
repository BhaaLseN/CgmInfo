using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class CharacterCodingAnnouncer : Command
    {
        public CharacterCodingAnnouncer(int characterCodingAnnouncerType)
            : base(1, 15)
        {
            CharacterCodingAnnouncerType = (CharacterCodingAnnouncerType)characterCodingAnnouncerType;
        }

        public CharacterCodingAnnouncerType CharacterCodingAnnouncerType { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorCharacterCodingAnnouncer(this, parameter);
        }
    }
}
