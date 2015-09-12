using System.Windows;
using System.Windows.Media;

namespace CgmInfoGui.Visuals
{
    public class RectangleVisual : VisualBase
    {
        public RectangleVisual(Point firstCorner, Point secondCorner)
        {
            FirstCorner = firstCorner;
            SecondCorner = secondCorner;
        }
        public Point FirstCorner { get; private set; }
        public Point SecondCorner { get; private set; }

        protected internal override void DrawTo(DrawingContext drawingContext, VisualContext visualContext)
        {
            drawingContext.DrawRectangle(null, GetBlack(), new Rect(visualContext.Correct(FirstCorner), visualContext.Correct(SecondCorner)));
        }
    }
}
