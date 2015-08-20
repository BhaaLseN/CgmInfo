using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using CgmInfo.Commands.ApplicationStructureDescriptor;
using CgmInfo.Commands.Delimiter;

namespace CgmInfoGui.Traversal
{
    public class XCFDocumentContext
    {
        private XElement _lastElement;
        private static readonly XNamespace xcf = "http://www.cgmopen.org/schema/webcgm/";
        public XCFDocumentContext()
        {
            // webcgm XCF skeleton, which is the bare minimum thats usually there [WebCGM20-XCF 4.3.3]
            // http://www.w3.org/TR/webcgm20/WebCGM20-XCF.html
            XCF = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XDocumentType("webcgm", "-//OASIS//DTD WebCGM 2.0//EN", "http://docs.oasis-open.org/webcgm/v2.0/webcgm20.dtd", null),
                new XElement(xcf + "webcgm",
                    new XAttribute("xmlns", xcf.NamespaceName),
                    new XAttribute("version", "2.0")));
        }
        public XDocument XCF { get; private set; }

        public void AddAPSElement(BeginApplicationStructure beginApplicationStructure)
        {
            switch (beginApplicationStructure.Type.ToUpperInvariant())
            {
                case "GROBJECT": // [WebCGM20-XCF 4.3.5]
                case "LAYER": // [WebCGM20-XCF 4.3.4]
                case "PARA": // [WebCGM20-XCF 4.3.6]
                case "SUBPARA": // [WebCGM20-XCF 4.3.7]
                    var apsElement = new XElement(xcf + beginApplicationStructure.Type.ToLower(),
                        new XAttribute("apsid", beginApplicationStructure.Identifier));
                    XCF.Root.Add(apsElement);
                    _lastElement = apsElement;
                    break;
            }
        }
        public void AddAPSAttribute(ApplicationStructureAttribute applicationStructureAttribute)
        {
            if (_lastElement != null)
            {
                string[] supportedAttributes = GetSupportedAttributes(_lastElement.Name.LocalName);
                if (supportedAttributes.Contains(applicationStructureAttribute.AttributeType.ToLower()))
                {
                    string attributeValue = SerializeAttributeValue(applicationStructureAttribute);
                    _lastElement.Add(new XAttribute(applicationStructureAttribute.AttributeType.ToLower(), attributeValue));
                }
                else if (string.Equals(applicationStructureAttribute.AttributeType, "linkuri", StringComparison.InvariantCultureIgnoreCase))
                {
                    // [WebCGM20-XCF 4.3.8]
                    var allValues = applicationStructureAttribute.DataRecord.Elements.SelectMany(el => el.Values).ToArray();
                    if (allValues.Length == 3)
                    {
                        _lastElement.Add(new XElement(xcf + "linkuri",
                            new XAttribute("uri", allValues[0] ?? ""),
                            string.IsNullOrEmpty(allValues[1] as string) ? null : new XAttribute("desc", allValues[1]),
                            string.IsNullOrEmpty(allValues[2] as string) ? null : new XAttribute("behavior", allValues[2])));
                    }
                    else
                    {
                        _lastElement.Add(new XElement(xcf + "linkuri",
                            new XAttribute("uri", allValues.FirstOrDefault() ?? "")),
                            new XComment("Invalid linkuri attribute content"));
                    }
                }
            }
        }
        private string SerializeAttributeValue(ApplicationStructureAttribute applicationStructureAttribute)
        {
            var allValues = applicationStructureAttribute.DataRecord.Elements.SelectMany(el => el.Values).ToArray();
            switch (applicationStructureAttribute.AttributeType.ToUpperInvariant())
            {
                case "SCREENTIP":
                case "LAYERDESC":
                case "VISIBILITY":
                case "INTERACTIVITY":
                    if (allValues.Length == 1)
                        return Convert.ToString(allValues[0]);
                    else
                        return "#INVALID";
                case "REGION":
                case "VIEWCONTEXT":
                    return string.Join(" ", allValues.Select(v => Convert.ToString(v, CultureInfo.GetCultureInfo("en"))));
                default:
                    return "#UNSUPPORTED";
            }
        }
        private string[] GetSupportedAttributes(string apsName)
        {
            // NOTE: apsid is derived from the actual APS beginning; not from an APS attribute (hence omitted here)
            switch (apsName.ToUpperInvariant())
            {
                case "LAYER": // [WebCGM20-XCF 4.3.4]
                    return new[] { "layerdesc", "visibility", "interactivity" };
                case "GROBJECT": // [WebCGM20-XCF 4.3.5]
                case "PARA": // [WebCGM20-XCF 4.3.6]
                case "SUBPARA": // [WebCGM20-XCF 4.3.7]
                    return new[] { "screentip", "region", "viewcontext", "visibility", "interactivity" };
                default:
                    return new string[0];
            }
        }
    }
}
