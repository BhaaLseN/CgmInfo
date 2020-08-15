using System.Collections.Generic;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    public class MetafileElementsList : Command
    {
        public MetafileElementsList(IEnumerable<MetafileElementsListElement> elements)
            : base(1, 11)
        {
            Elements = elements;
        }

        public IEnumerable<MetafileElementsListElement> Elements { get; }

        public override void Accept<T>(ICommandVisitor<T> visitor, T parameter)
        {
            visitor.AcceptMetafileDescriptorMetafileElementsList(this, parameter);
        }
    }

    public class MetafileElementsListElement
    {
        public MetafileElementsListElement(int elementClass, int elementId)
        {
            ElementClass = elementClass;
            ElementId = elementId;
            Name = GetName(elementClass, elementId);
        }

        public MetafileElementsListElement(int elementClass, int elementId, string overrideName)
            : this(elementClass, elementId)
        {
            Name = overrideName;
        }

        public int ElementClass { get; }
        public int ElementId { get; }
        public string Name { get; }

        // returns a readable name for the given class/id.
        // NOTE: only handles pseudo classes at this point; the rest seems to be uncommon.
        public static string GetName(int elementClass, int elementId) => (elementClass, elementId) switch
        {
            // drawing set: (-1,0)
            (-1, 0) => "Drawing",
            // drawing-plus-control set: (-1,1)
            (-1, 1) => "Drawing+Control",
            // version-2 set: (-1,2)
            (-1, 2) => "Version 2",
            // extended-primitives set: (-1,3)
            (-1, 3) => "Extended Primitives",
            // version-2-gksm set: (-1,4)
            (-1, 4) => "Version 2 (GKSM)",
            // version-3 set: (-1,5)
            (-1, 5) => "Version 3",
            // version-4 set: (-1,6)
            (-1, 6) => "Version 4",
            _ => string.Format("Class {0}, Id {1}", elementClass, elementId),
        };
    }
}
