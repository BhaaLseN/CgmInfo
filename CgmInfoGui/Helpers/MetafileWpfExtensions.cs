using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using CgmInfo.Utilities;
using AvaloniaPoint = Avalonia.Point;

namespace CgmInfoGui;

public static class MetafileWpfExtensions
{
    public static AvaloniaPoint ToPoint(this MetafilePoint point)
    {
        return new AvaloniaPoint(point.X, point.Y);
    }
    public static AvaloniaPoint[] ToPoints(this MetafilePoint[] points)
    {
        return points.Select(p => p.ToPoint()).ToArray();
    }

    public static Color GetColor(this MetafileColor color)
    {
        try
        {
            var argb = color.ToARGB();
            return Color.FromArgb((byte)argb.Alpha, (byte)argb.Red, (byte)argb.Green, (byte)argb.Blue);
        }
        // not all color spaces are supported yet; make it obvious in case there is one.
        catch (NotSupportedException)
        {
            return Colors.HotPink;
        }
    }

    public static void PolyLineTo(this StreamGeometryContext context, IEnumerable<AvaloniaPoint> points, bool isStroked)
    {
        foreach (var point in points)
            context.LineTo(point, isStroked);
    }
}
