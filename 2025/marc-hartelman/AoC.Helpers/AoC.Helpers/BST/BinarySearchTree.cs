namespace AoC.Helpers.BST;

/// <summary>
/// Represents a generic Binary Search Tree (BST) data structure.
/// </summary>
/// <typeparam name="T">The type of data stored in the tree nodes. Must implement IComparable.</typeparam>
public class BinarySearchTree<T> where T : IComparable<T>
{
    public Node<T>? Root { get; private set; }

    /// <summary>
    /// Inserts a new value into the Binary Search Tree.
    /// </summary>
    /// <param name="data">The data value to insert.</param>
    public void Insert(T data)
    {
        Root = InsertRec(Root, data);
    }

    private Node<T> InsertRec(Node<T>? root, T data)
    {
        if (root == null)
        {
            return new Node<T>(data);
        }

        switch (data.CompareTo(root.Data))
        {
            case < 0:
                root.Left = InsertRec(root.Left, data);
                break;
            case > 0:
                root.Right = InsertRec(root.Right, data);
                break;
        }

        return root;
    }

    /// <summary>
    /// Checks if a specific value exists in the tree.
    /// </summary>
    /// <param name="data">The value to search for.</param>
    /// <returns>True if the value is found; otherwise, false.</returns>
    public bool Contains(T data)
    {
        return ContainsRec(Root, data);
    }

    private bool ContainsRec(Node<T>? root, T data)
    {
        if (root == null)
        {
            return false;
        }

        var comparison = data.CompareTo(root.Data);
        return comparison switch
        {
            0 => true,
            < 0 => ContainsRec(root.Left, data),
            _ => ContainsRec(root.Right, data)
        };
    }

    /// <summary>
    /// Performs an in-order traversal of the tree (Left, Root, Right).
    /// Returns values in sorted ascending order.
    /// </summary>
    /// <returns>A list of values sorted in ascending order.</returns>
    public List<T> InOrderTraversal()
    {
        var result = new List<T>();
        InOrderRec(Root, result);
        return result;
    }

    private void InOrderRec(Node<T>? root, List<T> result)
    {
        if (root == null)
        {
            return;
        }

        InOrderRec(root.Left, result);
        result.Add(root.Data);
        InOrderRec(root.Right, result);
    }

    /// <summary>
    /// Removes a specific value from the tree.
    /// </summary>
    /// <param name="data">The value to delete.</param>
    public void Delete(T data)
    {
        Root = DeleteRec(Root, data);
    }

    private Node<T>? DeleteRec(Node<T>? root, T data)
    {
        if (root == null)
        {
            return root;
        }

        var comparison = data.CompareTo(root.Data);

        switch (comparison)
        {
            case < 0:
                root.Left = DeleteRec(root.Left, data);
                break;
            case > 0:
                root.Right = DeleteRec(root.Right, data);
                break;
            default:
            {
                // node with only one child or no child
                if (root.Left == null)
                {
                    return root.Right;
                }

                if (root.Right == null)
                {
                    return root.Left;
                }

                // node with two children: Get the inorder successor (smallest in the right subtree)
                root.Data = MinValue(root.Right);

                // Delete the inorder successor
                root.Right = DeleteRec(root.Right, root.Data);
                break;
            }
        }

        return root;
    }

    private T MinValue(Node<T> root)
    {
        var minv = root.Data;
        while (root.Left != null)
        {
            minv = root.Left.Data;
            root = root.Left;
        }

        return minv;
    }

    /// <summary>
    /// Finds the minimum value currently stored in the tree.
    /// </summary>
    /// <returns>The minimum value.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the tree is empty.</exception>
    public T FindMin()
    {
        return Root == null ? throw new InvalidOperationException("Tree is empty") : MinValue(Root);
    }

    /// <summary>
    /// Finds the maximum value currently stored in the tree.
    /// </summary>
    /// <returns>The maximum value.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the tree is empty.</exception>
    public T FindMax()
    {
        if (Root == null)
        {
            throw new InvalidOperationException("Tree is empty");
        }

        var current = Root;
        while (current.Right != null)
        {
            current = current.Right;
        }
        return current.Data;
    }

    /// <summary>
    /// Calculates the height (maximum depth) of the tree.
    /// </summary>
    /// <returns>The height of the tree (0 for empty tree, 1 for root only).</returns>
    public int GetHeight()
    {
        return GetHeightRec(Root);
    }

    private int GetHeightRec(Node<T>? root)
    {
        if (root == null)
        {
            return 0;
        }

        var leftHeight = GetHeightRec(root.Left);
        var rightHeight = GetHeightRec(root.Right);
        return Math.Max(leftHeight, rightHeight) + 1;
    }

    /// <summary>
    /// Performs a pre-order traversal of the tree (Root, Left, Right).
    /// </summary>
    /// <returns>A list of values in pre-order sequence.</returns>
    public List<T> PreOrderTraversal()
    {
        var result = new List<T>();
        PreOrderRec(Root, result);
        return result;
    }

    private void PreOrderRec(Node<T>? root, List<T> result)
    {
        if (root == null)
        {
            return;
        }

        result.Add(root.Data);
        PreOrderRec(root.Left, result);
        PreOrderRec(root.Right, result);
    }

    /// <summary>
    /// Performs a post-order traversal of the tree (Left, Right, Root).
    /// </summary>
    /// <returns>A list of values in post-order sequence.</returns>
    public List<T> PostOrderTraversal()
    {
        var result = new List<T>();
        PostOrderRec(Root, result);
        return result;
    }

    private void PostOrderRec(Node<T>? root, List<T> result)
    {
        if (root == null)
        {
            return;
        }

        PostOrderRec(root.Left, result);
        PostOrderRec(root.Right, result);
        result.Add(root.Data);
    }

    /// <summary>
    /// Performs a level-order traversal (Breadth-First Search) of the tree.
    /// </summary>
    /// <returns>A list of values visited level by level, from top to bottom, left to right.</returns>
    public List<T> LevelOrderTraversal()
    {
        var result = new List<T>();
        if (Root == null)
        {
            return result;
        }

        var queue = new Queue<Node<T>>();
        queue.Enqueue(Root);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current.Data);

            if (current.Left != null)
            {
                queue.Enqueue(current.Left);
            }

            if (current.Right != null)
            {
                queue.Enqueue(current.Right);
            }
        }

        return result;
    }
}