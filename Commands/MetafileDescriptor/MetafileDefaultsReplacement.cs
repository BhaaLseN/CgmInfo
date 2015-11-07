using System.Collections.Generic;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class MetafileDefaultsReplacement : Command
    {
        public MetafileDefaultsReplacement(IEnumerable<Command> commands)
            : base(1, 12)
        {
            Commands = commands;
        }

        public IEnumerable<Command> Commands { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorMetafileDefaultsReplacement(this, parameter);
        }
    }
}
