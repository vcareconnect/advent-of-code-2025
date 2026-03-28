namespace AdvendOfCode2025;

public class Dag5 : Dag
{
    private List<string> _ranges = [];

    private List<string> _products = [];

    public Dag5()
    {
        var switched = false;

        var s = LoadInput();
        var data = LoadInput().Replace("\r", "").Split("\n");

        foreach (var se in data)
        {
            if (string.IsNullOrEmpty(se))
            {
                switched = true;
            }
            else if (switched)
            {
                _products.Add(se);
            }
            else
            {
                _ranges.Add(se);
            }
        }
    }

    public override void ExecutePart1()
    {
        var validCount = 0;
        foreach (var product in _products)
        {
            if (IsInRange(product))
            {
                validCount++;
            }
        }

        Console.WriteLine("Valid products: " + validCount);
    }

    private bool IsInRange(string s)
    {
        var number = long.Parse(s);

        foreach (var range in _ranges)
        {
            var parts = range.Split('-');
            var min = long.Parse(parts[0]);
            var max = long.Parse(parts[1]);

            if (number >= min && number <= max)
            {
                return true;
            }
        }

        return false;
    }

    // public override void ExecutePart2()
    // {
    //     var freshProducts = new HashSet<long>();
    //     for (var index = 0; index < _ranges.Count; index++)
    //     {
    //         var range = _ranges[index];
    //         var parts = range.Split('-');
    //         var min = long.Parse(parts[0]);
    //         var max = long.Parse(parts[1]);
    //
    //         for (var i = min; i <= max; i++)
    //         {
    //             freshProducts.Add(i);
    //         }
    //         
    //         Console.WriteLine("Processed range " + (index + 1) + "/" + _ranges.Count);
    //     }
    //
    //     var count = freshProducts.Count;
    //
    //     Console.WriteLine("Distinct valid products: " + count);
    // }

    public override void ExecutePart2() // 147
    {
        var results = new List<(long Start, long End, long Diff)>();
        for (var index = 0; index < _ranges.Count; index++)
        {
            var range = _ranges[index];
            var parts = range.Split('-');
            var min = long.Parse(parts[0]);
            var max = long.Parse(parts[1]);

            if (results.Any(x => !(max < x.Start || min > x.End)))
            {
                var overlapping = results.Where(x => !(max < x.Start || min > x.End)).ToList();
                var lowest = Math.Min(min, overlapping.Min(x => x.Start));
                var highest = Math.Max(max, overlapping.Max(x => x.End));
                results.RemoveAll(x => overlapping.Contains(x));
                results.Add((lowest, highest, highest - lowest + 1));
            }
            else
            {
                results.Add((min, max, max - min + 1));
            }
        }

        var count = results.Select(x => x.Diff).Sum();

        Console.WriteLine("Distinct valid products: " + count);
    }
}