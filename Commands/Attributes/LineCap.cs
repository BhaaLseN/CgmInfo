using System.Collections.Generic;
using System.Collections.ObjectModel;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
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

        public int LineCapIndicator { get; private set; }
        public int DashCapIndicator { get; private set; }
        public string LineCapName { get; private set; }
        public string DashCapName { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeLineCap(this, parameter);
        }

        private static readonly ReadOnlyDictionary<int, string> _knownLineCapIndicators = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // line cap indicators originally part of ISO/IEC 8632:1999
            { 1, "Unspecified" },
            { 2, "Butt" },
            { 3, "Round" },
            { 4, "Projecting Square" },
            { 5, "Triangle" },
        });
        public static IReadOnlyDictionary<int, string> KnownLineCapIndicators
        {
            get { return _knownLineCapIndicators; }
        }
        public static string GetLineCapName(int index)
        {
            string name;
            if (KnownLineCapIndicators.TryGetValue(index, out name))
                return name;

            return "Reserved";
        }

        private static readonly ReadOnlyDictionary<int, string> _knownDashCapIndicators = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // dash cap indicators originally part of ISO/IEC 8632:1999
            { 1, "Unspecified" },
            { 2, "Butt" },
            { 3, "Match" },
        });
        public static IReadOnlyDictionary<int, string> KnownDashCapIndicators
        {
            get { return _knownDashCapIndicators; }
        }
        public static string GetDashCapName(int index)
        {
            string name;
            if (KnownDashCapIndicators.TryGetValue(index, out name))
                return name;

            return "Reserved";
        }
    }
}
