using System.Collections.Generic;
using System.Collections.ObjectModel;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Escape
{
    public class EscapeCommand : Command
    {
        public EscapeCommand(int identifier, StructuredDataRecord dataRecord)
            : base(6, 1)
        {
            Identifier = identifier;
            Name = GetName(identifier);
            DataRecord = dataRecord;
        }

        public int Identifier { get; private set; }
        public string Name { get; private set; }
        public StructuredDataRecord DataRecord { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptEscapeEscape(this, parameter);
        }

        public static IReadOnlyDictionary<int, string> KnownEscapeTypes { get; } = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // ISO/IEC 8632:1999 defines no escape elements; all values that follow are registered with the ISO/IEC 9973 Items Register
            { 1 , "Set Dash" },
            { 2 , "Set Line Cap" },
            { 3 , "Set Mitre Limit" },
            { 4 , "Set Line Join" },
            { 5 , "Set Conic Arc Transformation Matrix" },
            { 6 , "Set Edge Mitre Limit" },
            { 7 , "Set Edge Cap" },
            { 8 , "Set Edge Join" },
            { 9 , "Select Typeface Posture" },
            { 10, "Select Typeface Structure" },
            { 11, "Select Typeface Scores" },
            { 12, "Select Typeface Weight" },
            { 13, "Set Fully Justified Text" },
            { 14, "Select Typeface Proportionate Width" },
            { 15, "Segment List" },
            { 16, "Set Quick Update Erase Flag" },
            { 17, "Inquire Quick Update Erase Flag" },
            { 18, "Copy Element from Structure" },
            { 19, "Copy element range from structure" },
            { 20, "Move Structure Element" },
            { 21, "Move Element Range" },
            { 22, "Transparent Cell Colour" },
            { 23, "Set Watch on Element Range" },
            { 24, "End Watch on Element Range" },
            { 25, "Inquire Watch on Element Range" },
            { 26, "Map DC Points to Window System Coordinates" },
            { 27, "Map Window System Points to DC" },
            { 28, "Redraw Regions" },
            { 29, "Set Pick Mapping Data" },
            { 30, "Inquire Pick Mapping State" },
            { 31, "Inquire Pick Mapping Facilities" },
            { 32, "Map DC Point to Pick Paths" },
            { 33, "Map DC Points to WC" },
            { 34, "Inquire Window System Colour" },
            { 35, "Set Application Filter" },
            { 36, "Inquire Application Filter" },
            { 37, "Inquire Conditional Traversal Facilities" },
            { 38, "Inquire Highlighting Facilities" },
            { 39, "Inquire List of Highlighting Indices" },
            { 40, "Inquire Highlighting Representation" },
            { 41, "Inquire Predefined Highlighting Representation" },
            { 42, "Inquire Workstation Highlighting Table Length" },
            { 43, "Set Highlighting Representation" },
            { 45, "Alpha Transparency" },
            { 46, "Symbol Background Enable" },
            { 47, "Symbol Reference Point" },
            { 48, "Symbol Design Height and Width" },
        });
        public static string GetName(int index)
        {
            if (KnownEscapeTypes.TryGetValue(index, out string name))
                return name;

            return "Reserved";
        }
    }
}
