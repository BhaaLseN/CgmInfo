using System;
using System.Linq;
using System.Windows.Media;
using CgmInfo.Utilities;
using WpfPoint = System.Windows.Point;

namespace CgmInfoGui
{
    public static class MetafileWpfExtensions
    {
        public static WpfPoint ToPoint(this MetafilePoint point)
        {
            return new WpfPoint(point.X, point.Y);
        }
        public static WpfPoint[] ToPoints(this MetafilePoint[] points)
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
    }
}
