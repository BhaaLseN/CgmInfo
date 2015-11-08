using System.Collections.Generic;
using System.Collections.ObjectModel;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    public class EdgeTypeContinuation : Command
    {
        public EdgeTypeContinuation(int index)
            : base(5, 46)
        {
            Index = index;
            Name = GetName(index);
        }

        public int Index { get; private set; }
        public string Name { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeEdgeTypeContinuation(this, parameter);
        }

        private static readonly ReadOnlyDictionary<int, string> _knownEdgeTypeContinuations = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // edge type continuations originally part of ISO/IEC 8632:1999
            { 1, "Unspecified" },
            { 2, "Continue" },
            { 3, "Restart" },
            { 4, "Adaptive Continue" },
        });
        public static IReadOnlyDictionary<int, string> KnownEdgeTypeContinuations
        {
            get { return _knownEdgeTypeContinuations; }
        }
        public static string GetName(int index)
        {
            string name;
            if (KnownEdgeTypeContinuations.TryGetValue(index, out name))
                return name;

            return "Reserved";
        }
    }
}
