using System;
using System.Collections.Generic;
using System.Linq;
using CgmInfo.Commands;
using CgmInfo.Commands.ApplicationStructureDescriptor;
using CgmInfo.Commands.Delimiter;
using CgmInfoGui.ViewModels.Nodes;

namespace CgmInfoGui.Traversal
{
    public class HotspotContext : NodeContext
    {
        // keep track of APS elements; not every element is a hotspot
        private readonly Stack<BeginApplicationStructure> _apsStack = new Stack<BeginApplicationStructure>();
        private HotspotNode _currentNode;
        private bool _shouldKeepHotspot;

        public void BeginAPS(BeginApplicationStructure beginAps)
        {
            _apsStack.Push(beginAps);
        }
        public BeginApplicationStructure EndAPS()
        {
            if (_apsStack.Count == 0)
                return null;
            return _apsStack.Pop();
        }

        public HotspotNode BeginHotspot(string identifier)
        {
            var hotspotNode = new HotspotNode(identifier);
            BeginLevel(hotspotNode);
            // remember this node until the APS Body begins (which is when attributes are over)
            _currentNode = hotspotNode;
            // even though this is a hotspot element; say we don't want to keep it
            // adding attributes to it will make it viable
            _shouldKeepHotspot = false;
            return hotspotNode;
        }
        public void SealHotspot()
        {
            if (_currentNode != null && !_shouldKeepHotspot)
            {
                // discard a started hotspot when no viable attributes have been seen until this point
                EndHotspot();
                CurrentLevelNodes.Remove(_currentNode);
                // replace the current APS stack value; otherwise EndHotspot will be called twice
                var currentAps = EndAPS();
                BeginAPS(new BeginApplicationStructure(currentAps.Identifier, "NotAHotspot", currentAps.Inheritance));
            }
            // reset the node; should be called when the APS Body begins (ie. no more attributes for this node follow)
            _currentNode = null;
        }
        public void EndHotspot()
        {
            EndLevel();
        }

        public void AddHotspotAttribute(ApplicationStructureAttribute applicationStructureAttribute)
        {
            if (_currentNode == null || applicationStructureAttribute == null)
                return;

            bool hotspotAttribute = false;
            switch (applicationStructureAttribute.AttributeType.ToUpperInvariant())
            {
                case "REGION": // [WebCGM20-IC 3.2.2.1]
                    UpdateHotspotRegion(applicationStructureAttribute.DataRecord);
                    // Objects may contain an explicit 'region' APS Attribute, which provides the boundary for picking operations. [WebCGM20-Concepts 2.2.3]
                    hotspotAttribute = true;
                    break;
                case "LINKURI": // [WebCGM20-IC 3.2.2.3]
                    UpdateLinkTarget(applicationStructureAttribute.DataRecord);
                    // A graphical hotspot is a graphical object that participates in a link. [S1000D Iss2.3, Chap 7.3.2, Para 4.6]
                    hotspotAttribute = true;
                    break;
                case "SCREENTIP": // [WebCGM20-IC 3.2.2.6]
                    UpdateScreentip(applicationStructureAttribute.DataRecord);
                    break;
                case "NAME": // [WebCGM20-IC 3.2.2.7]
                    UpdateName(applicationStructureAttribute.DataRecord);
                    break;
            }

            // toggle viability to true, but never switch back to false for the same hotspot
            _shouldKeepHotspot |= hotspotAttribute;
        }

        private void UpdateName(StructuredDataRecord dataRecord)
        {
            var firstElement = dataRecord.Elements.FirstOrDefault();
            object nameObject = firstElement != null ? firstElement.Values.FirstOrDefault() : null;
            if (nameObject != null)
            {
                _currentNode.Name = Convert.ToString(nameObject);
            }
        }
        private void UpdateScreentip(StructuredDataRecord dataRecord)
        {
            var firstElement = dataRecord.Elements.FirstOrDefault();
            object screentipObject = firstElement != null ? firstElement.Values.FirstOrDefault() : null;
            if (screentipObject != null)
            {
                _currentNode.Screentip = Convert.ToString(screentipObject);
            }
        }
        private void UpdateLinkTarget(StructuredDataRecord dataRecord)
        {
            var firstElement = dataRecord.Elements.FirstOrDefault();
            object[] linkValues = firstElement != null ? firstElement.Values : new object[0];
            if (linkValues.Length == 3)
            {
                _currentNode.LinkTarget = new HotspotLinkTarget(Convert.ToString(linkValues[0]), Convert.ToString(linkValues[1]), Convert.ToString(linkValues[2]));
            }
        }
        private void UpdateHotspotRegion(StructuredDataRecord dataRecord)
        {
            // region has 2 SDR elements; the region type and the actual values according to the type
            // just say its an object boundary hotspot if we can't find anything better
            if (dataRecord.Elements.Count() != 2)
                return;

            var firstElement = dataRecord.Elements.First();
            object regionTypeObject = firstElement != null ? firstElement.Values.FirstOrDefault() : null;
            if (regionTypeObject != null && Enum.TryParse<HotspotRegionType>(Convert.ToString(regionTypeObject), out var regionType))
            {
                _currentNode.RegionType = regionType;
                _currentNode.RegionValues = dataRecord.Elements.Skip(1).First().Values.OfType<double>().ToArray();
            }
        }
    }
}
