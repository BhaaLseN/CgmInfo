using System;
using System.Windows;
using System.Windows.Media;

namespace CgmInfoGui.Visuals
{
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
            Angle = RadiansToDegrees(Math.Atan2(FirstConjugateDiameter.Y - Center.Y, FirstConjugateDiameter.X - Center.X));
        }
        public Point Center { get; private set; }
        public Point FirstConjugateDiameter { get; private set; }
        public Point SecondConjugateDiameter { get; private set; }

        public double RadiusX { get; private set; }
        public double RadiusY { get; private set; }
        public double Angle { get; private set; }

        protected internal override void DrawTo(DrawingContext drawingContext, VisualContext visualContext)
        {
            Point center = visualContext.Correct(Center);
            drawingContext.PushTransform(new RotateTransform(Angle, center.X, center.Y));
            drawingContext.DrawEllipse(null, GetBlack(), center, RadiusX, RadiusY);
            drawingContext.Pop();
        }
    }
}
