using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    [TextToken("BEGMF")]
    public class BeginMetafile : Command
    {
        public BeginMetafile(string name)
            : base(0, 1)
        {
            Name = name;
        }

        public string Name { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterBeginMetafile(this, parameter);
        }
    }
}
