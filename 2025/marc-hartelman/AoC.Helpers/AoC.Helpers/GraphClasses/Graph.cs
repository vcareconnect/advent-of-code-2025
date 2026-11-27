namespace AoC.Helpers.GraphClasses;

/// <summary>
/// Represents a generic Graph data structure.
/// A graph is a collection of nodes (vertices) connected by edges.
/// </summary>
/// <typeparam name="T">The type of data stored in the graph nodes. Must be non-null.</typeparam>
public class Graph<T> where T : notnull
{
    /// <summary>
    /// The list of all nodes currently in the graph.
    /// </summary>
    public List<GraphNode<T>> Nodes { get; } = [];

    /// <summary>
    /// Adds a new node to the graph if it doesn't already exist.
    /// </summary>
    /// <param name="value">The value to store in the node.</param>
    /// <returns>The existing node if found, otherwise the newly created node.</returns>
    public GraphNode<T> AddNode(T value)
    {
        var existing = Nodes.FirstOrDefault(n => n.Value.Equals(value));
        if (existing != null)
        {
            return existing;
        }

        var node = new GraphNode<T>(value);
        Nodes.Add(node);
        return node;
    }

    /// <summary>
    /// Creates a connection (edge) between two nodes.
    /// </summary>
    /// <param name="from">The value of the starting node.</param>
    /// <param name="to">The value of the destination node.</param>
    /// <param name="weight">The cost/distance of this connection. Defaults to 1.</param>
    /// <param name="isDirected">
    /// If true, creates a one-way connection (From one way To). 
    /// If false, creates a two-way connection (From two way To).
    /// Defaults to true.
    /// </param>
    public void AddEdge(T from, T to, int weight = 1, bool isDirected = true)
    {
        var fromNode = AddNode(from);
        var toNode = AddNode(to);

        fromNode.AddEdge(toNode, weight);

        if (!isDirected)
        {
            toNode.AddEdge(fromNode, weight);
        }
    }

    /// <summary>
    /// Performs a Breadth-First Search (BFS) starting from the specified node.
    /// BFS explores the graph layer by layer (closest neighbors first).
    /// Useful for finding the shortest path in an unweighted graph.
    /// </summary>
    /// <param name="startValue">The node to start the search from.</param>
    /// <returns>A list of nodes in the order they were visited.</returns>
    public List<T> BreadthFirstSearch(T startValue)
    {
        var startNode = Nodes.FirstOrDefault(n => n.Value.Equals(startValue));
        if (startNode == null)
        {
            return [];
        }

        var visited = new HashSet<T>();
        var queue = new Queue<GraphNode<T>>();
        var result = new List<T>();

        visited.Add(startNode.Value);
        queue.Enqueue(startNode);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current.Value);

