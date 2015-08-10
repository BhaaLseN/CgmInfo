using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class MaximumVdcExtent : Command
    {
        public MaximumVdcExtent(double x1, double y1, double x2, double y2)
            : base(1, 17)
        {
            FirstCorner = new PointF((float)x1, (float)y1);
            SecondCorner = new PointF((float)x2, (float)y2);
        }

        public PointF FirstCorner { get; private set; }
        public PointF SecondCorner { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorMaximumVdcExtent(this, parameter);
        }
    }
}
