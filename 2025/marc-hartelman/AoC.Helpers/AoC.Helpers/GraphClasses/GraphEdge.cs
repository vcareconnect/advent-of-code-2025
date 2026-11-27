namespace AoC.Helpers.GraphClasses;

public class GraphEdge<T> where T : notnull
{
    public GraphNode<T> From { get; }
    public GraphNode<T> To { get; }
    public int Weight { get; }

    public GraphEdge(GraphNode<T> from, GraphNode<T> to, int weight)
    {
        From = from;
        To = to;
        Weight = weight;
    }

    public override string ToString() => $"{From} -> {To} ({Weight})";
}