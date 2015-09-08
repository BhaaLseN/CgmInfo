using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class ClipRectangle : Command
    {
        public ClipRectangle(PointF firstCorner, PointF secondCorner)
            : base(3, 5)
        {
            FirstCorner = firstCorner;
            SecondCorner = secondCorner;
        }

        public PointF FirstCorner { get; private set; }
        public PointF SecondCorner { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptControlClipRectangle(this, parameter);
        }
    }
}
