using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using CgmInfo.Commands.Enums;
using CgmInfoGui.Visuals;
using CgmTextAlignment = CgmInfo.Commands.Attributes.TextAlignment;

namespace CgmInfoGui.Traversal
{
    public class GraphicalElementContext
    {
        public TextVisual LastText { get; set; }
        public VisualRoot Visuals { get; } = new VisualRoot();
        public LineAttributeValues LineAttributes { get; } = new LineAttributeValues();
        public TextAttributeValues TextAttributes { get; } = new TextAttributeValues();

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
            if (Visuals.GeometryExtent.IsEmpty)
            {
                Visuals.GeometryExtent = rect;
            }
            else
            {
                Visuals.GeometryExtent = Rect.Union(Visuals.GeometryExtent, rect);
            }
        }

        public void FinalizeText(TextVisual text)
        {
            // this location assumes the default WPF layout of X-right/Y-down (Horizontal-Left/Vertical-Bottom)
            // the CGM equivalent would be Horizontal-Left/Vertical-Top, while the default of Normal/Normal
            // would be something like Left/Baseline instead.
            // [ISO/IEC 8632-1 6.7.3ff]
            var location = text.Location;

            var formattedText = text.CreateFormattedText();
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
                    location.X -= textWidth / 2.0;
                    break;
                case HorizontalTextAlignment.Right:
                    // shift the location over by the text width
                    location.X -= textWidth;
                    break;
                case HorizontalTextAlignment.Continuous:
                    // shift contually by the given value
                    // this means: assume the total width as 1.0, add that value to the location
                    double continuousShift = textWidth * TextAttributes.TextAlignment.HorizontalContinuousAlignment;
                    location.X += continuousShift;
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
                    location.Y += textHeight / 2.0;
                    break;
                case VerticalTextAlignment.Base:
                case VerticalTextAlignment.Bottom:
                    // FIXME: Bottom is not the same as Base
                    // shift the location down by text height
                    location.Y += textHeight;
                    break;
                case VerticalTextAlignment.Continuous:
                    // shift contually by the given value
                    // this means: assume the total height as 1.0, add that value to the location
                    double continuousShift = textWidth * TextAttributes.TextAlignment.VerticalContinuousAlignment;
                    location.Y += continuousShift;
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
            public Brush TextColor { get; internal set; }
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
                FontList = new string[1] { FallbackFont };
                // technically, those are wrong (because they depend on the file configuration); but good enough for now.
                TextColor = Brushes.Black;
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
            public double GetFontSize()
            {
                // FIXME: this is wrong, character height is base to cap, while font size is
                //        from one base to another (including ascenders and descenders).
                return CharacterHeight;
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
