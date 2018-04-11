using System.Collections.Generic;
using System.Collections.ObjectModel;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    public class EdgeCap : Command
    {
        public EdgeCap(int edgeCapIndicator, int dashCapIndicator)
            : base(5, 44)
        {
            EdgeCapIndicator = edgeCapIndicator;
            DashCapIndicator = dashCapIndicator;
            EdgeCapName = GetLineCapName(edgeCapIndicator);
            DashCapName = GetDashCapName(dashCapIndicator);
        }

        public int EdgeCapIndicator { get; private set; }
        public int DashCapIndicator { get; private set; }
        public string EdgeCapName { get; private set; }
        public string DashCapName { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeEdgeCap(this, parameter);
        }

        private static readonly ReadOnlyDictionary<int, string> _knownEdgeCapIndicators = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // edge cap indicators originally part of ISO/IEC 8632:1999
            { 1, "Unspecified" },
            { 2, "Butt" },
            { 3, "Round" },
            { 4, "Projected Square" },
            { 5, "Triangle" },
        });
        public static IReadOnlyDictionary<int, string> KnownEdgeCapIndicators
        {
            get { return _knownEdgeCapIndicators; }
        }
        public static string GetLineCapName(int index)
        {
            if (KnownEdgeCapIndicators.TryGetValue(index, out string name))
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
            if (KnownDashCapIndicators.TryGetValue(index, out string name))
                return name;

            return "Reserved";
        }
    }
}
