using System.Collections.Generic;
using System.Collections.ObjectModel;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("LINEJOIN")]
    public class LineJoin : Command
    {
        public LineJoin(int index)
            : base(5, 38)
        {
            Index = index;
            Name = GetName(index);
        }

        public int Index { get; private set; }
        public string Name { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeLineJoin(this, parameter);
        }

        public static IReadOnlyDictionary<int, string> KnownLineJoinIndicators { get; } = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // line join indicators originally part of ISO/IEC 8632:1999
            { 1, "Unspecified" },
            { 2, "Mitre" },
            { 3, "Round" },
            { 4, "Bevel" },
        });
        public static string GetName(int index)
        {
            if (KnownLineJoinIndicators.TryGetValue(index, out string name))
                return name;

            return "Reserved";
        }
    }
}
