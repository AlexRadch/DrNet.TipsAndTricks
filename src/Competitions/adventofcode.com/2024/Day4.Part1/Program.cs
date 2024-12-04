﻿
{
    var matrix = ReadMatrix(File.OpenText("input1.txt"));

    var result = Solve(matrix, "XMAS");

    Console.WriteLine($"{result}");
}

{
    var matrix = ReadMatrix(File.OpenText("input2.txt"));

    var result = Solve(matrix, "XMAS");

    Console.WriteLine($"{result}");
}

static string[] ReadMatrix(TextReader input) =>
    ReadRows(input).ToArray();

static IEnumerable<string> ReadRows(TextReader input)
{
    while (input.ReadLine() is string row && row.Length > 0)
        yield return row;
}

static int Solve<TMatrix, TString>(TMatrix matrix, TString word)
    where TMatrix: IList<string>
    where TString: IEnumerable<char>
{
    var c = word.First();
    var subWord = word.Skip(1);

    var starts = Enumerable.Range(0, matrix.Count)
            .SelectMany(row => Enumerable.Range(0, matrix[row].Length).Select(column => (Row: row, Column: column)));
    starts = starts.Where(pos => matrix[pos.Row][pos.Column] == c);

    var counts = starts.Select(pos => SolveFromPos(matrix, pos.Row, pos.Column, subWord));
    return counts.Sum();
}

static int SolveFromPos<TMatrix, TString>(TMatrix matrix, int row, int column, TString word)
    where TMatrix : IList<string>
    where TString : IEnumerable<char>
{
    int result = 0;
    foreach(var (rowDelta, columnDelta) in Deltas)
    {
        var nextRow = row;
        var nextColumn = column;
        foreach (var c in word)
        {
            nextRow += rowDelta;
            nextColumn += columnDelta;
            if (!(nextRow >= 0 && nextRow < matrix.Count && nextColumn >= 0 && nextColumn < matrix[nextRow].Length))
                goto Next;
            if (matrix[nextRow][nextColumn] != c)
                goto Next;

        }
        result++;
    Next:
        { }
    }

    return result;
}

partial class Program
{
    static readonly (int Row, int Column)[] Deltas =
        [(-1, -1), (-1, 0), (-1, 1),
         (0, -1),           (0, 1),
         (1, -1),  (1, 0),  (1, 1),];
}
