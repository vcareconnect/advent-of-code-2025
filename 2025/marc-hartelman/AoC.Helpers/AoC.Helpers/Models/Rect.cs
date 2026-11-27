namespace AoC.Helpers.Models;

/// <summary>
/// Represents a 2D rectangle defined by a top-left point and size.
/// </summary>
public readonly record struct Rect(int X, int Y, int Width, int Height)
{
    public int Left => X;
    public int Top => Y;
    public int Right => X + Width;
    public int Bottom => Y + Height;

    /// <summary>
    /// Returns the area of the rectangle.
    /// </summary>
    public long Area => (long)Width * Height;

    /// <summary>
    /// Checks if a Point2D is inside this rectangle.
    /// </summary>
    public bool Contains(Point2D p)
    {
        return p.X >= Left && p.X < Right && p.Y >= Top && p.Y < Bottom;
    }

    /// <summary>
    /// Checks if this rectangle intersects with another.
    /// </summary>
    public bool Intersects(Rect other)
    {
        return Left < other.Right && Right > other.Left &&
               Top < other.Bottom && Bottom > other.Top;
    }

    /// <summary>
    /// Creates a bounding box that encompasses a collection of points.
    /// </summary>
    public static Rect FromPoints(IEnumerable<Point2D> points)
    {
        int minX = int.MaxValue, minY = int.MaxValue;
        int maxX = int.MinValue, maxY = int.MinValue;

        foreach (var p in points)
        {
            if (p.X < minX) minX = p.X;
            if (p.X > maxX) maxX = p.X;
            if (p.Y < minY) minY = p.Y;
            if (p.Y > maxY) maxY = p.Y;
        }

        return new Rect(minX, minY, maxX - minX + 1, maxY - minY + 1);
    }
}