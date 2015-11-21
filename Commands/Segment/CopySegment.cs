using System.Drawing.Drawing2D;
using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Segment
{
    public class CopySegment : Command
    {
        public CopySegment(int segmentIdentifier, Matrix matrix, SegmentTransformationApplication transformationApplication)
            : base(8, 1)
        {
            SegmentIdentifier = segmentIdentifier;
            Matrix = matrix;
            TransformationApplication = transformationApplication;
        }

        public int SegmentIdentifier { get; private set; }
        public Matrix Matrix { get; private set; }
        public SegmentTransformationApplication TransformationApplication { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptSegmentCopySegment(this, parameter);
        }
    }
}
