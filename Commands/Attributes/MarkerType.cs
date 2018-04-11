using System.Collections.Generic;
using System.Collections.ObjectModel;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    public class MarkerType : Command
    {
        public MarkerType(int index)
            : base(5, 6)
        {
            Index = index;
            Name = GetName(index);
        }

        public int Index { get; private set; }
        public string Name { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeMarkerType(this, parameter);
        }

        private static readonly ReadOnlyDictionary<int, string> _knownMarkerTypes = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // there is no value 0, guessing it should mean "None" if it ever happens
            { 0, "None" },
            // marker types originally part of ISO/IEC 8632:1999
            { 1, "Dot" },
            { 2, "Plus" },
            { 3, "Asterisk" },
            { 4, "Circle" },
            { 5, "Cross" },
            // marker types later registered with the ISO/IEC 9973 Items Register
            { 6, "Meteorological Station Circle, one okta of cloud" },
            { 7, "Meteorological Station Circle, two oktas of cloud" },
            { 8, "Meteorological Station Circle, three oktas of cloud" },
            { 9, "Meteorological Station Circle, four oktas of cloud" },
            { 10, "Meteorological Station Circle, five oktas of cloud" },
            { 11, "Meteorological Station Circle, six oktas of cloud" },
            { 12, "Meteorological Station Circle, seven oktas of cloud" },
            { 13, "Meteorological Station Circle, eight oktas of cloud" },
            { 14, "Meteorological Station Circle, sky obscured" },
            { 15, "Meteorological Station Circle, sky not observed" },
            { 16, "Meteorological Automatic Station Circle, no cloud" },
            { 17, "Meteorological Automatic Station Circle, one okta of cloud" },
            { 18, "Meteorological Automatic Station Circle, two oktas of cloud" },
            { 19, "Meteorological Automatic Station Circle, three oktas of cloud" },
            { 20, "Meteorological Automatic Station Circle, four oktas of cloud" },
            { 21, "Meteorological Automatic Station Circle, five oktas of cloud" },
            { 22, "Meteorological Automatic Station Circle, six oktas of cloud" },
            { 23, "Meteorological Automatic Station Circle, seven oktas of cloud" },
            { 24, "Meteorological Automatic Station Circle, eight oktas of cloud" },
            { 25, "Meteorological Automatic Station Circle, sky obscured" },
            { 26, "Meteorological Automatic Station Circle, sky not observed" },
        });
        public static IReadOnlyDictionary<int, string> KnownMarkerTypes
        {
            get { return _knownMarkerTypes; }
        }
        public static string GetName(int index)
        {
            if (index < 0)
                return "Private";

            if (KnownMarkerTypes.TryGetValue(index, out string name))
                return name;

            return "Reserved";
        }
    }
}
