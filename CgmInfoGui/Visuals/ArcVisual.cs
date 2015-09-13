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
        if (RadiusX < RadiusY)
            (RadiusX, RadiusY) = (RadiusY, RadiusX);

        // calculate angle between CDPs and center for the arc sweep
        double firstCDPAngle = Math.Atan2(firstConjugateDiameter.Y - center.Y, firstConjugateDiameter.X - center.X);
        double secondCDPAngle = Math.Atan2(secondConjugateDiameter.Y - center.Y, secondConjugateDiameter.X - center.X);
        if (firstCDPAngle < 0)
            firstCDPAngle += 2 * Math.PI;
        if (secondCDPAngle < 0)
            secondCDPAngle += 2 * Math.PI;

        double sweepAngle = secondCDPAngle - firstCDPAngle;
        if (sweepAngle < 0)
            sweepAngle += 2 * Math.PI;
        if (firstCDPAngle > secondCDPAngle)
            sweepAngle = (2 * Math.PI) - sweepAngle;

        Angle = sweepAngle;
    }

    public Point Center { get; }
    public Point Start { get; }
    public Point End { get; }
    public double RadiusX { get; }
    public double RadiusY { get; }
    public double Angle { get; }

    protected internal override void DrawTo(DrawingContext drawingContext, VisualContext visualContext)
    {
        var start = Vector.Normalize(Start);
        var end = Vector.Normalize(End);
        var nnStart = new Point(Center.X + (start.X * RadiusX), Center.Y + (start.Y * RadiusY));
        var nnEnd = new Point(Center.X + (end.X * RadiusX), Center.Y + (end.Y * RadiusY));

        var startPoint = visualContext.Correct(nnStart);
        var endPoint = visualContext.Correct(nnEnd);

        // D.4.5.6 CIRCULAR ARC CENTRE
        // If the start ray and the end ray coincide, it is recommended that the interpreter draw the full circle.
        if (Angle == 0)
        {
            // But: ArcTo cannot be used to draw a full circle.
            // FIXME: use EDGE* and INTERIOR* here.
            drawingContext.DrawEllipse(null, GetBlack(), visualContext.Correct(Center), RadiusX, RadiusY);
        }
        else
        {
            var geo = new StreamGeometry();
            using (var ctx = geo.Open())
            {
                ctx.BeginFigure(startPoint, isFilled: false);
                ctx.ArcTo(endPoint, new Size(RadiusX, RadiusY), rotationAngle: 0, isLargeArc: RadiansToDegrees(Angle) > 180, SweepDirection.Clockwise, isStroked: true);
                ctx.EndFigure(isClosed: false);
            }
            // FIXME: use EDGE* and INTERIOR* here.
            drawingContext.DrawGeometry(null, GetBlack(), geo);
        }
    }
}
