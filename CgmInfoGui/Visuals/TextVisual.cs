using System.Globalization;
using Avalonia;
using Avalonia.Media;

namespace CgmInfoGui.Visuals;

public class TextVisual : VisualBase
{
    public TextVisual(string text, Point location, FontFamily fontFamily, double fontSize, Color fontColor)
    {
        Text = text;
        Location = location;
        Font = fontFamily;
        FontSize = fontSize;
        FontColor = fontColor;
    }

    public string Text { get; set; }
    public Point Location { get; internal set; }
    public double FontSize { get; }
    public Color FontColor { get; }
    public FontFamily Font { get; }

    protected internal override void DrawTo(DrawingContext drawingContext, VisualContext visualContext)
    {
        var formattedText = CreateFormattedText(new SolidColorBrush(FontColor));
        drawingContext.DrawText(formattedText, visualContext.Correct(Location));
    }

    internal FormattedText CreateFormattedText(IBrush? textColor)
    {
        return new FormattedText(Text, CultureInfo.GetCultureInfo("en"), FlowDirection.LeftToRight, new Typeface(Font, FontStyle.Normal, FontWeight.Normal, FontStretch.Normal), FontSize, textColor);
    }
}
