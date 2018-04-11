using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using CgmInfo.Traversal;
using CgmInfo.Utilities;

namespace CgmInfo.Commands.Attributes
{
    public class InterpolatedInterior : Command
    {
        public InterpolatedInterior(int index, PointF[] referenceGeometry, double[] stageDesignators, MetafileColor[] colorSpecifiers)
            : base(5, 43)
        {
            Index = index;
            Name = GetName(index);
            ReferenceGeometry = referenceGeometry;
            StageDesignators = stageDesignators;
            ColorSpecifiers = colorSpecifiers;
        }

        public int Index { get; private set; }
        public string Name { get; private set; }
        public PointF[] ReferenceGeometry { get; private set; }
        public double[] StageDesignators { get; private set; }
        public MetafileColor[] ColorSpecifiers { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeInterpolatedInterior(this, parameter);
        }

        private static readonly ReadOnlyDictionary<int, string> _knownInteriorStyles = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // interior styles originally part of ISO/IEC 8632:1999
            { 1, "Parallel" },
            { 2, "Elliptical" },
            { 3, "Triangular" },
        });
        public static IReadOnlyDictionary<int, string> KnownInteriorStyles
        {
            get { return _knownInteriorStyles; }
        }
        public static string GetName(int index)
        {
            if (KnownInteriorStyles.TryGetValue(index, out string name))
                return name;

            return "Reserved";
        }
    }
}
