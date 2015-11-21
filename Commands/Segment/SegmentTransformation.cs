using System.Drawing.Drawing2D;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Segment
{
    public class SegmentTransformation : Command
    {
        public SegmentTransformation(int segmentIdentifier, Matrix matrix)
            : base(8, 4)
        {
            SegmentIdentifier = segmentIdentifier;
            Matrix = matrix;
        }

        public int SegmentIdentifier { get; private set; }
        public Matrix Matrix { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptSegmentSegmentTransformation(this, parameter);
        }
    }
}
