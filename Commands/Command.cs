using CgmInfo.Traversal;

namespace CgmInfo.Commands
{
#if DEBUG
    [System.Diagnostics.DebuggerDisplay("{ElementClass}/{ElementId}")]
#endif
    public abstract class Command
    {
        protected Command(int elementClass, int elementId)
        {
            ElementClass = elementClass;
            ElementId = elementId;
        }

        public int ElementId { get; private set; }
        public int ElementClass { get; private set; }

        public abstract void Accept<T>(ICommandVisitor<T> visitor, T parameter);
    }
}
