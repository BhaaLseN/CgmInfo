using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class IndexPrecision : Command
    {
        public IndexPrecision(int precision)
            : base(1, 6)
        {
            Precision = precision;
        }

        public int Precision { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorIndexPrecision(this, parameter);
        }
    }
}
