using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("SAVEPRIMCONT")]
    public class SavePrimitiveContext : Command
    {
        public SavePrimitiveContext(int contextName)
            : base(3, 11)
        {
            ContextName = contextName;
        }

        public int ContextName { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptControlSavePrimitiveContext(this, parameter);
        }
    }
}