            foreach (var edge in current.Edges.Where(edge => !visited.Contains(edge.To.Value)))
            {
                visited.Add(edge.To.Value);
                queue.Enqueue(edge.To);
            }
        }

        return result;
    }

    /// <summary>
    /// Performs a Depth-First Search (DFS) starting from the specified node.
    /// DFS explores as far as possible along each branch before backtracking.
    /// Useful for traversing the entire graph or finding cycles.
    /// </summary>
    /// <param name="startValue">The node to start the search from.</param>
    /// <returns>A list of nodes in the order they were visited.</returns>
    public List<T> DepthFirstSearch(T startValue)
    {
        var startNode = Nodes.FirstOrDefault(n => n.Value.Equals(startValue));
        if (startNode == null)
        {
            return [];
        }

        var visited = new HashSet<T>();
        var result = new List<T>();
        var stack = new Stack<GraphNode<T>>();

        stack.Push(startNode);

        while (stack.Count > 0)
        {
            var current = stack.Pop();

            if (!visited.Add(current.Value))
            {
                continue;
            }

            result.Add(current.Value);

            // Push neighbors to stack (reversed to maintain natural order if desired)
            // Reversing is optional but helps process neighbors in stored order
            for (var i = current.Edges.Count - 1; i >= 0; i--)
            {
                var neighbor = current.Edges[i].To;
                if (!visited.Contains(neighbor.Value))
                {
                    stack.Push(neighbor);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Finds the shortest path between two nodes using Dijkstra's algorithm.
    /// Useful for finding the path with the minimum total weight (cost/distance).
    /// Guaranteed to find the shortest path if weights are non-negative.
    /// </summary>
    /// <param name="startValue">The starting node value.</param>
    /// <param name="endValue">The target node value.</param>
    /// <returns>
    /// A tuple containing:
    /// - Path: A list of values representing the path from start to end.
    /// - Distance: The total weight/cost of the path. Returns -1 if no path exists.
    /// </returns>
    public (List<T> Path, int Distance) GetShortestPathDijkstra(T startValue, T endValue)
    {
        var startNode = Nodes.FirstOrDefault(n => n.Value.Equals(startValue));
        var endNode = Nodes.FirstOrDefault(n => n.Value.Equals(endValue));

        if (startNode == null || endNode == null)
        {
            return ([], -1);
        }

        var distances = new Dictionary<GraphNode<T>, int>();
        var previous = new Dictionary<GraphNode<T>, GraphNode<T>>();
        var unvisited = new PriorityQueue<GraphNode<T>, int>();

        foreach (var node in Nodes)
        {
            distances[node] = int.MaxValue;
        }

        distances[startNode] = 0;
        unvisited.Enqueue(startNode, 0);

        while (unvisited.Count > 0)
        {
            var current = unvisited.Dequeue();

            if (current == endNode)
            {
                break;
            }

            // Note: PriorityQueue might contain duplicate entries for the same node with different priorities
            // Check if we found a shorter path already
            if (current != startNode && distances[current] == int.MaxValue)
            {
                continue;
            }

            foreach (var edge in current.Edges)
            {
                var alt = distances[current] + edge.Weight;
                if (alt >= distances[edge.To])
                {
                    continue;
                }

                distances[edge.To] = alt;
                previous[edge.To] = current;
                unvisited.Enqueue(edge.To, alt);
            }
        }

        if (distances[endNode] == int.MaxValue)
        {
            return ([], -1); // Path not found
        }

        // Reconstruct path
        var path = new List<T>();
        var curr = endNode;
        while (curr != null)
        {
            path.Add(curr.Value);
            previous.TryGetValue(curr, out curr);
        }

        path.Reverse();

        return (path, distances[endNode]);
    }

    /// <summary>
    /// Finds the shortest path between two nodes using the A* (A-Star) algorithm.
    /// A* is generally faster than Dijkstra for spatial graphs because it uses a "heuristic" 
    /// to estimate which path is more promising (moves closer to the goal).
    /// </summary>
    /// <param name="startValue">The starting node value.</param>
    /// <param name="endValue">The target node value.</param>
    /// <param name="heuristic">
    /// A function that estimates the cost from a given node to the goal.
    /// Example for a grid: (node) => Math.Abs(node.x - target.x) + Math.Abs(node.y - target.y).
    /// Important: This function must never overestimate the true cost.
    /// </param>
    /// <returns>
    /// A tuple containing:
    /// - Path: A list of values representing the path from start to end.
    /// - Distance: The total weight/cost of the path. Returns -1 if no path exists.
    /// </returns>
    public (List<T> Path, int Distance) GetShortestPathAStar(T startValue, T endValue, Func<T, int> heuristic)
    {
        var startNode = Nodes.FirstOrDefault(n => n.Value.Equals(startValue));
        var endNode = Nodes.FirstOrDefault(n => n.Value.Equals(endValue));

        if (startNode == null || endNode == null)
        {
            return ([], -1);
        }

        // gScore: Cost from start to current node
        var gScore = new Dictionary<GraphNode<T>, int>();

        // fScore: Estimated total cost (gScore + heuristic)
        // We don't strictly need to store fScore separately if we calculate it for the PriorityQueue, 
        // but keeping gScore is essential for the algorithm.

        var previous = new Dictionary<GraphNode<T>, GraphNode<T>>();
        var openSet = new PriorityQueue<GraphNode<T>, int>();

        // Initialize gScores to infinity
        foreach (var node in Nodes)
        {
            gScore[node] = int.MaxValue;
        }

        gScore[startNode] = 0;
        // Priority is fScore = gScore + h(start)
        openSet.Enqueue(startNode, heuristic(startNode.Value));

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();

            if (current == endNode)
            {
                // Reconstruct path
                var path = new List<T>();
                var curr = endNode;
                while (curr != null)
                {
                    path.Add(curr.Value);
                    previous.TryGetValue(curr, out curr);
                }

                path.Reverse();
                return (path, gScore[endNode]);
            }

            // If we found a shorter path to 'current' already processed, we might skip (standard optimization)
            // However, standard A* doesn't require closed set if using consistent heuristic, 
            // but checking gScore validity prevents reprocessing effectively.

            foreach (var edge in current.Edges)
            {
                var tentativeGScore = gScore[current] + edge.Weight;

                if (tentativeGScore >= gScore[edge.To])
                {
                    continue;
                }

                gScore[edge.To] = tentativeGScore;
                var fScore = tentativeGScore + heuristic(edge.To.Value);

                previous[edge.To] = current;
                openSet.Enqueue(edge.To, fScore);
            }
        }

        return ([], -1); // Path not found
    }
}