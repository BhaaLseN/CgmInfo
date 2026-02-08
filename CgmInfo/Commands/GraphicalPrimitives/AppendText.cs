using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("APNDTEXT")]
    public class AppendText : Command
    {
        public AppendText(FinalFlag final, string text)
            : base(4, 6)
        {
            Final = final;
            Text = text;
        }
        public FinalFlag Final { get; }
        public string Text { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveAppendText(this, parameter);
        }
    }
}
