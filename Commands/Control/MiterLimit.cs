using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class MiterLimit : Command
    {
        public MiterLimit(double limit)
            : base(3, 19)
        {
            Limit = limit;
        }

        public double Limit { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptControlMiterLimit(this, parameter);
        }
    }
}
