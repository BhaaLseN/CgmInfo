using System.Collections.Generic;
using System.Collections.ObjectModel;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class FontProperties : Command
    {
        public FontProperties(FontProperty[] properties)
            : base(1, 21)
        {
            Properties = properties;
        }

        public FontProperty[] Properties { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorFontProperties(this, parameter);
        }
    }

    public class FontProperty
    {
        public FontProperty(int indicator, int priority, StructuredDataElement record)
        {
            Indicator = indicator;
            Name = GetName(indicator);
            Priority = priority;
            Record = record;
        }
        public int Indicator { get; private set; }
        public string Name { get; private set; }
        public int Priority { get; private set; }
        public StructuredDataElement Record { get; private set; }

        private static readonly ReadOnlyDictionary<int, string> _knownPropertyIndicators = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // property indicators originally part of ISO/IEC 8632:1999
            { 1, "Font Index" },
            { 2, "Standard Version" },
            { 3, "Design Source" },
            { 4, "Font Family" },
            { 5, "Posture" },
            { 6, "Weight" },
            { 7, "Proportionate Width" },
            { 8, "Included Glyph Connections" },
            { 9, "Included Glyphs" },
            { 10, "Design Size" },
            { 11, "Minimum Size" },
            { 12, "Maximum Size" },
            { 13, "Design Group" },
            { 14, "Structure" },
        });
        public static IReadOnlyDictionary<int, string> KnownPropertyIndicators
        {
            get { return _knownPropertyIndicators; }
        }
        public static string GetName(int index)
        {
            if (KnownPropertyIndicators.TryGetValue(index, out string name))
                return name;

            return "Reserved";
        }
    }
}
