using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class RestorePrimitiveContext : Command
    {
        public RestorePrimitiveContext(string contextName)
            : base(3, 12)
        {
            ContextName = contextName;
        }

        public string ContextName { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptControlRestorePrimitiveContext(this, parameter);
        }
    }
}
