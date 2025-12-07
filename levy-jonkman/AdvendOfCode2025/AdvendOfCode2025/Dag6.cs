namespace AdvendOfCode2025;

public class Dag6 : Dag
{
    public override void ExecutePart1()
    {
        var input = LoadInput().Replace("\r", "").Split("\n");
        var data = input.Select(x => Format(x).Split("-")).ToArray();
        var list = new Dictionary<int, List<string>>();

        for (var i = 0; i < data.Length; i++)
        {
            var line = data[i];
            for (var j = 0; j < line.Length; j++)
            {
                var value = line[j];
                if (!list.TryGetValue(j, out var arr))
                {
                    arr = [];
                    list.Add(j, arr);
                }

                arr.Add(value);
            }
        }

        var realData = list.OrderBy(x => x.Key).Select(x => x.Value).ToList();

        long counter = 0;
        foreach (var sum in realData)
        {
            long count = 0;
            var isPlus = sum[^1] == "+";
            for (var i = 0; i < sum.Count - 1; i++)
            {
                var value = long.Parse(sum[i]);
                if (isPlus)
                {
                    count += value;
                }
                else
                {
                    if (count == 0)
                    {
                        count = value;
                    }
                    else
                    {
                        count *= value;
                    }
                }
            }

            counter += count;
        }

        Console.WriteLine("Result " + counter + ": " + counter);
    }
    
    private static string Format(string s)
    {
        var lastSpace = false;
        var newString = "";
        foreach (var letter in s)
        {
            if (letter == ' ')
            {
                if (lastSpace || newString.Length == 0)
                {
                    continue;
                }

                newString += '-';
                lastSpace = true;
            }
            else
            {
                newString += letter;
                lastSpace = false;
            }
        }

        return lastSpace ? newString[..^1] : newString;
    }


    public override void ExecutePart2()
    {
        var input = LoadInput().Replace("\r", "").Split("\n");

        var newSum = true;
        var isPlus = false;
        long num = 0;
        long finalResult = 0;

        for (var i = 0; i < input[0].Length; i++)
        {
            var firstNumber = input.Select(x => x[i]).ToArray();
            if (firstNumber.All(x => x == ' '))
            {
                newSum = true;
                continue;
            }

            if (newSum)
            {
                isPlus = firstNumber[^1] == '+';
                finalResult += num;
                num = ToNumber(firstNumber);
                newSum = false;
            }
            else
            {
                if (isPlus)
                {
                    num += ToNumber(firstNumber);
                }
                else
                {
                    num *= ToNumber(firstNumber);
                }
            }
        }
        finalResult += num;
        Console.WriteLine(finalResult);
    }

    private static long ToNumber(char[] data)
    {
        var res = "";
        for (var i = 0; i < data.Length - 1; i++)
        {
            var c = data[i];
            res += c;
        }

        return long.Parse(res.Trim());
    }
}