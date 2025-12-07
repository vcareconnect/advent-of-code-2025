namespace AdvendOfCode2025;

public class Dag7 : Dag
{
    private readonly char[][] _input = LoadInput().Replace("\r", "").Split("\n").Select(x => x.ToCharArray()).ToArray();
    private int _splits;

    public override void ExecutePart1()
    {
        var xPosOfS = Array.IndexOf(_input[0], 'S');
        Console.WriteLine($"{_input.Length}");

        Split(xPosOfS, 1);

        Console.WriteLine($"Aantal splitsingen: {_splits}");
    }

    private void Split(int x, int y)
    {
        while (y < _input.Length)
        {
            if (_input[y][x] == '.')
            {
                _input[y][x] = '|';
                y++;
            }
            else if (_input[y][x] == '^')
            {
                _splits++;
                Split(x - 1, y);
                Split(x + 1, y);
                break;
            }
            else
            {
                break;
            }
        }
    }

    public override void ExecutePart2()
    {
        var input2 = _input.Select(x => x.Select(_ => (long)0).ToArray()).ToArray();
        input2[0][Array.IndexOf(_input[0], 'S')] = 1;

        for (var i = 1; i < input2.Length; i++)
        {
            for (var i1 = 0; i1 < input2[i].Length; i1++)
            {
                if (_input[i][i1] == '^')
                {
                    var value = input2[i - 1][i1];
                    input2[i][i1 - 1] += value;
                    input2[i][i1 + 1] += value;
                }
                else
                {
                    input2[i][i1] += input2[i - 1][i1];
                }
            }
        }

        Console.WriteLine($"Aantal timelines: {input2[^1].Sum()}");
    }
}