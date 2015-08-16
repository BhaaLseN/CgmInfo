using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class CharacterSetList : Command
    {
        public CharacterSetList(int characterSetType, string designationSequenceTail)
            : base(1, 14)
        {
            CharacterSetType = (CharacterSetType)characterSetType;
            DesignationSequenceTail = designationSequenceTail;
        }

        public CharacterSetType CharacterSetType { get; private set; }
        public string DesignationSequenceTail { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorCharacterSetList(this, parameter);
        }
    }
}
