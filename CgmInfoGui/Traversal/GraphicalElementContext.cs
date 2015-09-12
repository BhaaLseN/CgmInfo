using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using CgmInfoGui.Visuals;

namespace CgmInfoGui.Traversal
{
    public class GraphicalElementContext
    {
        public TextVisual LastText { get; set; }
        public VisualRoot Visuals { get; } = new VisualRoot();
        public LineAttributeValues LineAttributes { get; } = new LineAttributeValues();

        public void Add(VisualBase visual)
        {
            Visuals.Add(visual);
        }
        public void SetExtent(Point lowerLeft, Point upperRight)
        {
            Visuals.VdcExtent = new Rect(lowerLeft, upperRight);
            Visuals.DirectionX = lowerLeft.X <= upperRight.X ? 1.0 : -1.0;
            Visuals.DirectionY = lowerLeft.Y <= upperRight.Y ? 1.0 : -1.0;
        }
        public void IncreaseBounds(Point point)
        {
            IncreaseBounds(new Rect(point, new Size(1, 1)));
        }
        public void IncreaseBounds(Rect rect)
        {
            if (Visuals.GeometryExtent.IsEmpty)
            {
                Visuals.GeometryExtent = rect;
            }
            else
            {
                Visuals.GeometryExtent = Rect.Union(Visuals.GeometryExtent, rect);
            }
        }

        public sealed class LineAttributeValues
        {
            public SolidColorBrush LineColor { get; internal set; }
            public double LineWidth { get; internal set; }
            internal int LineType { private get; set; }
            internal int DashCap { private get; set; }
            internal int LineCap { private get; set; }
            internal int LineJoin { private get; set; }
            internal double MiterLimit { private get; set; }

            internal LineAttributeValues()
            {
                // METAFILE DEFAULTS [ISO/IEC 8632-1 8]
                LineType = 1;
                DashCap = 1;
                LineCap = 1;
                LineJoin = 1;
                MiterLimit = 32767.0d;
                LineWidth = 1.0d;
                // "device-dependant foreground color", black seems to be a good fit on screen.
                LineColor = Brushes.Black;
            }

            private static readonly DashStyle StitchLine = Frozen(new DashStyle(new[] { 10d, 10d }, 0d));
            private static readonly DashStyle ChainLine = Frozen(new DashStyle(new[] { 20d, 5d, 5d, 5d }, 0d));
            private static readonly DashStyle CenterLine = Frozen(new DashStyle(new[] { 40d, 5d, 10d, 5d }, 0d));
            private static readonly DashStyle HiddenLine = Frozen(new DashStyle(new[] { 10d, 5d }, 0d));
            private static readonly DashStyle PhantomLine = Frozen(new DashStyle(new[] { 40d, 2.5d, 10d, 2.5d, 10d, 2.5d }, 0d));
            private static readonly Dictionary<int, Action<Pen>> DashStyleModifications = new Dictionary<int, Action<Pen>>
            {
                { 2, pen => pen.DashStyle = DashStyles.Dash },
                { 3, pen => pen.DashStyle = DashStyles.Dot },
                { 4, pen => pen.DashStyle = DashStyles.DashDot },
                { 5, pen => pen.DashStyle = DashStyles.DashDotDot },
                { 9, pen => pen.DashStyle = StitchLine },
                { 10, pen => pen.DashStyle = ChainLine },
                { 11, pen => pen.DashStyle = CenterLine },
                { 12, pen => pen.DashStyle = HiddenLine },
                { 13, pen => pen.DashStyle = PhantomLine },
            };

            private static readonly Dictionary<int, Action<Pen>> LineCapModifications = new Dictionary<int, Action<Pen>>
            {
                { 3, pen => pen.StartLineCap = pen.EndLineCap = PenLineCap.Round },
                { 4, pen => pen.StartLineCap = pen.EndLineCap = PenLineCap.Square },
                { 5, pen => pen.StartLineCap = pen.EndLineCap = PenLineCap.Triangle },
            };

            private static readonly Dictionary<int, Action<Pen>> DashCapModifications = new Dictionary<int, Action<Pen>>
            {
                { 3, pen => pen.DashCap = pen.StartLineCap },
            };

            private static readonly Dictionary<int, Action<Pen>> LineJoinModifications = new Dictionary<int, Action<Pen>>
            {
                { 2, pen => pen.LineJoin = PenLineJoin.Miter },
                { 3, pen => pen.LineJoin = PenLineJoin.Round },
                { 4, pen => pen.LineJoin = PenLineJoin.Bevel },
            };

            public Pen GetPen()
            {
                var pen = new Pen(LineColor, LineWidth);

                if (DashStyleModifications.TryGetValue(LineType, out var dashStyleMod))
                    dashStyleMod(pen);
                if (LineCapModifications.TryGetValue(LineCap, out var lineCapMod))
                    lineCapMod(pen);
                if (DashCapModifications.TryGetValue(LineCap, out var dashCapMod))
                    dashCapMod(pen);
                if (LineJoinModifications.TryGetValue(LineCap, out var lineJoinMod))
                    lineJoinMod(pen);
                pen.MiterLimit = MiterLimit;

                pen.Freeze();
                return pen;
            }

            private static T Frozen<T>(T freezable) where T : Freezable
            {
                freezable.Freeze();
                return freezable;
            }
        }
    }
}
