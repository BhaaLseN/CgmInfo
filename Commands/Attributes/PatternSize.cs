using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.Attributes
{
    public class PatternSize : Command
    {
        public PatternSize(MetafilePoint width, MetafilePoint height)
            : base(5, 33)
        {
            Width = width;
            Height = height;
        }

        public MetafilePoint Width { get; private set; }
        public MetafilePoint Height { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributePatternSize(this, parameter);
        }
    }
}
