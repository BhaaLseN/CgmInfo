using Avalonia;
using CgmInfoGui.Visuals;

namespace CgmInfoGui.Traversal;

public class GraphicalElementContext
{
    public TextVisual? LastText { get; set; }
    public VisualRoot Visuals { get; } = new();

    public void Add(VisualBase visual)
    {
        Visuals.Add(visual);
    }
    public void IncreaseBounds(Point point)
    {
        IncreaseBounds(new Rect(point, new Size(1, 1)));
    }
    public void IncreaseBounds(Rect rect)
    {
        if (Visuals.GeometryExtent is { Width: 0, Height: 0 })
        {
            Visuals.GeometryExtent = rect;
        }
        else
        {
            Visuals.GeometryExtent = Visuals.GeometryExtent.Union(rect);
        }
    }
}
