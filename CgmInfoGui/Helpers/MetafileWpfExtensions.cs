using CgmInfo.Utilities;
using AvaloniaPoint = Avalonia.Point;

namespace CgmInfoGui;

public static class MetafileWpfExtensions
{
    public static AvaloniaPoint ToPoint(this MetafilePoint point)
    {
        return new AvaloniaPoint(point.X, point.Y);
    }
}
