using CgmInfo.Traversal;

namespace CgmInfo.Commands.Segment
{
    public class SegmentDisplayPriority : Command
    {
        public SegmentDisplayPriority(int segmentIdentifier, int priority)
            : base(8, 6)
        {
            SegmentIdentifier = segmentIdentifier;
            Priority = priority;
        }

        public int SegmentIdentifier { get; private set; }
        public int Priority { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptSegmentSegmentDisplayPriority(this, parameter);
        }
    }
}
