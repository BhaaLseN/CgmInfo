using Avalonia;
using Avalonia.Media;

namespace CgmInfoGui.Visuals;

public class CircleVisual : VisualBase
{
    public CircleVisual(Point center, double radius)
    {
        Center = center;
        Radius = radius;
    }
    public Point Center { get; }
    public double Radius { get; }

    protected internal override void DrawTo(DrawingContext drawingContext)
    {
        // FIXME: use EDGE* and INTERIOR* here.
        drawingContext.DrawEllipse(null, GetBlack(), Center, Radius, Radius);
    }
}
