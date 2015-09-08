using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class SavePrimitiveContext : Command
    {
        public SavePrimitiveContext(string contextName)
            : base(3, 11)
        {
            ContextName = contextName;
        }

        public string ContextName { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptControlSavePrimitiveContext(this, parameter);
        }
    }
}
