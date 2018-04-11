using System.Collections.Generic;
using System.Collections.ObjectModel;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.Attributes
{
    public class RestrictedTextType : Command
    {
        public RestrictedTextType(int index)
            : base(5, 42)
        {
            Index = index;
            Name = GetName(index);
        }

        public int Index { get; private set; }
        public string Name { get; private set; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptAttributeRestrictedTextType(this, parameter);
        }

        private static readonly ReadOnlyDictionary<int, string> _knownRestrictionTypes = new ReadOnlyDictionary<int, string>(new Dictionary<int, string>
        {
            // restriction types originally part of ISO/IEC 8632:1999
            { 1, "Basic" },
            { 2, "Boxed-Cap" },
            { 3, "Boxed-All" },
            { 4, "Isotropic-Cap" },
            { 5, "Isotropic-All" },
            { 6, "Justified" },
        });
        public static IReadOnlyDictionary<int, string> KnownRestrictionTypes
        {
            get { return _knownRestrictionTypes; }
        }
        public static string GetName(int index)
        {
            if (KnownRestrictionTypes.TryGetValue(index, out string name))
                return name;

            return "Reserved";
        }
    }
}
