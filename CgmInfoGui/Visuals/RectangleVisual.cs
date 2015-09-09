using System;
using Avalonia;
using Avalonia.Media;

namespace CgmInfoGui.Visuals;

public class RectangleVisual : VisualBase
{
    public RectangleVisual(Point firstCorner, Point secondCorner)
    {
        FirstCorner = new Point(Math.Min(firstCorner.X, secondCorner.X), Math.Min(firstCorner.Y, secondCorner.Y));
        SecondCorner = new Point(Math.Max(firstCorner.X, secondCorner.X), Math.Max(firstCorner.Y, secondCorner.Y));
    }
    public Point FirstCorner { get; }
    public Point SecondCorner { get; }

    protected internal override void DrawTo(DrawingContext drawingContext)
    {
        // FIXME: use EDGE* and INTERIOR* here.
        drawingContext.DrawRectangle(null, GetBlack(), new Rect(FirstCorner, SecondCorner));
    }
}
