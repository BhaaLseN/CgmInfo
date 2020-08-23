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

        public int ElementId { get; }
        public int ElementClass { get; }

        public abstract void Accept<T>(ICommandVisitor<T> visitor, T parameter);
    }
}
