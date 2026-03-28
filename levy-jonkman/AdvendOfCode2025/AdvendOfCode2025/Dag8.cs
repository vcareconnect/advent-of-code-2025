namespace AdvendOfCode2025;

public class Dag8 : Dag
{
    private readonly Dictionary<Point, Guid> _circuits = [];
    private readonly List<(Point, Point, double)> _distances;

    private record Point(int X, int Y, int Z);
    
    private const int Count = 1000;

    public Dag8()
    {
        var inputData = LoadInput().Replace("\r", "").Split("\n");

        var points = inputData.Select(x => x.Split(","))
            .Select(x => new Point(int.Parse(x[0]), int.Parse(x[1]), int.Parse(x[2]))).ToList();

        var distances = new List<(Point, Point, double)>();
        for (var i = 0; i < points.Count; i++)
        {
            for (var j = i + 1; j < points.Count; j++)
            {
                if (i == j) continue;

                var distance = CalculateDistance(points[i], points[j]);
                distances.Add((points[i], points[j], distance));
            }
        }

        _distances = distances.OrderBy(x => x.Item3).ToList();
    }


    public override void ExecutePart1()
    {
        for (var i = 0; i < Count; i++)
        {
            var (point1, point2, _) = _distances[i];
            if (_circuits.TryGetValue(point1, out var circuit1) &&
                _circuits.TryGetValue(point2, out var circuit2) &&
                circuit1 == circuit2)
            {
                continue;
            }

            AddPointsToCircuit(point1, point2);
        }

        var sizes = _circuits.GroupBy(x => x.Value)
            .Select(x => (x.Key, x.Count()))
            .OrderByDescending(x => x.Item2)
            .Select(x => x.Item2).ToList();
        var result = sizes[0] * sizes[1] * sizes[2];

        Console.WriteLine("Result: " + result);
    }

    public override void ExecutePart2()
    {
        (Point, Point, double) last = default!;
        foreach (var points in _distances)
        {
            var (point1, point2, _) = points;
            if (_circuits.TryGetValue(point1, out var circuit1) &&
                _circuits.TryGetValue(point2, out var circuit2) &&
                circuit1 == circuit2)
            {
                continue;
            }

            AddPointsToCircuit(point1, point2);
            last = points;
        }

        var result = last.Item1!.X * last.Item2!.X;

        Console.WriteLine("Result: " + result);
    }

    private static long CalculateDistance(Point p1, Point p2)
    {
        long deltaX = p1.X - p2.X;
        long deltaY = p1.Y - p2.Y;
        long deltaZ = p1.Z - p2.Z;

        return deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ;
    }

    private void AddPointsToCircuit(Point p1, Point p2)
    {
        if (_circuits.TryGetValue(p1, out var circuit1) && _circuits.TryGetValue(p2, out var circuit2))
        {
            var circuitsToMerge = _circuits.Where(x => x.Value == circuit2).Select(x => x.Key).ToList();
            foreach (var point in circuitsToMerge)
            {
                _circuits[point] = circuit1;
            }
        }
        else if (_circuits.TryGetValue(p1, out circuit1) && !_circuits.TryGetValue(p2, out circuit2))
        {
            _circuits[p2] = circuit1;
        }
        else if (!_circuits.TryGetValue(p1, out circuit1) && _circuits.TryGetValue(p2, out circuit2))
        {
            _circuits[p1] = circuit2;
        }
        else
        {
            var newCircuitId = Guid.NewGuid();
            _circuits[p1] = newCircuitId;
            _circuits[p2] = newCircuitId;
        }
    }
}