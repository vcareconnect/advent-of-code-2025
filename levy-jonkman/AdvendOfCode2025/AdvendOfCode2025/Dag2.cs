namespace AdvendOfCode2025;

public class Dag2 : Dag
{
    public override void ExecutePart1() => Execute(IsInvalidIDPort1); // 9188031749
    public override void ExecutePart2() => Execute(IsInvalidIDPort2); // 11323661261

    private static void Execute(Func<long, bool> func)
    {
        var input = GetRanges();

        long counter = 0;
        foreach (var (min, max) in input)
        {
            for (var x = min; x <= max; x++)
            {
                if (func(x))
                {
                    counter += x;
                }
            }
        }

        Console.WriteLine(counter);
    }

    private static bool IsInvalidIDPort1(long id)
    {
        var digits = id.ToString();

        var half = digits.Length / 2;
        return IsInPattern(digits, digits.Substring(0, half));
    }

    private static bool IsInvalidIDPort2(long id)
    {
        var digits = id.ToString();

        var half = digits.Length / 2;
        for (var i = 0; i < half; i++)
        {
            var pattern = digits[..(i + 1)];
            if (IsInPattern(digits, pattern))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsInPattern(string digits, string pattern)
    {
        if (pattern.Length == 0)
        {
            return false;
        }

        var t = digits.Length / pattern.Length;
        if (t * pattern.Length != digits.Length)
        {
            return false;
        }

        return digits == string.Concat(Enumerable.Repeat(pattern, t));
    }

    private static List<(long Min, long Max)> GetRanges() => LoadInput().Split(",").Select(r =>
    {
        var parts = r.Split("-");
        return (Min: long.Parse(parts[0]), Max: long.Parse(parts[1]));
    }).ToList();
}