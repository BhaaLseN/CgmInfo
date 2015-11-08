using System.Collections.Generic;
using System.Collections.ObjectModel;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    public class EdgeType : Command
    {
        public EdgeType(int index)
            : base(5, 27)
        {
            Index = index;
            Name = GetName(index);
        }

        public int Index { get; private set; }
        public string Name { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeEdgeType(this, parameter);
        }

        private static readonly ReadOnlyDictionary<int, string> _knownEdgeTypes = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // there is no value 0, guessing it should mean "None" if it ever happens
            { 0, "None" },
            // edge types originally part of ISO/IEC 8632:1999
            { 1, "Solid" },
            { 2, "Dash" },
            { 3, "Dot" },
            { 4, "Dash-Dot" },
            { 5, "Dash-Dot-Dot" },
        });
        public static IReadOnlyDictionary<int, string> KnownEdgeTypes
        {
            get { return _knownEdgeTypes; }
        }
        public static string GetName(int index)
        {
            if (index < 0)
                return "Private";

            string name;
            if (KnownEdgeTypes.TryGetValue(index, out name))
                return name;

            return "Reserved";
        }
    }
}
