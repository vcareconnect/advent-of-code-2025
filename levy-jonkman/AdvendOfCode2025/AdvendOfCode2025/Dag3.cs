namespace AdvendOfCode2025;

public class Dag3 : Dag
{
    public override void ExecutePart1() => Execute(2); // 17107

    public override void ExecutePart2() => Execute(12); // 169349762274117

    private void Execute(int size)
    {
        var input = LoadInput().Replace("\r", "").Split("\n");

        long sum = 0;
        foreach (var line in input)
        {
            var big = GetBigestNumberInLine(line, size);
            sum += big;
        }

        Console.WriteLine("Sum: " + sum);
    }

    private long GetBigestNumberInLine(string line, int toKeep)
    {
        var left = 0;
        var right = line.Length - (toKeep - 1);

        var digits = new List<int>();
        for (var i = 0; i < toKeep; i++)
        {
            var text = line.Substring(left, right - left);
            var (dig, index) = GetBigestDigitInLine(text);
            left += index + 1;
            right++;

            digits.Add(dig);
        }

        return long.Parse(string.Join("", digits));
    }

    private (int, int) GetBigestDigitInLine(string line)
    {
        var digitOne = int.Parse(line[0].ToString());
        var index = 0;
        for (var i = 1; i < line.Length; i++)
        {
            var digit = int.Parse(line[i].ToString());
            if (digit > digitOne)
            {
                index = i;
                digitOne = digit;
            }
        }

        return (digitOne, index);
    }
}