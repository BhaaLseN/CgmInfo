using System;
using System.Linq;
using Avalonia;
using Avalonia.Media;

namespace CgmInfoGui.Visuals;

public class LineVisual : VisualBase
{
    public LineVisual(Point[] points, Func<Pen> pen)
        : this(points, pen, isClosed: false)
    {
    }
    public LineVisual(Point[] points, Func<Pen> pen, bool isClosed)
    {
        Points = points;
        Pen = pen;
        IsClosed = isClosed;
    }
    public Point[] Points { get; }
    public Func<Pen> Pen { get; }
    public bool IsClosed { get; }

    protected internal override void DrawTo(DrawingContext drawingContext, VisualContext visualContext)
    {
        var geo = new StreamGeometry();
        using (var ctx = geo.Open())
        {
            Point[] points = [.. Points.Select(visualContext.Correct)];
            if (IsClosed)
                points = [.. points, points[0]];
            ctx.BeginFigure(points[0], isFilled: false);
            ctx.PolyLineTo(points[1..], isStroked: true);
        }
        drawingContext.DrawGeometry(null, Pen(), geo);
    }
}
