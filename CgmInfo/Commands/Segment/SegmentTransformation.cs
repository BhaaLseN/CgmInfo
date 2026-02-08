using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.Segment
{
    [TextToken("SEGTRAN")]
    public class SegmentTransformation : Command
    {
        public SegmentTransformation(int segmentIdentifier, MetafileMatrix matrix)
            : base(8, 4)
        {
            SegmentIdentifier = segmentIdentifier;
            Matrix = matrix;
        }

        public int SegmentIdentifier { get; }
        public MetafileMatrix Matrix { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptSegmentSegmentTransformation(this, parameter);
        }
    }
}
