using System;
using System.Windows;
using System.Windows.Media;

namespace CgmInfoGui.Visuals
{
    public class CircularArcVisual : VisualBase
    {
        public CircularArcVisual(Point center, Point start, Point end, double radius)
        {
            Center = center;
            Start = start;
            End = end;
            Radius = radius;
        }
        public Point Center { get; private set; }
        public Point Start { get; private set; }
        public Point End { get; private set; }
        public double Radius { get; private set; }

        protected internal override void DrawTo(DrawingContext drawingContext, VisualContext visualContext)
        {
            double startAngle = Math.Atan2(Start.Y, Start.X);
            double endAngle = Math.Atan2(End.Y, End.X);
            var startPoint = new Point(Center.X + Math.Cos(startAngle) * Radius, Center.Y + Math.Sin(startAngle) * Radius);
            var endPoint = new Point(Center.X + Math.Cos(endAngle) * Radius, Center.Y + Math.Sin(endAngle) * Radius);
            double rotationAngle = RadiansToDegrees(endAngle - startAngle);
            if (rotationAngle < 0)
                rotationAngle = Math.Abs(rotationAngle);

            var geo = new StreamGeometry();
            using (var ctx = geo.Open())
            {
                ctx.BeginFigure(visualContext.Correct(startPoint), false, false);
                ctx.ArcTo(visualContext.Correct(endPoint), new Size(Radius, Radius), 0, rotationAngle > 180, SweepDirection.Clockwise, true, false);
            }
            geo.Freeze();
            drawingContext.DrawGeometry(null, GetBlack(), geo);
        }
    }
}
