using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.GraphicalPrimitives
{
    public class Polybezier : Command
    {
        public Polybezier(int continuityIndicator, PointF[] pointSequences)
            : base(4, 26)
        {
            ContinuityIndicator = continuityIndicator;
            Name = GetName(continuityIndicator);
            PointSequences = pointSequences;
        }

        public int ContinuityIndicator { get; private set; }
        public string Name { get; private set; }
        public PointF[] PointSequences { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptGraphicalPrimitivePolybezier(this, parameter);
        }

        private static readonly ReadOnlyDictionary<int, string> _knownContinuityIndicators = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // continuity indicators originally part of ISO/IEC 8632:1999
            { 1, "Discontinuous" },
            { 2, "Continuous" },
        });
        public static IReadOnlyDictionary<int, string> KnownContinuityIndicators
        {
            get { return _knownContinuityIndicators; }
        }
        public static string GetName(int index)
        {
            string name;
            if (KnownContinuityIndicators.TryGetValue(index, out name))
                return name;

            return "Reserved";
        }
    }
}
