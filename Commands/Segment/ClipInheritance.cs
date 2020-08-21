using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Segment
{
    [TextToken("CLIPINH")]
    public class ClipInheritance : Command
    {
        public ClipInheritance(ClipInheritanceType inheritanceType)
            : base(8, 3)
        {
            InheritanceType = inheritanceType;
        }

        public ClipInheritanceType InheritanceType { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptSegmentClipInheritance(this, parameter);
        }
    }
}
