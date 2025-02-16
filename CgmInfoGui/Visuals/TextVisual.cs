using System.Globalization;
using Avalonia;
using Avalonia.Media;

namespace CgmInfoGui.Visuals;

public class TextVisual : VisualBase
{
    public TextVisual(string text, Point location)
    {
        Text = text;
        Location = location;
    }
    public string Text { get; set; }
    public Point Location { get; }

    protected internal override void DrawTo(DrawingContext drawingContext)
    {
        var formattedText = new FormattedText(Text, CultureInfo.GetCultureInfo("en"), FlowDirection.LeftToRight, new Typeface(new FontFamily("Arial"), FontStyle.Normal, FontWeight.Normal, FontStretch.Normal), 12, Brushes.Black);
        drawingContext.DrawText(formattedText, Location);
    }
}
