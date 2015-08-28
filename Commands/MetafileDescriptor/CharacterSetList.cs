using System.Collections.Generic;
using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class CharacterSetList : Command
    {
        public CharacterSetList(IEnumerable<CharacterSetListEntry> entries)
            : base(1, 14)
        {
            Entries = entries;
        }

        public IEnumerable<CharacterSetListEntry> Entries { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorCharacterSetList(this, parameter);
        }
    }
    public class CharacterSetListEntry
    {
        public CharacterSetListEntry(CharacterSetType characterSetType, string designationSequenceTail)
        {
            CharacterSetType = characterSetType;
            DesignationSequenceTail = designationSequenceTail;
        }

        public CharacterSetType CharacterSetType { get; private set; }
        public string DesignationSequenceTail { get; private set; }
    }
}
