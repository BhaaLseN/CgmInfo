using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("CLIPRECT")]
    public class ClipRectangle : Command
    {
        public ClipRectangle(MetafilePoint firstCorner, MetafilePoint secondCorner)
            : base(3, 5)
        {
            FirstCorner = firstCorner;
            SecondCorner = secondCorner;
        }

        public MetafilePoint FirstCorner { get; private set; }
        public MetafilePoint SecondCorner { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptControlClipRectangle(this, parameter);
        }
    }
}
