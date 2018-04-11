using System.Collections.Generic;
using System.Collections.ObjectModel;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    public class LineType : Command
    {
        public LineType(int index)
            : base(5, 2)
        {
            Index = index;
            Name = GetName(index);
        }

        public int Index { get; private set; }
        public string Name { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeLineType(this, parameter);
        }

        public static IReadOnlyDictionary<int, string> KnownLineTypes { get; } = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // there is no value 0, guessing it should mean "None" if it ever happens
            { 0, "None" },
            // line types originally part of ISO/IEC 8632:1999
            { 1, "Solid" },
            { 2, "Dash" },
            { 3, "Dot" },
            { 4, "Dash-Dot" },
            { 5, "Dash-Dot-Dot" },
            // line types later registered with the ISO/IEC 9973 Items Register
            { 6, "Single Arrow" },
            { 7, "Single Dot" },
            { 8, "Double Arrow" },
            { 9, "Stitch Line" },
            { 10, "Chain Line" },
            { 11, "Center Line" },
            { 12, "Hidden Line" },
            { 13, "Phantom Line" },
            { 14, "Break Line - Style 1 (Freehand)" },
            { 15, "Break Line - Style 2 (Zig-Zag)" },
            // special type that uses the "Set Dash" ESCAPE instruction to set a custom pattern (ISO/IEC 9973)
            { 16, "User-specified Dash Pattern" },
        });
        public static string GetName(int index)
        {
            if (index < 0)
                return "Private";

            if (KnownLineTypes.TryGetValue(index, out string name))
                return name;

            return "Reserved";
        }
    }
}
