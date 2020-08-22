using System.Collections.Generic;
using System.Collections.ObjectModel;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("LINECAP")]
    public class LineCap : Command
    {
        public LineCap(int lineCapIndicator, int dashCapIndicator)
            : base(5, 37)
        {
            LineCapIndicator = lineCapIndicator;
            DashCapIndicator = dashCapIndicator;
            LineCapName = GetLineCapName(lineCapIndicator);
            DashCapName = GetDashCapName(dashCapIndicator);
        }

        public int LineCapIndicator { get; }
        public int DashCapIndicator { get; }
        public string LineCapName { get; }
        public string DashCapName { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeLineCap(this, parameter);
        }

        public static IReadOnlyDictionary<int, string> KnownLineCapIndicators { get; } = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // line cap indicators originally part of ISO/IEC 8632:1999
            { 1, "Unspecified" },
            { 2, "Butt" },
            { 3, "Round" },
            { 4, "Projecting Square" },
            { 5, "Triangle" },
        });
        public static string GetLineCapName(int index)
        {
            if (KnownLineCapIndicators.TryGetValue(index, out string name))
                return name;

            return "Reserved";
        }

        public static IReadOnlyDictionary<int, string> KnownDashCapIndicators { get; } = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // dash cap indicators originally part of ISO/IEC 8632:1999
            { 1, "Unspecified" },
            { 2, "Butt" },
            { 3, "Match" },
        });
        public static string GetDashCapName(int index)
        {
            if (KnownDashCapIndicators.TryGetValue(index, out string name))
                return name;

            return "Reserved";
        }
    }
}
