using CgmInfo.Traversal;

namespace CgmInfo.Commands.Delimiter
{
    [TextToken("BEGPIC")]
    public class BeginPicture : Command
    {
        public BeginPicture(string name)
            : base(0, 3)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptDelimiterBeginPicture(this, parameter);
        }
    }
}
