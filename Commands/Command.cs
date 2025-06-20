using System.ComponentModel;
using CgmInfo.Traversal;
using CgmInfo.Utilities;

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

        /// <summary>The internal buffer data representing this command (as read from the input).</summary>
        [Browsable(false)]
        public TrackingBuffer? Buffer { get; internal set; }

        public abstract void Accept<T>(ICommandVisitor<T> visitor, T parameter);
    }
}
