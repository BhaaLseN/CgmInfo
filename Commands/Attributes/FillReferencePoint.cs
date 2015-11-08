using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    public class FillReferencePoint : Command
    {
        public FillReferencePoint(PointF referencePoint)
            : base(5, 31)
        {
            ReferencePoint = referencePoint;
        }

        public PointF ReferencePoint { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeFillReferencePoint(this, parameter);
        }
    }
}
