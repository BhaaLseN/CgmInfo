using System;
using System.Windows.Media;

namespace CgmInfoGui.Visuals
{
    public abstract class VisualBase
    {
        protected internal abstract void DrawTo(DrawingContext drawingContext, VisualContext visualContext);
        private static readonly Lazy<Pen> Black = new Lazy<Pen>(() =>
        {
            var black = new Pen(Brushes.Black, 1);
            black.Freeze();
            return black;
        });
        protected static Pen GetBlack() => Black.Value;
    }
}
