using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    [TextToken("BEGAPS")]
    public class BeginApplicationStructure : Command
    {
        public BeginApplicationStructure(string identifier, string type, InheritanceFlag inheritanceFlag)
            : base(0, 21)
        {
            Identifier = identifier;
            Type = type;
            Inheritance = inheritanceFlag;
        }

        public string Identifier { get; }
        public string Type { get; }
        public InheritanceFlag Inheritance { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterBeginApplicationStructure(this, parameter);
        }
    }
}
