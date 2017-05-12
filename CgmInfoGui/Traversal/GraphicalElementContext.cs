using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using CgmInfo.Commands.Enums;
using CgmInfoGui.Visuals;
using CgmTextAlignment = CgmInfo.Commands.Attributes.TextAlignment;

namespace CgmInfoGui.Traversal;

public class GraphicalElementContext
{
    public TextVisual? LastText { get; set; }
    public VisualRoot Visuals { get; } = new();
    public LineAttributeValues LineAttributes { get; } = new();
    public TextAttributeValues TextAttributes { get; } = new();

    public void Add(VisualBase visual)
    {
        Visuals.Add(visual);
    }
    public void SetMaximumExtent(Point lowerLeft, Point upperRight)
    {
        Visuals.VdcExtent = new Rect(lowerLeft, upperRight);
        SetExtent(lowerLeft, upperRight);

        // CHARACTER HEIGHT: [ISO/IEC 8632-1 8]
        // 1/100 of the length of the longest side of the rectangle defined by default VDC extent
        // NOTE: we cannot do that in SetExtent, since that is the actually calculated height after everything has drawn
        double longestSide = Math.Max(Visuals.VdcExtent.Width, Visuals.VdcExtent.Height);
        double assumedDefaultFontSize = longestSide / 100.0;
        TextAttributes.CharacterHeight = assumedDefaultFontSize;
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

    public void FinalizeText(TextVisual text)
    {
        // this location assumes the default WPF layout of X-right/Y-down (Horizontal-Left/Vertical-Bottom)
        // the CGM equivalent would be Horizontal-Left/Vertical-Top, while the default of Normal/Normal
        // would be something like Left/Baseline instead.
        // [ISO/IEC 8632-1 6.7.3ff]
        var location = text.Location;

        // we just need text dimensions, we don't care about the color here.
        var formattedText = text.CreateFormattedText(textColor: null);
        double textWidth = formattedText.Width;
        switch (TextAttributes.TextAlignment.Horizontal)
        {
            case HorizontalTextAlignment.Normal:
                // Normal depends on Text Path: [ISO/IEC 8632-1 7.7.18]
                //  TP-Right -> Left
                //  TP-Left -> Right
                //  TP-Up -> Center
                //  TP-Down -> Center
                if (TextAttributes.TextPath == TextPathType.Right)
                    goto case HorizontalTextAlignment.Left;
                else if (TextAttributes.TextPath == TextPathType.Left)
                    goto case HorizontalTextAlignment.Right;
                else
                    goto case HorizontalTextAlignment.Center;
            case HorizontalTextAlignment.Left:
                // nothing to do, value is Left-aligned already
                break;
            case HorizontalTextAlignment.Center:
                // calculate the center point based on text width
                location = location.WithX(location.X - (textWidth / 2.0));
                break;
            case HorizontalTextAlignment.Right:
                // shift the location over by the text width
                location = location.WithX(location.X - textWidth);
                break;
            case HorizontalTextAlignment.Continuous:
                // shift contually by the given value
                // this means: assume the total width as 1.0, add that value to the location
                double continuousShift = textWidth * TextAttributes.TextAlignment.HorizontalContinuousAlignment;
                location = location.WithX(location.X + continuousShift);
                break;
            default:
                // should not happen
                break;
        }

        // TODO: is vertical alignment based on character height, or overall text height? (different for multiline text!)
        double textHeight = TextAttributes.CharacterHeight;
        switch (TextAttributes.TextAlignment.Vertical)
        {
            case VerticalTextAlignment.Normal:
                // Normal depends on Text Path: [ISO/IEC 8632-1 7.7.18]
                //  TP-Right -> Baseline
                //  TP-Left -> Baseline
                //  TP-Up -> Baseline
                //  TP-Down -> Top
                // assume we're default left-to-right text path
                if (TextAttributes.TextPath == TextPathType.Down)
                    goto case VerticalTextAlignment.Top;
                else
                    goto case VerticalTextAlignment.Base;
            case VerticalTextAlignment.Top:
            case VerticalTextAlignment.Cap:
                // FIXME: Top is not the same as Cap
                // nothing to do, text is Top-aligned already
                break;
            case VerticalTextAlignment.Half:
                // calculate the center point based on character height
                location = location.WithY(location.Y + (textHeight / 2.0));
                break;
            case VerticalTextAlignment.Base:
            case VerticalTextAlignment.Bottom:
                // FIXME: Bottom is not the same as Base
                // shift the location down by text height
                location = location.WithY(location.Y + textHeight);
                break;
            case VerticalTextAlignment.Continuous:
                // shift contually by the given value
                // this means: assume the total height as 1.0, add that value to the location
                double continuousShift = textWidth * TextAttributes.TextAlignment.VerticalContinuousAlignment;
                location = location.WithY(location.Y + continuousShift);
                break;
            default:
                // should not happen
                break;
        }

        text.Location = location;
    }

    public sealed class TextAttributeValues
    {
        public double CharacterExpansionFactor { get; internal set; }
        public double CharacterSpacing { get; internal set; }
        public double CharacterHeight { get; internal set; }
        public Color TextColor { get; internal set; }
        public CgmTextAlignment TextAlignment { get; internal set; }
        public TextPathType TextPath { get; internal set; }
        internal int FontIndex { private get; set; }
        internal string[] FontList { private get; set; }

        internal TextAttributeValues()
        {
            // METAFILE DEFAULTS [ISO/IEC 8632-1 8]
            CharacterExpansionFactor = 1.0f;
            CharacterSpacing = 0.0f;
            TextAlignment = new CgmTextAlignment(HorizontalTextAlignment.Normal, VerticalTextAlignment.Normal, 0.0, 0.0);
            FontIndex = 1;
            FontList = [FallbackFont];
            // technically, those are wrong (because they depend on the file configuration); but good enough for now.
            TextColor = Colors.Black;
            CharacterHeight = 12f;
        }

        public static readonly string FallbackFont = "Arial";
        public string Font
        {
            get
            {
                // TEXT FONT INDEX starts at index 1 [ISO/IEC 8632-1 7.3.13]
                int fontIndex = FontIndex - 1;
                if (FontList != null && fontIndex >= 0 && fontIndex < FontList.Length)
                    return FontList[fontIndex];
                return FallbackFont;
            }
        }

        public FontFamily GetFontFamily()
        {
            // unsupported font families automatically fall back to something that the system has
            return new FontFamily(Font);
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
