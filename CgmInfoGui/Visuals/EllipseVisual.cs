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
        using (drawingContext.PushTransform(Transform))
        {
            // Build ellipse-local transform
            var ellipseMatrix = Matrix.CreateTranslation(-0.5, -0.5) * new Matrix(FirstConjugateDiameter.X, FirstConjugateDiameter.Y, SecondConjugateDiameter.X, SecondConjugateDiameter.Y, 0, 0) * Matrix.CreateTranslation(Center.X, Center.Y);
            using (drawingContext.PushTransform(ellipseMatrix))
            {
                drawingContext.DrawEllipse(brush: null, GetBlack(), center: new Point(0.5, 0.5), radiusX: 0.5, radiusY: 0.5);
            }
        }
        return;
        var center = visualContext.Correct(Center);
        double rad = visualContext.Angle(MajorAxisPoint.Y - Center.Y, MajorAxisPoint.X - Center.X);
        // FIXME: use EDGE* and INTERIOR* here.
        using (drawingContext.PushTransform(Matrix.CreateRotation(rad, center)))
            drawingContext.DrawEllipse(null, GetBlack(), center, RadiusX, RadiusY);
    }
}
