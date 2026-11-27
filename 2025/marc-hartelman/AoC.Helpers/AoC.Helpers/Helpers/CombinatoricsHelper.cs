namespace AoC.Helpers.Helpers;

public static class CombinatoricsHelper
{
    /// <summary>
    /// Generates all possible permutations of an enumerable.
    /// Use carefully! The number of permutations grows factorially (N!).
    /// Good for lists of size 10 or less.
    /// </summary>
    public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
    {
        if (length == 1) return list.Select(t => new T[] { t });

        var enumerable = list.ToList();
        return GetPermutations(enumerable, length - 1)
            .SelectMany(t => enumerable.Where(e => !t.Contains(e)),
                (t1, t2) => t1.Concat([t2]));
    }

    /// <summary>
    /// Generates all distinct combinations of a specific size from the input list.
    /// Order does not matter (e.g., {A,B} is the same as {B,A}).
    /// </summary>
    public static IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> elements, int k)
    {
        var enumerable = elements.ToList();
        return k == 0 
            ? [[]]
            : enumerable.SelectMany((e, i) => 
                GetCombinations(enumerable.Skip(i + 1), k - 1).Select(c => (new[] {e}).Concat(c)));
    }
}