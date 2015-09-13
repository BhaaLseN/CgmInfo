using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace CgmInfoGui.Visuals
{
    public class LineVisual : VisualBase
    {
        public LineVisual(Point[] points, Pen pen)
            : this(points, pen, false)
        {
        }
        public LineVisual(Point[] points, Pen pen, bool isClosed)
        {
            IsClosed = isClosed;
            Points = points;
            Pen = pen;
        }
        public Point[] Points { get; private set; }
        public Pen Pen { get; private set; }
        public bool IsClosed { get; private set; }

        protected internal override void DrawTo(DrawingContext drawingContext, VisualContext visualContext)
        {
            var geo = new StreamGeometry();
            using (var ctx = geo.Open())
            {
                Point[] points = Points.Select(p => visualContext.Correct(p)).ToArray();
                ctx.BeginFigure(points[0], false, IsClosed);
                ctx.PolyLineTo(points.Skip(1).ToList(), true, false);
            }
            geo.Freeze();
            drawingContext.DrawGeometry(null, Pen, geo);
        }
    }
}
