using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class RestrictedText : Command
    {
        public RestrictedText(double deltaWidth, double deltaHeight, MetafilePoint position, FinalFlag final, string text)
            : base(4, 5)
        {
            DeltaWidth = deltaWidth;
            DeltaHeight = deltaHeight;
            Position = position;
            Final = final;
            Text = text;
        }
        public double DeltaWidth { get; private set; }
        public double DeltaHeight { get; private set; }
        public MetafilePoint Position { get; private set; }
        public FinalFlag Final { get; private set; }
        public string Text { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveRestrictedText(this, parameter);
        }
    }
}
