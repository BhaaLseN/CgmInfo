using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("VDCTYPE")]
    public class VdcType : Command
    {
        public VdcType(VdcTypeSpecification specification)
            : base(1, 3)
        {
            Specification = specification;
        }

        public VdcTypeSpecification Specification { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorVdcType(this, parameter);
        }
    }
}
