using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.Segment
{
    public class CopySegment : Command
    {
        public CopySegment(int segmentIdentifier, MetafileMatrix matrix, SegmentTransformationApplication transformationApplication)
            : base(8, 1)
        {
            SegmentIdentifier = segmentIdentifier;
            Matrix = matrix;
            TransformationApplication = transformationApplication;
        }

        public int SegmentIdentifier { get; private set; }
        public MetafileMatrix Matrix { get; private set; }
        public SegmentTransformationApplication TransformationApplication { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptSegmentCopySegment(this, parameter);
        }
    }
}
