using System.Collections.Generic;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class MetafileElementsList : Command
    {
        public MetafileElementsList(IEnumerable<string> elements)
            : base(1, 11)
        {
            Elements = elements;
        }

        public IEnumerable<string> Elements { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorMetafileElementsList(this, parameter);
        }
    }
}
