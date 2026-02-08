using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("TEXT")]
    public class TextCommand : Command
    {
        public TextCommand(MetafilePoint position, FinalFlag final, string text)
            : base(4, 4)
        {
            Position = position;
            Final = final;
            Text = text;
        }
        public MetafilePoint Position { get; }
        public FinalFlag Final { get; }
        public string Text { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveText(this, parameter);
        }
    }
}
