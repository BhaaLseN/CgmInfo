using System.Collections.Generic;
using System.Collections.ObjectModel;
using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    [TextToken("POLYBEZIER")]
    public class Polybezier : Command
    {
        public Polybezier(int continuityIndicator, MetafilePoint[] pointSequences)
            : base(4, 26)
        {
            ContinuityIndicator = continuityIndicator;
            Name = GetName(continuityIndicator);
            PointSequences = pointSequences;
        }

        public int ContinuityIndicator { get; }
        public string Name { get; }
        public MetafilePoint[] PointSequences { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitivePolybezier(this, parameter);
        }

        public static IReadOnlyDictionary<int, string> KnownContinuityIndicators { get; } = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // continuity indicators originally part of ISO/IEC 8632:1999
            { 1, "Discontinuous" },
            { 2, "Continuous" },
        });
        public static string GetName(int index)
        {
            if (KnownContinuityIndicators.TryGetValue(index, out string name))
                return name;

            return "Reserved";
        }
    }
}
