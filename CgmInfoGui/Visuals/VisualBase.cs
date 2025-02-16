using System;
using Avalonia.Media;

namespace CgmInfoGui.Visuals;

public abstract class VisualBase
{
    protected internal abstract void DrawTo(DrawingContext drawingContext);

    private static readonly Lazy<Pen> Black = new(() =>
    {
        var black = new Pen(Brushes.Black, 1);
        return black;
    });
    protected static Pen GetBlack() => Black.Value;

    // FIXME: figure out a useful way of inverting color in dark theme.
    private static readonly Lazy<Color> ForegroundColor = new(() => /*App.Current?.ActualThemeVariant == Avalonia.Styling.ThemeVariant.Dark ? Colors.LightGray :*/ Colors.Black);
    protected internal static Color GetForegroundColor() => ForegroundColor.Value;

    private static readonly Lazy<Color> BackgroundColor = new(() => /*App.Current?.ActualThemeVariant == Avalonia.Styling.ThemeVariant.Dark ? Colors.Black :*/ Colors.LightGray);
    protected internal static Color GetBackgroundColor() => BackgroundColor.Value;
}
