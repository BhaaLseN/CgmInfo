using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class MaximumVdcExtent : Command
    {
        public MaximumVdcExtent(PointF firstCorner, PointF secondCorner)
            : base(1, 17)
        {
            FirstCorner = firstCorner;
            SecondCorner = secondCorner;
        }

        public PointF FirstCorner { get; private set; }
        public PointF SecondCorner { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorMaximumVdcExtent(this, parameter);
        }
    }
}
