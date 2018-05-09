using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.Segment
{
    public class SegmentTransformation : Command
    {
        public SegmentTransformation(int segmentIdentifier, MetafileMatrix matrix)
            : base(8, 4)
        {
            SegmentIdentifier = segmentIdentifier;
            Matrix = matrix;
        }

        public int SegmentIdentifier { get; private set; }
        public MetafileMatrix Matrix { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptSegmentSegmentTransformation(this, parameter);
        }
    }
}
