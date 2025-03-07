using System;
using Avalonia;

namespace CgmInfoGui.Visuals;

public class VisualContext
{
    public VisualContext(Rect geometryExtent, double directionX, double directionY)
    {
        GeometryExtent = geometryExtent;
        DirectionX = directionX;
        DirectionY = directionY;
    }
    public Rect GeometryExtent { get; }
    public double DirectionX { get; }
    public double DirectionY { get; }

    /// <summary>
    /// Returns a corrected version of <paramref name="point"/>
    /// based on <see cref="GeometryExtent"/> and <see cref="DirectionX"/>/<see cref="DirectionY"/>.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public Point Correct(Point point)
    {
        // Avalonia canvas direction is 1.0/-1.0 (ie. positive X is +1.0 to the right, positive Y is -1.0 down)
        // same direction? no need to flip anything around.
        if (DirectionX == 1.0 && DirectionY == -1.0)
            return point;

        double newX = DirectionX == 1.0 ? point.X : GeometryExtent.Right - point.X + GeometryExtent.Left;
        double newY = DirectionY == -1.0 ? point.Y : GeometryExtent.Bottom - point.Y + GeometryExtent.Top;
        return new Point(newX, newY);
    }
    /// <summary>
    /// Returns the calculated angle from <paramref name="x"/> and <paramref name="y"/>,
    /// corrected for the coordinate system orientation.
    /// </summary>
    /// <param name="y"></param>
    /// <param name="x"></param>
    /// <returns>Math.Atan2(<paramref name="y"/>, <paramref name="x"/>)</returns>
    public double Angle(double y, double x)
    {
        if (DirectionX == -1.0)
            x *= -1.0;
        if (DirectionY == 1.0)
            y *= -1.0;

        return Math.Atan2(y, x);
    }
}
