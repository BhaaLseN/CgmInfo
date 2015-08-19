using System;
using System.Linq;
using CgmInfo.Commands.ApplicationStructureDescriptor;
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Traversal
{
    public class APSStructureContext : NodeContext
    {
        public void AddAttributeNode(ApplicationStructureAttribute applicationStructureAttribute)
        {
            var allValues = applicationStructureAttribute.DataRecord.Elements.SelectMany(el => el.Values).ToArray();
            switch (applicationStructureAttribute.AttributeType.ToUpperInvariant())
            {
                #region WebCGM 2.0 Intelligent Content [WebCGM20-IC]
                // http://www.w3.org/TR/webcgm20/WebCGM20-IC.html

                case "CONTENT": // [WebCGM20-IC 3.2.2.8]
                case "INTERACTIVITY": // [WebCGM20-IC 3.2.2.10]
                case "LAYERNAME": // [WebCGM20-IC 3.2.2.4]
                case "LAYERDESC": // [WebCGM20-IC 3.2.2.5]
                case "NAME": // [WebCGM20-IC 3.2.2.7]
                case "SCREENTIP": // [WebCGM20-IC 3.2.2.6]
                case "VISIBILITY": // [WebCGM20-IC 3.2.2.9]
                    // simple attribute with one value; show just the value (unless it doesn't have one value for whatever reason)
                    if (allValues.Length != 1)
                        goto default;

                    AddNode("@{0} = '{1}'", applicationStructureAttribute.AttributeType, allValues[0]);
                    break;

                case "LINKURI": // [WebCGM20-IC 3.2.2.3]
                    // linkuri has exactly 3 attributes; if not, show the default stuff
                    if (allValues.Length != 3)
                        goto default;

                    // The data record is an SDR of one member, containing three strings (type SF, String Fixed).
                    // The first string is the link destination, a IRI,
                    // the second string (possibly null) is a Link Title parameter,
                    // and the third string (possibly null) is the Behavior parameter.
                    // Note that a null string is a zero-length string, and is not the same as an omitted parameter. The parameter must not be omitted.
                    AddNode("@{0} <a href=\"{1}\" target=\"{3}\">{2}</a>",
                        applicationStructureAttribute.AttributeType,
                        allValues[0], allValues[1], allValues[2]);
                    break;

                #endregion

                default:
                    // in case theres just a single value; show it directly.
                    if (allValues.Length == 1)
                    {
                        AddNode("@{0} = '{1}'", applicationStructureAttribute.AttributeType, allValues[0]);
                    }
                    else
                    {
                        var attributeNode = AddNode("@{0} [{1} values]", applicationStructureAttribute.AttributeType, allValues.Length);
                        attributeNode.Nodes.AddRange(allValues.Select(value => new SimpleNode(Convert.ToString(value))));
                    }
                    break;
            }
        }
    }
}
