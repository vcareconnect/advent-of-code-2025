namespace AoC.Helpers.Models;

public readonly record struct Point2D(int X, int Y)
{
    public static Point2D Zero => new(0, 0);
    public static Point2D Up => new(0, -1);    // Assumes screen coords (0,0 at top-left)
    public static Point2D Down => new(0, 1);
    public static Point2D Left => new(-1, 0);
    public static Point2D Right => new(1, 0);

    // Neighbors for generic grid traversal
    public static Point2D[] Directions4 => [Up, Right, Down, Left];
    public static Point2D[] Directions8 => [
        Up, new(1, -1), Right, new(1, 1),
        Down, new(-1, 1), Left, new(-1, -1)
    ];

    // Operator overloading makes physics simulations trivial
    public static Point2D operator +(Point2D a, Point2D b) => new(a.X + b.X, a.Y + b.Y);
    public static Point2D operator -(Point2D a, Point2D b) => new(a.X - b.X, a.Y - b.Y);
    public static Point2D operator *(Point2D a, int scale) => new(a.X * scale, a.Y * scale);

    public int ManhattanDistance(Point2D other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    
    // Alias properties if you prefer Row/Col mental model
    public int Col => X;
    public int Row => Y;
}