using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("PATTABLE")]
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

        public int Index { get; }
        public int Width { get; }
        public int Height { get; }
        public MetafileColor[] Colors { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributePatternTable(this, parameter);
        }
    }
}
