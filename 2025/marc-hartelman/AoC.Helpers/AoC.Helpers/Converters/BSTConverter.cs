using AoC.Helpers.BST;
using AoC.Helpers.Helpers;

namespace AoC.Helpers.Converters;

public static class BSTConverter
{
    /// <summary>
    /// Creates a BST <see cref="string"/> from lines in a file.
    /// </summary>
    public static BinarySearchTree<string> CreateFromLines(string filePath, char[]? ignoredChars = null)
    {
        var lines = TextInputHelper.ReadLinesAsStringList(filePath, ignoredChars);
        var bst = new BinarySearchTree<string>();
        
        foreach (var line in lines.Where(line => !string.IsNullOrEmpty(line)))
        {
            bst.Insert(line);
        }

        return bst;
    }

    /// <summary>
    /// Creates a BST generic class <see cref="T"/> from lines in a file using a custom parser (e.g., int.Parse).
    /// </summary>
    public static BinarySearchTree<T> CreateFromLines<T>(string filePath, Func<string, T> parser, char[]? ignoredChars = null) where T : IComparable<T>
    {
        var lines = TextInputHelper.ReadLinesAsStringList(filePath, ignoredChars);
        var bst = new BinarySearchTree<T>();
        
        foreach (var line in lines.Where(line => !string.IsNullOrEmpty(line)))
        {
            bst.Insert(parser(line));
        }

        return bst;
    }

    /// <summary>
    /// Creates a BST <see cref="string"/> by splitting file content by a delimiter.
    /// </summary>
    public static BinarySearchTree<string> CreateFromDelimitedText(string filePath, char delimiter, char[]? ignoredChars = null)
    {
        var items = TextInputHelper.ReadAsStringList(filePath, delimiter, ignoredChars);
        var bst = new BinarySearchTree<string>();

        foreach (var item in items)
        {
            if (!string.IsNullOrEmpty(item))
            {
                bst.Insert(item);
            }
        }

        return bst;
    }

    /// <summary>
    /// Creates a BST <see cref="T"/> by splitting file content by a delimiter and parsing to T.
    /// </summary>
    public static BinarySearchTree<T> CreateFromDelimitedText<T>(string filePath, char delimiter, Func<string, T> parser, char[]? ignoredChars = null) where T : IComparable<T>
    {
        var items = TextInputHelper.ReadAsStringList(filePath, delimiter, ignoredChars);
        var bst = new BinarySearchTree<T>();

        foreach (var item in items.Where(item => !string.IsNullOrEmpty(item)))
        {
            bst.Insert(parser(item));
        }

        return bst;
    }

    /// <summary>
    /// Creates a BST <see cref="char"/> from all characters in the file.
    /// </summary>
    public static BinarySearchTree<char> CreateFromChars(string filePath, char[]? ignoredChars = null)
    {
        var chars = TextInputHelper.ReadAsCharList(filePath, ignoredChars);
        var bst = new BinarySearchTree<char>();

        foreach (var c in chars)
        {
            bst.Insert(c);
        }

        return bst;
    }
    
    /// <summary>
    /// Creates a BST <see cref="T"/> from lines in a file using the generic list reader.
    /// This is useful when the logic for parsing each line into T is handled by a converter passed to ReadLinesAsList.
    /// </summary>
    public static BinarySearchTree<T> CreateFromGenericList<T>(string filePath, Func<string, T> converter, char[]? ignoredChars = null) where T : IComparable<T>
    {
        // Use the generic list reader from TextInputHelper
        var items = TextInputHelper.ReadLinesAsList(filePath, converter, ignoredChars);
        var bst = new BinarySearchTree<T>();

        foreach (var item in items.OfType<T>())
        {
            bst.Insert(item);
        }

        return bst;
    }
}