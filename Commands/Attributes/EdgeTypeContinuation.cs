using System.Collections.Generic;
using System.Collections.ObjectModel;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("EDGETYPECONT")]
    public class EdgeTypeContinuation : Command
    {
        public EdgeTypeContinuation(int index)
            : base(5, 46)
        {
            Index = index;
            Name = GetName(index);
        }

        public int Index { get; }
        public string Name { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeEdgeTypeContinuation(this, parameter);
        }

        public static IReadOnlyDictionary<int, string> KnownEdgeTypeContinuations { get; } = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // edge type continuations originally part of ISO/IEC 8632:1999
            { 1, "Unspecified" },
            { 2, "Continue" },
            { 3, "Restart" },
            { 4, "Adaptive Continue" },
        });
        public static string GetName(int index)
        {
            if (KnownEdgeTypeContinuations.TryGetValue(index, out string name))
                return name;

            return "Reserved";
        }
    }
}
