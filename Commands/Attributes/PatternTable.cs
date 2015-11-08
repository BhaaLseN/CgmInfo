using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.Attributes
{
    public class PatternTable : Command
    {
        public PatternTable(int index, int width, int height, MetafileColor[] colors)
            : base(5, 32)
        {
            Index = index;
            Width = width;
            Height = height;
            Colors = colors;
        }

        public int Index { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public MetafileColor[] Colors { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributePatternTable(this, parameter);
        }
    }
}
