using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    public class LineTypeInitialOffset : Command
    {
        public LineTypeInitialOffset(double offset)
            : base(5, 40)
        {
            Offset = offset;
        }

        public double Offset { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeLineTypeInitialOffset(this, parameter);
        }
    }
}
