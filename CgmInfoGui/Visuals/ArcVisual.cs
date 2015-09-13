using System;
using Avalonia;
using Avalonia.Media;

namespace CgmInfoGui.Visuals;

public class ArcVisual : VisualBase
{
    private ArcVisual(Point center, Point start, Point end)
    {
        Center = center;
        Start = start;
        End = end;
    }
    public ArcVisual(Point center, Point start, Point end, double radius)
        : this(center, start, end)
    {
        RadiusY = RadiusX = radius;
    }

    public ArcVisual(Point center, Point firstConjugateDiameter, Point secondConjugateDiameter, Point start, Point end)
        : this(center, start, end)
    {
        // calculate the actual radius in X/Y direction
        RadiusX = Distance(Center, firstConjugateDiameter);
        RadiusY = Distance(Center, secondConjugateDiameter);
        // calculate angle between first CDP and center
        Angle = RadiansToDegrees(Math.Atan2(firstConjugateDiameter.Y - Center.Y, firstConjugateDiameter.X - Center.X));
    }

    public Point Center { get; }
    public Point Start { get; }
    public Point End { get; }
    public double RadiusX { get; }
    public double RadiusY { get; }
    public double Angle { get; }

    protected internal override void DrawTo(DrawingContext drawingContext, VisualContext visualContext)
    {
        double startAngle = Math.Atan2(Start.Y, Start.X);
        double endAngle = Math.Atan2(End.Y, End.X);
        var startPoint = new Point(Center.X + (Math.Cos(startAngle) * RadiusX), Center.Y + (Math.Sin(startAngle) * RadiusY));
        var endPoint = new Point(Center.X + (Math.Cos(endAngle) * RadiusX), Center.Y + (Math.Sin(endAngle) * RadiusY));
        double rotationAngle = RadiansToDegrees(endAngle - startAngle);

        // D.4.5.6 CIRCULAR ARC CENTRE
        // If the start ray and the end ray coincide, it is recommended that the interpreter draw the full circle.
        if (rotationAngle == 0)
        {
            // But: ArcTo cannot be used to draw a full circle.
            // FIXME: use EDGE* and INTERIOR* here.
            drawingContext.DrawEllipse(null, GetBlack(), Center, RadiusX, RadiusY);
        }
        else
        {
            var geo = new StreamGeometry();
            using (var ctx = geo.Open())
            {
                ctx.BeginFigure(visualContext.Correct(startPoint), isFilled: false);
                ctx.ArcTo(visualContext.Correct(endPoint), new Size(RadiusX, RadiusY), rotationAngle, isLargeArc: rotationAngle > 180, SweepDirection.Clockwise, isStroked: true);
                ctx.EndFigure(isClosed: false);
            }
            // FIXME: use EDGE* and INTERIOR* here.
            drawingContext.DrawGeometry(null, GetBlack(), geo);
        }
    }
}
