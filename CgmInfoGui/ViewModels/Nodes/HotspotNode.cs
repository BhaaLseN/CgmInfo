using System.ComponentModel;

namespace CgmInfoGui.ViewModels.Nodes;

public class HotspotNode : NodeBase
{
    public HotspotNode(string identifier)
    {
        ID = identifier;
        // TODO: maybe calculate the actual boundaries if the region type stays at 0?
        RegionValues = [];
    }

    public string ID { get; }
    public string? Name { get; set; }
    public string? Screentip { get; set; }
    [ReadOnly(true)]
    public HotspotRegionType RegionType { get; set; }
    public double[] RegionValues { get; set; }
    public HotspotLinkTarget? LinkTarget { get; set; }

    public override string DisplayName
    {
        get { return string.Format("{0} '{1}'", ID, string.IsNullOrEmpty(Name) ? "(no name)" : Name); }
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

public sealed class HotspotLinkTarget(string? destination, string? title, string? target)
{
    public string Destination { get; } = destination ?? string.Empty;
    public string? Title { get; } = title;
    public string? Target { get; } = target;

    public override string ToString()
    {
        if (!string.IsNullOrEmpty(Title))
            return string.Format("{0} ({1})", Title, Destination);
        else
            return Destination;
    }
}
