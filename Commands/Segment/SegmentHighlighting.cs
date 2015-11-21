using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Segment
{
    public class SegmentHighlighting : Command
    {
        public SegmentHighlighting(int segmentIdentifier, Highlighting highlighting)
            : base(8, 5)
        {
            SegmentIdentifier = segmentIdentifier;
            Highlighting = highlighting;
        }

        public int SegmentIdentifier { get; private set; }
        public Highlighting Highlighting { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptSegmentSegmentHighlighting(this, parameter);
        }
    }
}
