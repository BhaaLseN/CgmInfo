using System;
using System.Windows;
using System.Windows.Media;

namespace CgmInfoGui.Visuals
{
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

        public Point Center { get; private set; }
        public Point Start { get; private set; }
        public Point End { get; private set; }
        public double RadiusX { get; private set; }
        public double RadiusY { get; private set; }
        public double Angle { get; private set; }

        protected internal override void DrawTo(DrawingContext drawingContext, VisualContext visualContext)
        {
            double startAngle = Math.Atan2(Start.Y, Start.X);
            double endAngle = Math.Atan2(End.Y, End.X);
            var startPoint = new Point(Center.X + Math.Cos(startAngle) * RadiusX, Center.Y + Math.Sin(startAngle) * RadiusY);
            var endPoint = new Point(Center.X + Math.Cos(endAngle) * RadiusX, Center.Y + Math.Sin(endAngle) * RadiusY);
            double rotationAngle = RadiansToDegrees(endAngle - startAngle);
            if (rotationAngle < 0)
                rotationAngle = Math.Abs(rotationAngle);

            //drawingContext.PushTransform(new RotateTransform(Angle, Center.X, Center.Y));
            var geo = new StreamGeometry();
            using (var ctx = geo.Open())
            {
                ctx.BeginFigure(visualContext.Correct(startPoint), false, false);
                ctx.ArcTo(visualContext.Correct(endPoint), new Size(RadiusX, RadiusY), 0, rotationAngle > 180, SweepDirection.Clockwise, true, false);
            }
            geo.Freeze();
            drawingContext.DrawGeometry(null, GetBlack(), geo);
            //drawingContext.Pop();
        }
    }
}
