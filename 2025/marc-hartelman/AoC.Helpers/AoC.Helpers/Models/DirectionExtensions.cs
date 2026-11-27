namespace AoC.Helpers.Models;

public static class DirectionExtensions
{
    /// <summary>
    /// Rotates the direction 90 degrees clockwise.
    /// </summary>
    public static Direction TurnRight(this Direction dir)
    {
        return (Direction)(((int)dir + 1) % 4);
    }

    /// <summary>
    /// Rotates the direction 90 degrees counter-clockwise.
    /// </summary>
    public static Direction TurnLeft(this Direction dir)
    {
        return (Direction)(((int)dir + 3) % 4);
    }

    /// <summary>
    /// Reverses the direction (180 degrees).
    /// </summary>
    public static Direction Opposite(this Direction dir)
    {
        return (Direction)(((int)dir + 2) % 4);
    }

    /// <summary>
    /// Converts a Direction to a Point2D vector.
    /// Assumes standard screen coordinates (Y decreases going North/Up).
    /// </summary>
    public static Point2D ToPoint(this Direction dir) => dir switch
    {
        Direction.North => Point2D.Up,
        Direction.South => Point2D.Down,
        Direction.West => Point2D.Left,
        Direction.East => Point2D.Right,
        _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
    };

    /// <summary>
    /// Parses a character (^, >, v, < or U, R, D, L, or N, E, S, W) to a Direction.
    /// </summary>
    public static Direction FromChar(char c) => c switch
    {
        '^' or 'U' or 'N' => Direction.North,
        '>' or 'R' or 'E' => Direction.East,
        'v' or 'D' or 'S' => Direction.South,
        '<' or 'L' or 'W' => Direction.West,
        _ => throw new ArgumentException($"Invalid direction char: {c}")
    };
}