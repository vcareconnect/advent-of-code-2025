using System.Collections.Concurrent;

namespace AoC.Helpers.Helpers;

public static class Memoizer
{
    /// <summary>
    /// Returns a function that caches the results of the provided function 'f'.
    /// Useful for recursive DP problems.
    /// </summary>
    public static Func<TInput, TResult> Memoize<TInput, TResult>(Func<TInput, TResult> f) 
        where TInput : notnull
    {
        var cache = new ConcurrentDictionary<TInput, TResult>();
        return a => cache.GetOrAdd(a, f);
    }
    
    // Example Usage:
    // Func<int, long> fib = null;
    // fib = Memoizer.Memoize<int, long>(n => n <= 2 ? 1 : fib(n - 1) + fib(n - 2));
}