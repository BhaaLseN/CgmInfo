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
        double firstRadius = Distance(Center, FirstConjugateDiameter);
        double secondRadius = Distance(Center, SecondConjugateDiameter);
        RadiusX = Math.Max(firstRadius, secondRadius);
        RadiusY = Math.Min(firstRadius, secondRadius);

        // calculate angle between major axis CDP and center
        var majorAxisPoint = firstRadius >= secondRadius ? FirstConjugateDiameter : SecondConjugateDiameter;
        // FIXME: Avalonia is Y-, while the default DEVICE VIEWPORT is Y+.
        //        We'd need the DEVICE VIEWPORT value in here to calculate this correctly.
        //        Until then, assume it's the default and just invert the Y value.
        Rad = Math.Atan2(-(majorAxisPoint.Y - Center.Y), majorAxisPoint.X - Center.X);
        Angle = RadiansToDegrees(Rad);
    }
    public Point Center { get; }
    public Point FirstConjugateDiameter { get; }
    public Point SecondConjugateDiameter { get; }

    public double RadiusX { get; }
    public double RadiusY { get; }
    public double Rad { get; }
    public double Angle { get; }

    protected internal override void DrawTo(DrawingContext drawingContext)
    {
        // FIXME: use EDGE* and INTERIOR* here.
        using (drawingContext.PushTransform(Matrix.CreateRotation(Rad, Center)))
            drawingContext.DrawEllipse(null, GetBlack(), Center, RadiusX, RadiusY);
    }
}
