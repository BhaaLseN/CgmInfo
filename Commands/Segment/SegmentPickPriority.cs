using CgmInfo.Traversal;

namespace CgmInfo.Commands.Segment
{
    public class SegmentPickPriority : Command
    {
        public SegmentPickPriority(int segmentIdentifier, int priority)
            : base(8, 7)
        {
            SegmentIdentifier = segmentIdentifier;
            Priority = priority;
        }

        public int SegmentIdentifier { get; private set; }
        public int Priority { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptSegmentSegmentPickPriority(this, parameter);
        }
    }
}
