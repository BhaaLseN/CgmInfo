using System.Xml.Linq;
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
    }
}
