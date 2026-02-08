using System.Collections.Generic;
using System.Collections.ObjectModel;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("EDGETYPE")]
    public class EdgeType : Command
    {
        public EdgeType(int index)
            : base(5, 27)
        {
            Index = index;
            Name = GetName(index);
        }

        public int Index { get; }
        public string Name { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeEdgeType(this, parameter);
        }

        public static IReadOnlyDictionary<int, string> KnownEdgeTypes { get; } = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
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
        public static string GetName(int index)
        {
            if (index < 0)
                return "Private";

            if (KnownEdgeTypes.TryGetValue(index, out string name))
                return name;

            return "Reserved";
        }
    }
}
