using System.Linq;
using CgmInfo.Commands;

namespace CgmInfoGui.ViewModels.Nodes
{
    public class UnsupportedCounter : UnsupportedContainer
    {
        private bool _sorted;

        public void Count(UnsupportedCommand unsupportedCommand)
        {
            string elementName;
            if (unsupportedCommand.IsTextEncoding)
                elementName = unsupportedCommand.ElementName;
            else
                elementName = string.Format("Element Class={0}, Id={1}", unsupportedCommand.ElementClass, unsupportedCommand.ElementId);
            var matchingElement = Nodes.OfType<UnsupportedCountNode>().FirstOrDefault(u => u.ElementName == elementName);
            if (matchingElement == null)
            {
                matchingElement = new UnsupportedCountNode(elementName);
                Nodes.Add(matchingElement);
            }
            matchingElement.Count++;
        }
        public override string DisplayName
        {
            get
            {
                // one-time sort by occurrence count
                if (!_sorted)
                {
                    Nodes.Sort((n1, n2) => ((UnsupportedCountNode)n2).Count.CompareTo(((UnsupportedCountNode)n1).Count));
                    _sorted = true;
                }
                return base.DisplayName;
            }
        }

        private class UnsupportedCountNode : NodeBase
        {
            public UnsupportedCountNode(string elementName)
            {
                ElementName = elementName;
            }

            public string ElementName { get; }
            public int Count { get; set; }

            public override string DisplayName
            {
                get { return string.Format("{0}: {1}", ElementName, Count); }
            }
        }
    }
}
