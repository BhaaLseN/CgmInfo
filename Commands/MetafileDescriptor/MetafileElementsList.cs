using System.Collections.Generic;
using CgmInfo.Traversal;

namespace CgmInfo.Commands.MetafileDescriptor
{
    [TextToken("MFELEMLIST")]
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

        public static MetafileElementsListElement Parse(string melString) => melString switch
        {
            "DRAWINGPLUS" => new MetafileElementsListElement(-1, 0),
            "DRAWINGSET" => new MetafileElementsListElement(-1, 1),
            "VERSION2" => new MetafileElementsListElement(-1, 2),
            "EXTDPRIM" => new MetafileElementsListElement(-1, 3),
            "VERSION2GKSM" => new MetafileElementsListElement(-1, 4),
            "VERSION3" => new MetafileElementsListElement(-1, 5),
            "VERSION4" => new MetafileElementsListElement(-1, 6),
            // TODO: we probably want to parse those into their actual class/id pairs at some point.
            var everythingElse => new MetafileElementsListElement(-1, -1, everythingElse),
        };
        public string GetTextEncodingString() => (ElementClass, ElementId) switch
        {
            (-1, 0) => "DRAWINGPLUS",
            (-1, 1) => "DRAWINGSET",
            (-1, 2) => "VERSION2",
            (-1, 3) => "EXTDPRIM",
            (-1, 4) => "VERSION2GKSM",
            (-1, 5) => "VERSION3",
            (-1, 6) => "VERSION4",
            _ => Name,
        };
    }
}
