using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("MITRELIMIT")]
    public class MiterLimit : Command
    {
        public MiterLimit(double limit)
            : base(3, 19)
        {
            Limit = limit;
        }

        public double Limit { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptControlMiterLimit(this, parameter);
        }
    }
}
