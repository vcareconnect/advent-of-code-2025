namespace AoC.Helpers.Helpers;

public static class StackExtensions
{
    /// <summary>
    /// Pops multiple items from the stack and returns them as a list.
    /// The first item in the list is the item that was at the top of the stack.
    /// </summary>
    public static List<T> PopCount<T>(this Stack<T> stack, int count)
    {
        if (stack.Count < count)
        {
            throw new InvalidOperationException($"Cannot pop {count} items. Stack only has {stack.Count}.");
        }

        var result = new List<T>(count);
        for (var i = 0; i < count; i++)
        {
            result.Add(stack.Pop());
        }
        return result;
    }

    /// <summary>
    /// Pushes multiple items onto the stack.
    /// The last item in the collection will become the new top of the stack.
    /// </summary>
    public static void PushRange<T>(this Stack<T> stack, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            stack.Push(item);
        }
    }

    /// <summary>
    /// Moves N items from this stack to the target stack ONE BY ONE.
    /// This REVERSES the order of the moved chunk.
    /// (Simulates moving items manually one at a time).
    /// </summary>
    public static void MoveTo<T>(this Stack<T> source, Stack<T> target, int count)
    {
        for (var i = 0; i < count; i++)
        {
            target.Push(source.Pop());
        }
    }

    /// <summary>
    /// Moves N items from this stack to the target stack AS A BATCH.
    /// This PRESERVES the order of the moved chunk.
    /// (Simulates picking up a stack of crates and placing them elsewhere).
    /// </summary>
    public static void MoveBatchTo<T>(this Stack<T> source, Stack<T> target, int count)
    {
        // We need a temporary buffer to preserve order
        var tempBuffer = new T[count];
        
        // Pop into buffer (buffer[0] is Top)
        for (var i = 0; i < count; i++)
        {
            tempBuffer[i] = source.Pop();
        }

        // Push back in reverse order (buffer[last] goes in first)
        for (var i = count - 1; i >= 0; i--)
        {
            target.Push(tempBuffer[i]);
        }
    }

    /// <summary>
    /// Creates a shallow copy of the stack. 
    /// Essential for "What if" simulations where you need to branch logic without destroying the original stack.
    /// </summary>
    public static Stack<T> Clone<T>(this Stack<T> original)
    {
        // Stack enumerator yields from Top to Bottom.
        // Passing that directly to constructor would reverse the stack.
        // We must Reverse the enumeration to reconstruct the stack correctly.
        return new Stack<T>(original.Reverse());
    }
}