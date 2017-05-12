using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace CgmInfoGui.Visuals
{
    public class TextVisual : VisualBase
    {
        public TextVisual(string text, Point location, FontFamily fontFamily, double fontSize, Brush fontBrush)
        {
            Text = text;
            Location = location;
            Font = fontFamily;
            FontSize = fontSize;
            FontBrush = fontBrush;
        }

        public string Text { get; set; }
        public Point Location { get; internal set; }
        public double FontSize { get; }
        public Brush FontBrush { get; }
        public FontFamily Font { get; }

        internal static double PixelsPerDip { get; set; } = 1.0d;

        protected internal override void DrawTo(DrawingContext drawingContext, VisualContext visualContext)
        {
            var formattedText = CreateFormattedText();
            drawingContext.DrawText(formattedText, visualContext.Correct(Location));
        }

        internal FormattedText CreateFormattedText()
        {
            return new FormattedText(Text, CultureInfo.GetCultureInfo("en"), FlowDirection.LeftToRight, new Typeface(Font, new FontStyle(), new FontWeight(), new FontStretch()), FontSize, FontBrush, PixelsPerDip);
        }
    }
}
