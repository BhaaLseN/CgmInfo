using System;
using System.Windows;
using System.Windows.Media;

namespace CgmInfoGui.Visuals
{
    public class RectangleVisual : VisualBase
    {
        public RectangleVisual(Point firstCorner, Point secondCorner)
        {
            FirstCorner = new Point(Math.Min(firstCorner.X, secondCorner.X), Math.Min(firstCorner.Y, secondCorner.Y));
            SecondCorner = new Point(Math.Max(firstCorner.X, secondCorner.X), Math.Max(firstCorner.Y, secondCorner.Y));
        }
        public Point FirstCorner { get; private set; }
        public Point SecondCorner { get; private set; }

        protected internal override void DrawTo(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(null, GetBlack(), new Rect(FirstCorner, SecondCorner));
        }
    }
}
