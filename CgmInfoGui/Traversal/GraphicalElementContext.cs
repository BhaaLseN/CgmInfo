using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using CgmInfoGui.Visuals;

namespace CgmInfoGui.Traversal;

public class GraphicalElementContext
{
    public TextVisual? LastText { get; set; }
    public VisualRoot Visuals { get; } = new();
    public LineAttributeValues LineAttributes { get; } = new();

    public void Add(VisualBase visual)
    {
        Visuals.Add(visual);
    }
    public void SetMaximumExtent(Point lowerLeft, Point upperRight)
    {
        Visuals.VdcExtent = new Rect(lowerLeft, upperRight);
        SetExtent(lowerLeft, upperRight);
    }
    public void SetExtent(Point lowerLeft, Point upperRight)
    {
        Visuals.DirectionX = lowerLeft.X <= upperRight.X ? 1.0 : -1.0;
        Visuals.DirectionY = lowerLeft.Y <= upperRight.Y ? 1.0 : -1.0;
    }

    public void IncreaseBounds(Point point)
    {
        IncreaseBounds(new Rect(point, new Size(1, 1)));
    }
    public void IncreaseBounds(Rect rect)
    {
        if (Visuals.GeometryExtent is { Width: 0, Height: 0 })
        {
            Visuals.GeometryExtent = rect;
        }
        else
        {
            Visuals.GeometryExtent = Visuals.GeometryExtent.Union(rect);
        }
    }
    public sealed class LineAttributeValues
    {
        public Color LineColor { get; internal set; }
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
            LineColor = VisualBase.GetForegroundColor();
        }

        private static readonly DashStyle StitchLine = new([10d, 10d], 0d);
        private static readonly DashStyle ChainLine = new([20d, 5d, 5d, 5d], 0d);
        private static readonly DashStyle CenterLine = new([40d, 5d, 10d, 5d], 0d);
        private static readonly DashStyle HiddenLine = new([10d, 5d], 0d);
        private static readonly DashStyle PhantomLine = new([40d, 2.5d, 10d, 2.5d, 10d, 2.5d], 0d);
        private static readonly Dictionary<int, Action<Pen>> DashStyleModifications = new()
        {
            { 2, pen => pen.DashStyle = DashStyle.Dash },
            { 3, pen => pen.DashStyle = DashStyle.Dot },
            { 4, pen => pen.DashStyle = DashStyle.DashDot },
            { 5, pen => pen.DashStyle = DashStyle.DashDotDot },
            { 9, pen => pen.DashStyle = StitchLine },
            { 10, pen => pen.DashStyle = ChainLine },
            { 11, pen => pen.DashStyle = CenterLine },
            { 12, pen => pen.DashStyle = HiddenLine },
            { 13, pen => pen.DashStyle = PhantomLine },
        };

        private static readonly Dictionary<int, Action<Pen>> LineCapModifications = new()
        {
            { 3, pen => pen.LineCap = PenLineCap.Round },
            { 4, pen => pen.LineCap = PenLineCap.Square },
            //{ 5, pen => pen.LineCap = PenLineCap.Triangle }, // FIXME: no built-in triangle cap
        };

        private static readonly Dictionary<int, Action<Pen>> DashCapModifications = new()
        {
            //{ 3, pen => pen.DashCap = pen.StartLineCap }, // FIXME: no dash cap support
        };

        private static readonly Dictionary<int, Action<Pen>> LineJoinModifications = new()
        {
            { 2, pen => pen.LineJoin = PenLineJoin.Miter },
            { 3, pen => pen.LineJoin = PenLineJoin.Round },
            { 4, pen => pen.LineJoin = PenLineJoin.Bevel },
        };

        public Pen GetPen()
        {
            var pen = new Pen(new SolidColorBrush(LineColor), LineWidth);

            if (DashStyleModifications.TryGetValue(LineType, out var dashStyleMod))
                dashStyleMod(pen);
            if (LineCapModifications.TryGetValue(LineCap, out var lineCapMod))
                lineCapMod(pen);
            if (DashCapModifications.TryGetValue(LineCap, out var dashCapMod))
                dashCapMod(pen);
            if (LineJoinModifications.TryGetValue(LineCap, out var lineJoinMod))
                lineJoinMod(pen);
            pen.MiterLimit = MiterLimit;

            return pen;
        }
    }
}
