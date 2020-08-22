using CgmInfo.Traversal;

namespace CgmInfo.Commands.Segment
{
    [TextToken("SEGDISPPRI")]
    public class SegmentDisplayPriority : Command
    {
        public SegmentDisplayPriority(int segmentIdentifier, int priority)
            : base(8, 6)
        {
            SegmentIdentifier = segmentIdentifier;
            Priority = priority;
        }

        public int SegmentIdentifier { get; }
        public int Priority { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptSegmentSegmentDisplayPriority(this, parameter);
        }
    }
}
