// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using AdvendOfCode2025;

Console.WriteLine("Hello, World!");

var start = Stopwatch.GetTimestamp();
// new Dag2().ExecutePart1();
// new Dag2().ExecutePart2();
// new Dag3().ExecutePart1();
// new Dag3().ExecutePart2();
// new Dag4().ExecutePart1();
// new Dag4().ExecutePart2();
// new Dag5().ExecutePart1();
// new Dag5().ExecutePart2();
// new Dag6().ExecutePart1();
// new Dag6().ExecutePart2();
// new Dag7().ExecutePart1();
// new Dag7().ExecutePart2();
new Dag8().ExecutePart1();
// new Dag8().ExecutePart2();
var diff = Stopwatch.GetElapsedTime(start);
Console.WriteLine($"Time: {diff.TotalMilliseconds} ms");



public class Helper
{
    public static string LoadInput(bool example)
    {
        var methodInfo = new StackTrace().GetFrame(2)!.GetMethod();
        var className = methodInfo!.ReflectedType!.Name;
        var name = example ? $"{className}_Example" : className;
        return File.ReadAllText($"./Inputs/{name}.txt");
    }
}

public abstract class Dag
{
    public abstract void ExecutePart1();
    public abstract void ExecutePart2();

    protected static string LoadInput(bool example = false) => Helper.LoadInput(example);
}