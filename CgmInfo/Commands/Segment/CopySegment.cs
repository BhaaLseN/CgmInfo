using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.Segment
{
    [TextToken("COPYSEG")]
    public class CopySegment : Command
    {
        public CopySegment(int segmentIdentifier, MetafileMatrix matrix, SegmentTransformationApplication transformationApplication)
            : base(8, 1)
        {
            SegmentIdentifier = segmentIdentifier;
            Matrix = matrix;
            TransformationApplication = transformationApplication;
        }

        public int SegmentIdentifier { get; }
        public MetafileMatrix Matrix { get; }
        public SegmentTransformationApplication TransformationApplication { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptSegmentCopySegment(this, parameter);
        }
    }
}
