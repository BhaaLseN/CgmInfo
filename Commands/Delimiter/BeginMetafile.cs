using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    public class BeginMetafile : Command
    {
        public BeginMetafile(string name)
            : base(0, 1)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterBeginMetafile(this, parameter);
        }
    }
}
