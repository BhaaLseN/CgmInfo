using System.Collections.Generic;
using CgmInfo.Commands.Enums;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("ASF")]
    public class AspectSourceFlags : Command
    {
        public AspectSourceFlags(IDictionary<AspectSourceFlagsType, AspectSourceFlagsValue> values)
            : base(5, 35)
        {
            Values = values;
        }

        public IDictionary<AspectSourceFlagsType, AspectSourceFlagsValue> Values { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeAspectSourceFlags(this, parameter);
        }
    }
}
