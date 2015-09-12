using System.Windows;

namespace CgmInfoGui.Visuals
{
    public class VisualContext
    {
        public VisualContext(Rect geometryExtent, double directionX, double directionY)
        {
            GeometryExtent = geometryExtent;
            DirectionX = directionX;
            DirectionY = directionY;
        }
        public Rect GeometryExtent { get; private set; }
        public double DirectionX { get; private set; }
        public double DirectionY { get; private set; }

        /// <summary>
        /// Returns a corrected version of <paramref name="point"/>
        /// based on <see cref="GeometryExtent"/> and <see cref="DirectionX"/>/<see cref="DirectionY"/>.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public Point Correct(Point point)
        {
            // WPF canvas direction is 1.0/-1.0 (ie. positive X is +1.0 to the right, positive Y is -1.0 down)
            // same direction? no need to flip anything around.
            if (DirectionX == 1.0 && DirectionY == -1.0)
                return point;
            double newX = DirectionX == 1.0 ? point.X : GeometryExtent.Right - point.X + GeometryExtent.Left;
            double newY = DirectionY == -1.0 ? point.Y : GeometryExtent.Bottom - point.Y + GeometryExtent.Top;
            return new Point(newX, newY);
        }
    }
}
