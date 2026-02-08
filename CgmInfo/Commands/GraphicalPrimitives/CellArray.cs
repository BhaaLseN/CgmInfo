using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("CELLARRAY")]
    public class CellArray : Command
    {
        public CellArray(MetafilePoint cornerPointP, MetafilePoint cornerPointQ, MetafilePoint cornerPointR, int nx, int ny, MetafileColor[] colors)
            : base(4, 9)
        {
            CornerPointP = cornerPointP;
            CornerPointQ = cornerPointQ;
            CornerPointR = cornerPointR;
            NX = nx;
            NY = ny;
            Colors = colors;
        }

        public MetafilePoint CornerPointP { get; }
        public MetafilePoint CornerPointQ { get; }
        public MetafilePoint CornerPointR { get; }
        public int NX { get; }
        public int NY { get; }
        public MetafileColor[] Colors { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitiveCellArray(this, parameter);
        }
    }
}
