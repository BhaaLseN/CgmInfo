using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("RESTRTEXT")]
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
        public double DeltaWidth { get; }
        public double DeltaHeight { get; }
        public MetafilePoint Position { get; }
        public FinalFlag Final { get; }
        public string Text { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveRestrictedText(this, parameter);
        }
    }
}
