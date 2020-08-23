using System.Collections.Generic;
using System.Collections.ObjectModel;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    [TextToken("HATCHINDEX")]
    public class HatchIndex : Command
    {
        public HatchIndex(int index)
            : base(5, 24)
        {
            Index = index;
            Name = GetName(index);
        }

        public int Index { get; }
        public string Name { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeHatchIndex(this, parameter);
        }

        public static IReadOnlyDictionary<int, string> KnownHatchIndices { get; } = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // there is no value 0, guessing it should mean "None" if it ever happens
            { 0, "None" },
            // hatch indices originally part of ISO/IEC 8632:1999
            { 1, "Horizontal" },
            { 2, "Vertical" },
            { 3, "Positive Slope" },
            { 4, "Negative Slope" },
            { 5, "Horizontal/Vertical Crosshatch" },
            { 6, "Positive/Negative Slope Crosshatch" },
            // hatch indices later registered with the ISO/IEC 9973 Items Register
            { 7, "Cast Iron or Malleable Iron (and general use for all materials)" },
            { 8, "Steel" },
            { 9, "Bronze, Brass, Copper and Compositions" },
            { 10, "White Metal, Zinc, Lead, Babbit and Alloys" },
            { 11, "Magnesium, Aluminum and Aluminum Alloys" },
            { 12, "Rubber, Plastic and Electrical Insulation" },
            { 13, "Cork, Felt, Fabric, Leather and Fibre" },
            { 14, "Thermal Insulation" },
            { 15, "Titanium and refi-actory material" },
            { 16, "Marble, Slate, Porcelain, Glass, etc." },
            { 17, "Earth" },
            { 18, "Sand" },
            { 19, "Repeating Dot" },
        });
        public static string GetName(int index)
        {
            if (index < 0)
                return "Private";

            if (KnownHatchIndices.TryGetValue(index, out string name))
                return name;

            return "Reserved";
        }
    }
}
