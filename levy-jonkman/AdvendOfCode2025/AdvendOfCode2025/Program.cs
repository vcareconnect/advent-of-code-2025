// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using AdvendOfCode2025;

Console.WriteLine("Hello, World!");

var start = Stopwatch.GetTimestamp();
new Dag2().ExecutePart1();
new Dag2().ExecutePart2();
new Dag3().ExecutePart1();
new Dag3().ExecutePart2();
new Dag4().ExecutePart1();
new Dag4().ExecutePart2();
var diff = Stopwatch.GetElapsedTime(start);
Console.WriteLine($"Time: {diff.TotalMilliseconds} ms");



public class Helper
{
    public static string LoadInput()
    {
        var methodInfo = new StackTrace().GetFrame(2)!.GetMethod();
        var className = methodInfo!.ReflectedType!.Name;
        return File.ReadAllText($"./Inputs/{className}.txt");
    }
}

public abstract class Dag
{
    public abstract void ExecutePart1();
    public abstract void ExecutePart2();

    protected static string LoadInput() => Helper.LoadInput();
}