using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    public class PatternSize : Command
    {
        public PatternSize(PointF width, PointF height)
            : base(5, 33)
        {
            Width = width;
            Height = height;
        }

        public PointF Width { get; private set; }
        public PointF Height { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributePatternSize(this, parameter);
        }
    }
}
