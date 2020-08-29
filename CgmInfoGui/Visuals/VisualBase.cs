using System;
using System.Windows;
using System.Windows.Media;

namespace CgmInfoGui.Visuals
{
    public abstract class VisualBase : NotifyPropertyChangedBase
    {
        private VisualContainer _parentContainer;
        public VisualContainer ParentContainer
        {
            get { return _parentContainer; }
            internal set { SetField(ref _parentContainer, value); }
        }

        protected internal abstract void DrawTo(DrawingContext drawingContext, VisualContext visualContext);
        private static readonly Lazy<Pen> Black = new Lazy<Pen>(() =>
        {
            var black = new Pen(Brushes.Black, 1);
            black.Freeze();
            return black;
        });
        protected static Pen GetBlack() => Black.Value;

        public static double RadiansToDegrees(double rad)
        {
            return 180 / Math.PI * rad;
        }
        public static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2.0) + Math.Pow(p2.Y - p1.Y, 2.0));
        }
    }
}
