using System.Drawing;
using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class TextCommand : Command
    {
        public TextCommand(double positionX, double positionY, FinalFlag final, string text)
            : base(4, 4)
        {
            Position = new PointF((float)positionX, (float)positionY);
            Final = final;
            Text = text;
        }
        public PointF Position { get; private set; }
        public FinalFlag Final { get; private set; }
        public string Text { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveText(this, parameter);
        }
    }
}
