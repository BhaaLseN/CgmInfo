using System;
using Avalonia;
using Avalonia.Media;

namespace CgmInfoGui.Visuals;

public class CircularArcVisual : VisualBase
{
    public CircularArcVisual(Point center, Point start, Point end, double radius)
    {
        Center = center;
        Start = start;
        End = end;
        Radius = radius;
    }
    public Point Center { get; }
    public Point Start { get; }
    public Point End { get; }
    public double Radius { get; }

    protected internal override void DrawTo(DrawingContext drawingContext, VisualContext visualContext)
    {
        double startAngle = Math.Atan2(Start.Y, Start.X);
        double endAngle = Math.Atan2(End.Y, End.X);
        var startPoint = new Point(Center.X + (Math.Cos(startAngle) * Radius), Center.Y + (Math.Sin(startAngle) * Radius));
        var endPoint = new Point(Center.X + (Math.Cos(endAngle) * Radius), Center.Y + (Math.Sin(endAngle) * Radius));
        double rotationAngle = RadiansToDegrees(endAngle - startAngle);
        // TODO: is this correct, or are we going the wrong way afterwards?
        if (rotationAngle < 0)
            rotationAngle = Math.Abs(rotationAngle);

        var geo = new StreamGeometry();
        using (var ctx = geo.Open())
        {
            ctx.BeginFigure(visualContext.Correct(startPoint), isFilled: false);
            ctx.ArcTo(visualContext.Correct(endPoint), new Size(Radius, Radius), rotationAngle, isLargeArc: rotationAngle > 180, SweepDirection.Clockwise, isStroked: true);
        }
        // FIXME: use EDGE* and INTERIOR* here.
        drawingContext.DrawGeometry(null, GetBlack(), geo);
    }
}
