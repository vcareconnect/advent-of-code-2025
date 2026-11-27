using System.Text;

namespace AoC.Helpers.Helpers;

/// <summary>
/// A static helper class for reading and processing text input files.
/// Provides methods to parse files into various data structures like Lists, 2D Arrays, and custom types.
/// </summary>
public static class TextInputHelper
{
    /// <summary>
    /// Reads the entire file and returns a list of all characters found, excluding any characters specified in ignoredChars.
    /// Useful for processing a stream of characters regardless of line breaks.
    /// </summary>
    /// <param name="filePath">The relative or absolute path to the input file.</param>
    /// <param name="ignoredChars">Optional array of characters to skip (e.g., newlines '\n', '\r').</param>
    /// <returns>A single list containing all valid characters from the file.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file does not exist.</exception>
    public static List<char> ReadAsCharList(string filePath, char[]? ignoredChars = null)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var text = File.ReadAllText(filePath);
        var ignoreSet = ignoredChars != null ? new HashSet<char>(ignoredChars) : new HashSet<char>();

        return text.Where(c => !ignoreSet.Contains(c)).ToList();
    }

    /// <summary>
    /// Reads the file as a single stream of characters and groups them into lists based on a delimiter.
    /// Example: "A,B,C" with delimiter ',' becomes List containing ['A'], ['B'], ['C'].
    /// </summary>
    /// <param name="filePath">The path to the input file.</param>
    /// <param name="delimiter">The character that separates groups.</param>
    /// <param name="ignoredChars">Optional characters to skip.</param>
    /// <returns>A list of character lists, where each inner list represents a group separated by the delimiter.</returns>
    public static List<List<char>> ReadAsCharList(string filePath, char delimiter, char[]? ignoredChars = null)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var text = File.ReadAllText(filePath);
        var ignoreSet = ignoredChars != null ? new HashSet<char>(ignoredChars) : new HashSet<char>();

        var result = new List<List<char>>();
        var currentGroup = new List<char>();

        foreach (var c in text)
        {
            if (c == delimiter)
            {
                result.Add(currentGroup);
                currentGroup = [];
            }
            else if (!ignoreSet.Contains(c))
            {
                currentGroup.Add(c);
            }
        }

        result.Add(currentGroup);

        return result;
    }

    /// <summary>
    /// Reads the file line by line, treating each line as a list of characters.
    /// </summary>
    /// <param name="filePath">The path to the input file.</param>
    /// <param name="ignoredChars">Optional characters to skip.</param>
    /// <returns>A list of lists, where the outer list represents lines and the inner list contains characters of that line.</returns>
    public static List<List<char>> ReadLinesAsCharList(string filePath, char[]? ignoredChars = null)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var lines = File.ReadLines(filePath);
        var ignoreSet = ignoredChars != null ? new HashSet<char>(ignoredChars) : new HashSet<char>();

        return lines.Select(line => line.Where(c => !ignoreSet.Contains(c)).ToList()).ToList();
    }

    /// <summary>
    /// Reads the file line by line and converts it into a fixed-size 2D character array (char[,]).
    /// Useful for grid-based puzzles (e.g., mazes, maps).
    /// </summary>
    /// <param name="filePath">The path to the input file.</param>
    /// <param name="ignoredChars">Optional characters to skip.</param>
    /// <returns>A 2D array where [row, col] maps to the file's line and character position.</returns>
    public static char[,] ReadLinesAs2DCharArray(string filePath, char[]? ignoredChars = null)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var lines = File.ReadLines(filePath);
        var ignoreSet = ignoredChars != null ? new HashSet<char>(ignoredChars) : new HashSet<char>();

        var processedLines = lines
            .Select(line => line.Where(c => !ignoreSet.Contains(c)).ToArray())
            .ToList();

        if (processedLines.Count == 0)
        {
            return new char[0, 0];
        }

        var rows = processedLines.Count;
        var cols = processedLines.Max(l => l.Length);

        var result = new char[rows, cols];

        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < processedLines[r].Length; c++)
            {
                result[r, c] = processedLines[r][c];
            }
        }

        return result;
    }

    /// <summary>
    /// Reads the entire file content into a single string.
    /// </summary>
    /// <param name="filePath">The path to the input file.</param>
    /// <param name="ignoredChars">Optional characters to remove from the resulting string.</param>
    /// <returns>The full content of the file as a string.</returns>
    public static string ReadAsString(string filePath, char[]? ignoredChars = null)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var text = File.ReadAllText(filePath);
        if (ignoredChars == null || ignoredChars.Length == 0)
        {
            return text;
        }

        var ignoreSet = new HashSet<char>(ignoredChars);
        return new string(text.Where(c => !ignoreSet.Contains(c)).ToArray());
    }

    /// <summary>
    /// Reads the file as a single string and splits it into a list of substrings based on a delimiter.
    /// </summary>
    /// <param name="filePath">The path to the input file.</param>
    /// <param name="delimiter">The character that separates items.</param>
    /// <param name="ignoredChars">Optional characters to skip during processing.</param>
    /// <returns>A list of strings derived from splitting the file content.</returns>
    public static List<string> ReadAsStringList(string filePath, char delimiter, char[]? ignoredChars = null)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var text = File.ReadAllText(filePath);
        var ignoreSet = ignoredChars != null ? new HashSet<char>(ignoredChars) : new HashSet<char>();

        var result = new List<string>();
        var currentGroup = new StringBuilder();

        foreach (var c in text)
        {
            if (c == delimiter)
            {
                result.Add(currentGroup.ToString());
                currentGroup.Clear();
            }
            else if (!ignoreSet.Contains(c))
            {
                currentGroup.Append(c);
            }
        }

        result.Add(currentGroup.ToString());

        return result;
    }

    /// <summary>
    /// Reads the file line by line and returns a list of strings.
    /// </summary>
    /// <param name="filePath">The path to the input file.</param>
    /// <param name="ignoredChars">Optional characters to remove from each line.</param>
    /// <returns>A list containing each line of the file as a string.</returns>
    public static List<string> ReadLinesAsStringList(string filePath, char[]? ignoredChars = null)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var lines = File.ReadLines(filePath);
        if (ignoredChars == null || ignoredChars.Length == 0)
        {
            return lines.ToList();
        }

        var ignoreSet = new HashSet<char>(ignoredChars);
        return lines.Select(line => new string(line.Where(c => !ignoreSet.Contains(c)).ToArray())).ToList();
    }

    /// <summary>
    /// Reads the file line by line, splitting each line by a delimiter string to form a 2D string array.
    /// Useful for processing CSV-like data or space-separated values.
    /// </summary>
    /// <param name="filePath">The path to the input file.</param>
    /// <param name="delimiter">The string to split each line by.</param>
    /// <param name="ignoredChars">Optional characters to remove from the line *before* splitting.</param>
    /// <returns>A 2D string array where [row, col] contains the split value.</returns>
    public static string[,] ReadLinesAs2DStringArray(string filePath, string delimiter, char[]? ignoredChars = null)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var lines = File.ReadLines(filePath);
        var ignoreSet = ignoredChars != null ? new HashSet<char>(ignoredChars) : new HashSet<char>();

        var processedLines = lines
            .Select(line =>
            {
                var cleanLine = ignoreSet.Count > 0
                    ? new string(line.Where(c => !ignoreSet.Contains(c)).ToArray())
                    : line;
                return cleanLine.Split([delimiter], StringSplitOptions.None);
            })
            .ToList();

        if (processedLines.Count == 0)
        {
            return new string[0, 0];
        }

        var rows = processedLines.Count;
        var cols = processedLines.Max(l => l.Length);

        var result = new string[rows, cols];

        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < processedLines[r].Length; c++)
            {
                result[r, c] = processedLines[r][c];
            }
        }

        return result;
    }

    /// <summary>
    /// Reads the file line by line and converts each character into a custom type T using a converter function.
    /// Returns a 2D array of type T[,].
    /// Useful for converting a grid of digits directly into an int[,] array.
    /// </summary>
    /// <typeparam name="T">The target type for each cell.</typeparam>
    /// <param name="filePath">The path to the input file.</param>
    /// <param name="converter">A function that takes a character and returns an instance of T.</param>
    /// <param name="ignoredChars">Optional characters to skip.</param>
    /// <returns>A 2D array of type T.</returns>
    public static T[,] ReadLinesAs2DArray<T>(string filePath, Func<char, T> converter, char[]? ignoredChars = null)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var lines = File.ReadLines(filePath);
        var ignoreSet = ignoredChars != null ? new HashSet<char>(ignoredChars) : new HashSet<char>();

        var processedLines = lines
            .Select(line => line.Where(c => !ignoreSet.Contains(c)).Select(converter).ToArray())
            .ToList();

        if (processedLines.Count == 0)
        {
            return new T[0, 0];
        }

        var rows = processedLines.Count;
        var cols = processedLines.Max(l => l.Length);

        var result = new T[rows, cols];

        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < processedLines[r].Length; c++)
            {
                result[r, c] = processedLines[r][c];
            }
        }

        return result;
    }

    /// <summary>
    /// Reads the file line by line, splits each line by a delimiter, and converts the parts into a custom type T.
    /// Returns a 2D array of type T[,].
    /// </summary>
    /// <typeparam name="T">The target type for each cell.</typeparam>
    /// <param name="filePath">The path to the input file.</param>
    /// <param name="delimiter">The string to split each line by.</param>
    /// <param name="converter">A function that converts the string segment to T.</param>
    /// <param name="ignoredChars">Optional characters to remove from the line *before* splitting.</param>
    /// <returns>A 2D array of type T.</returns>
    public static T[,] ReadLinesAs2DArray<T>(string filePath, char delimiter, Func<string, T> converter,
        char[]? ignoredChars = null)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var lines = File.ReadLines(filePath);
        var ignoreSet = ignoredChars != null ? new HashSet<char>(ignoredChars) : new HashSet<char>();

        var processedLines = lines
            .Select(line =>
            {
                var cleanLine = ignoreSet.Count > 0
                    ? new string(line.Where(c => !ignoreSet.Contains(c)).ToArray())
                    : line;
                return cleanLine.Split([delimiter], StringSplitOptions.None)
                    .Select(converter)
                    .ToArray();
            })
            .ToList();

        if (processedLines.Count == 0)
        {
            return new T[0, 0];
        }

        var rows = processedLines.Count;
        var cols = processedLines.Max(l => l.Length);

        var result = new T[rows, cols];

        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < processedLines[r].Length; c++)
            {
                result[r, c] = processedLines[r][c];
            }
        }

        return result;
    }

    /// <summary>
    /// Reads the file line by line and converts each full line into a custom object of type T using a converter function.
    /// Returns a list of T objects.
    /// Useful for parsing lines that represent complex objects (e.g., "User: ID=5" -> UserObject).
    /// </summary>
    /// <typeparam name="T">The target object type.</typeparam>
    /// <param name="filePath">The path to the input file.</param>
    /// <param name="converter">A function that takes a line (string) and returns an instance of T.</param>
    /// <param name="ignoredChars">Optional characters to remove from the line before conversion.</param>
    /// <returns>A list of converted objects.</returns>
    public static List<T> ReadLinesAsList<T>(string filePath, Func<string, T> converter, char[]? ignoredChars = null)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var lines = File.ReadLines(filePath);
        var ignoreSet = ignoredChars != null ? new HashSet<char>(ignoredChars) : new HashSet<char>();

        return lines
            .Select(line =>
            {
                var cleanLine = ignoreSet.Count > 0
                    ? new string(line.Where(c => !ignoreSet.Contains(c)).ToArray())
                    : line;
                return converter(cleanLine);
            })
            .ToList();
    }

    /// <summary>
    /// Reads the file line by line, splitting each line by any whitespace, removing empty entries.
    /// Returns a 2D array of type T[,].
    /// </summary>
    public static T[,] ReadSpaceSeparated2DArray<T>(string filePath, Func<string, T> converter,
        char[]? ignoredChars = null)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var lines = File.ReadLines(filePath);
        var ignoreSet = ignoredChars != null ? new HashSet<char>(ignoredChars) : new HashSet<char>();

        var processedLines = lines
            .Select(line =>
            {
                var cleanLine = ignoreSet.Count > 0
                    ? new string(line.Where(c => !ignoreSet.Contains(c)).ToArray())
                    : line;
                return cleanLine.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries)
                    .Select(converter)
                    .ToArray();
            })
            .ToList();

        if (processedLines.Count == 0)
        {
            return new T[0, 0];
        }

        var rows = processedLines.Count;
        var cols = processedLines.Max(l => l.Length);

        var result = new T[rows, cols];

        for (var r = 0; r < rows; r++)
        {
            for (var c = 0; c < processedLines[r].Length; c++)
            {
                result[r, c] = processedLines[r][c];
            }
        }

        return result;
    }
    
    /// <summary>
    /// Reads the file as a fixed-width grid. Detects columns by finding vertical separators (indices where all lines have spaces).
    /// Returns a list of columns, where each column is a list of strings (rows) with their original padding preserved.
    /// </summary>
    public static List<List<string>> ReadFixedColumnGrid(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var lines = File.ReadAllLines(filePath).ToList();
        if (lines.Count == 0) return [];

        var maxLength = lines.Max(l => l.Length);
        
        // Pad all lines to the max length to ensure safe indexing
        var paddedLines = lines.Select(l => l.PadRight(maxLength)).ToList();

        // Detect separator indices (columns where ALL rows have a space)
        var isSeparator = new bool[maxLength];
        for (var c = 0; c < maxLength; c++)
        {
            isSeparator[c] = paddedLines.All(line => line[c] == ' ');
        }

        var columns = new List<List<string>>();
        var currentStart = -1;

        for (var c = 0; c < maxLength; c++)
        {
            if (!isSeparator[c])
            {
                // Start of a new column
                if (currentStart == -1)
                {
                    currentStart = c;
                }
            }
            else
            {
                if (currentStart == -1)
                {
                    continue;
                }

                // End of a column, extract it
                var length = c - currentStart;
                var colData = paddedLines.Select(line => line.Substring(currentStart, length)).ToList();
                columns.Add(colData);
                currentStart = -1;
            }
        }

        // last column doesn't read a seperator so we have to add manually at the end
        if (currentStart != -1)
        {
            var length = maxLength - currentStart;
            var colData = paddedLines.Select(line => line.Substring(currentStart, length)).ToList();
            columns.Add(colData);
        }

        return columns;
    }
}