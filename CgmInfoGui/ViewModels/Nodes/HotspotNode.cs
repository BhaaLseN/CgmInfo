using System.ComponentModel;

namespace CgmInfoGui.ViewModels.Nodes
{
    public class HotspotNode : NodeBase
    {
        private readonly string _identifier;

        public HotspotNode(string identifier)
        {
            _identifier = identifier;
            // TODO: maybe calculate the actual boundaries if the region type stays at 0?
            RegionValues = new double[0];
        }

        public string ID
        {
            get { return _identifier; }
        }
        public string Name { get; set; }
        public string Screentip { get; set; }
        [ReadOnly(true)]
        public HotspotRegionType RegionType { get; set; }
        public double[] RegionValues { get; set; }
        public HotspotLinkTarget LinkTarget { get; set; }

        public override string DisplayName
        {
            get { return string.Format("{0} '{1}'", _identifier, string.IsNullOrEmpty(Name) ? "(no name)" : Name); }
        }
    }

    public enum HotspotRegionType
    {
        Object = 0,
        Rectangle = 1,
        Ellipse = 2,
        Polygon = 3,
        Polybezier = 4,
    }

    public sealed class HotspotLinkTarget
    {
        public HotspotLinkTarget(string destination, string title, string target)
        {
            Destination = destination;
            Title = title;
            Target = target;
        }
        public string Destination { get; }
        public string Title { get; }
        public string Target { get; }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Title))
                return string.Format("{0} ({1})", Title, Destination);
            else
                return Destination;
        }
    }
}
