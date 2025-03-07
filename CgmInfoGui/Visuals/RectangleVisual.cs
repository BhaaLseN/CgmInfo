using Avalonia;
using Avalonia.Media;

namespace CgmInfoGui.Visuals;

public class RectangleVisual : VisualBase
{
    public RectangleVisual(Point firstCorner, Point secondCorner)
    {
        FirstCorner = firstCorner;
        SecondCorner = secondCorner;
    }
    public Point FirstCorner { get; }
    public Point SecondCorner { get; }

    protected internal override void DrawTo(DrawingContext drawingContext, VisualContext visualContext)
    {
        // FIXME: use EDGE* and INTERIOR* here.
        drawingContext.DrawRectangle(null, GetBlack(), new Rect(visualContext.Correct(FirstCorner), visualContext.Correct(SecondCorner)));
    }
}
