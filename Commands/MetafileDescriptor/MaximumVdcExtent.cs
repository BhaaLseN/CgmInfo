using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("MAXVDCEXT")]
    public class MaximumVdcExtent : Command
    {
        public MaximumVdcExtent(MetafilePoint firstCorner, MetafilePoint secondCorner)
            : base(1, 17)
        {
            FirstCorner = firstCorner;
            SecondCorner = secondCorner;
        }

        public MetafilePoint FirstCorner { get; }
        public MetafilePoint SecondCorner { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorMaximumVdcExtent(this, parameter);
        }
    }
}
