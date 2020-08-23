using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("COLRTABLE")]
    public class ColorTable : Command
    {
        public ColorTable(int startIndex, MetafileColor[] colors)
            : base(5, 34)
        {
            StartIndex = startIndex;
            Colors = colors;
        }

        public int StartIndex { get; }
        public MetafileColor[] Colors { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeColorTable(this, parameter);
        }
    }
}
