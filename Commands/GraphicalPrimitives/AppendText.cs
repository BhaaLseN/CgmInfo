using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class AppendText : Command
    {
        public AppendText(FinalFlag final, string text)
            : base(4, 6)
        {
            Final = final;
            Text = text;
        }
        public FinalFlag Final { get; private set; }
        public string Text { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveAppendText(this, parameter);
        }
    }
}
