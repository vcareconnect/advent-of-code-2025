using AoC.Helpers.GraphClasses;
using AoC.Helpers.Helpers;

namespace AoC.Helpers.Converters;

public class GraphConverter
{
    /// <summary>
    /// Creates a graph where nodes are (Row, Col) coordinates from a 2D grid file.
    /// Edges are created between adjacent cells (Up, Down, Left, Right).
    /// </summary>
    /// <param name="filePath">Path to the grid file.</param>
    /// <param name="isWalkable">Optional function to determine if a character represents a valid node. Defaults to all characters being valid.</param>
    /// <param name="allowDiagonals">If true, adds edges for diagonal neighbors (8 connections total). Defaults to false (4 connections).</param>
    /// <param name="getWeight">Optional function to calculate edge weight based on the characters of the two nodes (from, to). Defaults to 1.</param>
    /// <param name="ignoredChars">Optional characters to strip before processing.</param>
    public static Graph<(int Row, int Col)> CreateGridGraph(
        string filePath, 
        Func<char, bool>? isWalkable = null, 
        bool allowDiagonals = false,
        Func<char, char, int>? getWeight = null,
        char[]? ignoredChars = null)
    {
        var grid = TextInputHelper.ReadLinesAs2DCharArray(filePath, ignoredChars);
        var graph = new Graph<(int Row, int Col)>();
        var rows = grid.GetLength(0);
        var cols = grid.GetLength(1);

        isWalkable ??= _ => true;
        getWeight ??= (_, _) => 1;

        // Directions: Up, Down, Left, Right
        int[] dr = allowDiagonals 
            ? [-1, -1, -1, 0, 0, 1, 1, 1] 
            : [-1, 1, 0, 0];
        int[] dc = allowDiagonals 
            ? [-1, 0, 1, -1, 1, -1, 0, 1] 
            : [0, 0, -1, 1];

        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < cols; c++)
            {
                var fromVal = grid[r, c];
                if (!isWalkable(fromVal))
                {
                    continue;
                }

                // Ensure the node exists in the graph even if it ends up having no edges
                graph.AddNode((r, c));

                for (var i = 0; i < dr.Length; i++)
                {
                    var nr = r + dr[i];
                    var nc = c + dc[i];

                    if (nr < 0 || nr >= rows || nc < 0 || nc >= cols)
                    {
                        continue;
                    }

                    var toVal = grid[nr, nc];
                    if (!isWalkable(toVal))
                    {
                        continue;
                    }

                    var weight = getWeight(fromVal, toVal);
                    // Use directed edges to fully represent the grid connectivity
                    // (e.g., one might be able to go A->B but not B->A due to slopes)
                    graph.AddEdge((r, c), (nr, nc), weight, isDirected: true);
                }
            }
        }

        return graph;
    }

    /// <summary>
    /// Creates a graph from a file containing line-separated connections (e.g., "A-B", "Start -> End").
    /// Uses the specified delimiter to split lines into node names.
    /// Supports chains of nodes in a single line (e.g., "A-B-C" creates edges A-B and B-C).
    /// </summary>
    /// <param name="filePath">Path to the file.</param>
    /// <param name="delimiter">The string delimiter separating node names (e.g., "-", " -> ").</param>
    /// <param name="isDirected">Whether the edges are directed. Defaults to false.</param>
    /// <param name="ignoredChars">Optional characters to strip before processing.</param>
    public static Graph<string> CreateFromConnections(
        string filePath, 
        string delimiter, 
        bool isDirected = false, 
        char[]? ignoredChars = null)
    {
        // ReadLinesAs2DStringArray splits each line by the delimiter into columns
        var connections = TextInputHelper.ReadLinesAs2DStringArray(filePath, delimiter, ignoredChars);
        var graph = new Graph<string>();
        var rows = connections.GetLength(0);

        for (var i = 0; i < rows; i++)
        {
            // Iterate through the parts of the line to support chains like A-B-C
            for (var j = 0; j < connections.GetLength(1) - 1; j++)
            {
                var from = connections[i, j];
                var to = connections[i, j + 1];

                if (!string.IsNullOrWhiteSpace(from) && !string.IsNullOrWhiteSpace(to))
                {
                    graph.AddEdge(from.Trim(), to.Trim(), 1, isDirected);
                }
            }
        }

        return graph;
    }
    
    /// <summary>
    /// Creates a graph from a generic 2D grid (e.g., integers for a heightmap).
    /// Uses ReadLinesAs2DArray to parse characters into type TValue before building the graph.
    /// </summary>
    /// <typeparam name="TValue">The type of value in the grid cells (e.g., int).</typeparam>
    public static Graph<(int Row, int Col)> CreateGridGraph<TValue>(
        string filePath,
        Func<char, TValue> converter,
        Func<TValue, bool>? isWalkable = null,
        bool allowDiagonals = false,
        Func<TValue, TValue, int>? getWeight = null,
        char[]? ignoredChars = null)
    {
        // Uses the generic 2D array reader from TextInputHelper
        var grid = TextInputHelper.ReadLinesAs2DArray(filePath, converter, ignoredChars);
        var graph = new Graph<(int Row, int Col)>();
        var rows = grid.GetLength(0);
        var cols = grid.GetLength(1);

        isWalkable ??= _ => true;
        getWeight ??= (_, _) => 1;

        int[] dr = allowDiagonals 
            ? [-1, -1, -1, 0, 0, 1, 1, 1] 
            : [-1, 1, 0, 0];
        int[] dc = allowDiagonals 
            ? [-1, 0, 1, -1, 1, -1, 0, 1] 
            : [0, 0, -1, 1];

        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < cols; c++)
            {
                var fromVal = grid[r, c];
                if (!isWalkable(fromVal)) continue;

                graph.AddNode((r, c));

                for (var i = 0; i < dr.Length; i++)
                {
                    var nr = r + dr[i];
                    var nc = c + dc[i];

                    if (nr < 0 || nr >= rows || nc < 0 || nc >= cols)
                    {
                        continue;
                    }

                    var toVal = grid[nr, nc];
                    if (!isWalkable(toVal))
                    {
                        continue;
                    }

                    var weight = getWeight(fromVal, toVal);
                    graph.AddEdge((r, c), (nr, nc), weight, isDirected: true);
                }
            }
        }

        return graph;
    }

    /// <summary>
    /// Creates a graph from an adjacency list file (e.g., "NodeA: NodeB NodeC").
    /// Uses ReadLinesAsStringList to process line-by-line.
    /// </summary>
    /// <param name="filePath">Path to the file.</param>
    /// <param name="nodeNameDelimiter">The delimiter separating the root node from its neighbors (e.g., ":").</param>
    /// <param name="neighborDelimiter">The delimiter separating neighbors from each other (e.g., " " or ", ").</param>
    /// <param name="isDirected">Whether the edges are directed. Defaults to false.</param>
    /// <param name="ignoredChars">Optional characters to strip before processing.</param>
    public static Graph<string> CreateFromAdjacencyList(
        string filePath,
        string nodeNameDelimiter = ":",
        string neighborDelimiter = " ",
        bool isDirected = true,
        char[]? ignoredChars = null)
    {
        var lines = TextInputHelper.ReadLinesAsStringList(filePath, ignoredChars);
        var graph = new Graph<string>();

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            // Split "Node: Neighbor1 Neighbor2" into ["Node", "Neighbor1 Neighbor2"]
            var parts = line.Split([nodeNameDelimiter], 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 1)
            {
                continue;
            }

            var fromNode = parts[0].Trim();
            
            // If there are neighbors defined
            if (parts.Length > 1)
            {
                var neighbors = parts[1].Split([neighborDelimiter], StringSplitOptions.RemoveEmptyEntries);
                foreach (var neighbor in neighbors)
                {
                    graph.AddEdge(fromNode, neighbor.Trim(), 1, isDirected);
                }
            }
            else
            {
                // Ensure node is added even if it has no outgoing edges
                graph.AddNode(fromNode);
            }
        }

        return graph;
    }
    
    /// <summary>
    /// Creates a graph from a file where each line represents a connection or node info, parsed into a generic object TItem.
    /// This is useful for complex input formats where lines need custom parsing logic.
    /// </summary>
    /// <typeparam name="TNode">The type of the graph nodes (e.g., string or int).</typeparam>
    /// <typeparam name="TItem">The type of object parsed from each line.</typeparam>
    /// <param name="filePath">Path to the file.</param>
    /// <param name="lineParser">Function to parse a raw line string into a TItem object.</param>
    /// <param name="getEdgeInfo">Function that takes a TItem and returns the edge details: (FromNode, ToNode, Weight).</param>
    /// <param name="isDirected">Whether the edges are directed. Defaults to true.</param>
    /// <param name="ignoredChars">Optional characters to strip before processing.</param>
    public static Graph<TNode> CreateFromGenericList<TNode, TItem>(
        string filePath,
        Func<string, TItem> lineParser,
        Func<TItem, (TNode From, TNode To, int Weight)> getEdgeInfo,
        bool isDirected = true,
        char[]? ignoredChars = null) where TNode : notnull
    {
        // Use the existing helper to get a list of TItem objects
        var items = TextInputHelper.ReadLinesAsList(filePath, lineParser, ignoredChars);
        var graph = new Graph<TNode>();

        foreach (var item in items.OfType<TItem>())
        {
            var (from, to, weight) = getEdgeInfo(item);
            
            // Only add edge if valid nodes are returned (generic check for nulls if TNode is reference type)
            if (from != null && to != null)
            {
                graph.AddEdge(from, to, weight, isDirected);
            }
        }

        return graph;
    }
}