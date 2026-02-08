using System;
using System.Diagnostics.CodeAnalysis;
using CgmInfo.Traversal;

namespace CgmInfo.Commands
{
    public sealed class InvalidCommand : Command
    {
        // command 15/127 is considered reserved for extension in binary encoding [ISO/IEC 8632-3 5.4]
        // considering classes 10-15 are still unused/reserved, I doubt that particular extension is needed.
        // until then, we'll simply abuse it as marker for invalid Text Encoding commands.
        private const int ReservedElementClass_Text = 15;
        private const int ReservedElementId_Text = 127;

        public InvalidCommand(int elementClass, int elementId, Exception exception)
            : base(elementClass, elementId)
        {
            Exception = exception;
        }

        public InvalidCommand(string elementName, Exception exception)
            : base(ReservedElementClass_Text, ReservedElementId_Text)
        {
            ElementName = elementName;
            Exception = exception;
        }

        /// <summary>
        /// Returns whether this invalid command is based on the Binary Encoding
        /// (which uses <see cref="Command.ElementClass"/> and <see cref="Command.ElementId"/>
        /// to identify the given command) or the Text Encoding (which uses plain-text
        /// <see cref="ElementName"/>).
        /// </summary>
        [MemberNotNullWhen(true, nameof(ElementName))]
        public bool IsTextEncoding => ElementClass == ReservedElementClass_Text && ElementId == ReservedElementId_Text;
        /// <summary>Element Name in Text Encoding</summary>
        /// <remarks>Only set when <see cref="IsTextEncoding"/> is <c>true</c>.</remarks>
        public string? ElementName { get; }
        /// <summary>The <see cref="Exception"/> that occurred when trying to read this particular command.</summary>
        public Exception Exception { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptInvalidCommand(this, parameter);
        }
    }
}
