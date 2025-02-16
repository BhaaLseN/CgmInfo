using System;
using System.Linq;
using Avalonia;
using Avalonia.Media;

namespace CgmInfoGui.Visuals;

public class LineVisual : VisualBase
{
    public LineVisual(Point[] points, Func<Pen> pen)
    {
        Points = points;
        Pen = pen;
    }
    public Point[] Points { get; }
    public Func<Pen> Pen { get; }

    protected internal override void DrawTo(DrawingContext drawingContext)
    {
        var geo = new StreamGeometry();
        using (var ctx = geo.Open())
        {
            ctx.BeginFigure(Points[0], isFilled: false);
            ctx.PolyLineTo(Points[1..], isStroked: true);
        }
        drawingContext.DrawGeometry(null, Pen(), geo);
    }
}
