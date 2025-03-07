using System;
using Avalonia;
using Avalonia.Media;

namespace CgmInfoGui.Visuals;

public class EllipseVisual : VisualBase
{
    public EllipseVisual(Point center, Point firstConjugateDiameter, Point secondConjugateDiameter)
    {
        Center = center;
        FirstConjugateDiameter = firstConjugateDiameter;
        SecondConjugateDiameter = secondConjugateDiameter;

        // calculate the actual radius in X/Y direction
        RadiusX = Distance(Center, FirstConjugateDiameter);
        RadiusY = Distance(Center, SecondConjugateDiameter);
        // calculate angle between first CDP and center
        Rad = Math.Atan2(FirstConjugateDiameter.Y - Center.Y, FirstConjugateDiameter.X - Center.X);
        Angle = RadiansToDegrees(Rad);
    }
    public Point Center { get; }
    public Point FirstConjugateDiameter { get; }
    public Point SecondConjugateDiameter { get; }

    public double RadiusX { get; }
    public double RadiusY { get; }
    public double Rad { get; }
    public double Angle { get; }

    protected internal override void DrawTo(DrawingContext drawingContext, VisualContext visualContext)
    {
        var center = visualContext.Correct(Center);
        // FIXME: use EDGE* and INTERIOR* here.
        using (drawingContext.PushTransform(Matrix.CreateTranslation(center.X, center.Y) * Matrix.CreateRotation(Rad)))
            drawingContext.DrawEllipse(null, GetBlack(), center, RadiusX, RadiusY);
    }
}
