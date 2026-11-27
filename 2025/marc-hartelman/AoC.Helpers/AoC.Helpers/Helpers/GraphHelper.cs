namespace AoC.Helpers.Helpers;

public static class GraphHelper
{
    /// <summary>
    /// Dijkstra's Algorithm for finding the shortest path in a weighted graph.
    /// </summary>
    /// <typeparam name="T">The node type (e.g., Point2D or SearchState).</typeparam>
    /// <param name="start">Starting node.</param>
    /// <param name="getNeighbors">Returns neighbors and the cost to move to them.</param>
    /// <param name="isGoal">Returns true if we reached the target.</param>
    /// <returns>The minimum cost to reach the goal, or -1.</returns>
    public static long Dijkstra<T>(
        T start,
        Func<T, IEnumerable<(T Node, long Cost)>> getNeighbors,
        Func<T, bool> isGoal) where T : notnull
    {
        // Elements are dequeued based on lowest priority value (cost).
        var queue = new PriorityQueue<T, long>();
        var minCosts = new Dictionary<T, long>();

        queue.Enqueue(start, 0);
        minCosts[start] = 0;

        while (queue.Count > 0)
        {
            if (!queue.TryDequeue(out T? current, out long currentCost))
            {
                break;
            }

            // Optimization: If we found a shorter way to get here already, skip.
            if (currentCost > minCosts.GetValueOrDefault(current, long.MaxValue))
            {
                continue;
            }

            if (isGoal(current))
            {
                return currentCost;
            }

            foreach (var (neighbor, stepCost) in getNeighbors(current))
            {
                var newCost = currentCost + stepCost;

                if (newCost >= minCosts.GetValueOrDefault(neighbor, long.MaxValue))
                {
                    continue;
                }

                minCosts[neighbor] = newCost;
                queue.Enqueue(neighbor, newCost);
            }
        }

        return -1;
    }
}