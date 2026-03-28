namespace AdvendOfCode2025;

public class Dag4 : Dag
{
    private readonly char[][] _grid;
    private const char PaperRoll = '@';
    private const char NotPaperRoll = '.';
    private const int MaxPaperRolls = 3;

    public Dag4()
    {
        _grid = LoadInput().Replace("\r", "").Split("\n").Select(x => x.ToCharArray()).ToArray();
    }

    public override void ExecutePart1()
    {
        var sum = 0;
        for (var y = 0; y < _grid.Length; y++)
        {
            for (var x = 0; x < _grid[0].Length; x++)
            {
                var currentChar = _grid[x][y];
                if (currentChar == PaperRoll && CheckValue(x, y))
                {
                    sum++;
                }
            }
        }

        Console.WriteLine("Sum: " + sum);
    }

    public override void ExecutePart2()
    {
        var sum = 0;
        while (Iteration())
        {
            sum++;
        }
        
        Console.WriteLine("Sum: " + sum);
    }
    
    private bool Iteration()
    {
        for (var y = 0; y < _grid.Length; y++)
        {
            for (var x = 0; x < _grid[0].Length; x++)
            {
                var currentChar = _grid[x][y];
                if (currentChar == PaperRoll && CheckValue(x, y))
                {
                    _grid[x][y] = NotPaperRoll;
                    return true;
                }
            }
        }

        return false;
    }

    private bool CheckValue(int x, int y)
    {
        var count = 0;
        for (var dy = -1; dy < 2; dy++)
        {
            for (var dx = -1; dx < 2; dx++)
            {
                if ((dy != 0 || dx != 0) && IsPaperRoll(x + dx, y + dy))
                {
                    count++;
                }
            }
        }
        
        return count <= MaxPaperRolls;
    }

    private bool IsPaperRoll(int x, int y) => x >= 0 && y >= 0 && x < _grid.Length && y < _grid[0].Length && _grid[x][y] == PaperRoll;
}