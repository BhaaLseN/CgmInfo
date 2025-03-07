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

        // remember the major axis CDP for later
        MajorAxisPoint = firstRadius >= secondRadius ? FirstConjugateDiameter : SecondConjugateDiameter;
    }
    public Point Center { get; }
    public Point FirstConjugateDiameter { get; }
    public Point SecondConjugateDiameter { get; }
    public Point MajorAxisPoint { get; }

    public double RadiusX { get; }
    public double RadiusY { get; }

    protected internal override void DrawTo(DrawingContext drawingContext, VisualContext visualContext)
    {
        var center = visualContext.Correct(Center);
        double rad = visualContext.Angle(MajorAxisPoint.Y - Center.Y, MajorAxisPoint.X - Center.X);
        // FIXME: use EDGE* and INTERIOR* here.
        using (drawingContext.PushTransform(Matrix.CreateRotation(rad, center)))
            drawingContext.DrawEllipse(null, GetBlack(), center, RadiusX, RadiusY);
    }
}
