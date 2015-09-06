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
    }
}
