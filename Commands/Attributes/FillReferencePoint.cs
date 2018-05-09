using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.Attributes
{
    public class FillReferencePoint : Command
    {
        public FillReferencePoint(MetafilePoint referencePoint)
            : base(5, 31)
        {
            ReferencePoint = referencePoint;
        }

        public MetafilePoint ReferencePoint { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeFillReferencePoint(this, parameter);
        }
    }
}
