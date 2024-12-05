
{
    var matrix = ReadMatrix(File.OpenText("input1.txt"));

    var result = Solve(matrix, "MAS");

    Console.WriteLine($"{result}");
}

{
    var matrix = ReadMatrix(File.OpenText("input2.txt"));

    var result = Solve(matrix, "MAS");

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
    where TMatrix : IList<string>
    where TString : IEnumerable<char>
{
    var starts = Enumerable.Range(0, matrix.Count)
            .SelectMany(row => Enumerable.Range(0, matrix[0].Length).Select(column => (Row: row, Column: column)));

    var counts = starts.Select(pos => SolveFromPos(matrix, pos.Row, pos.Column, word));
    return counts.Sum();
}

static int SolveFromPos<TMatrix, TString>(TMatrix matrix, int row, int column, TString word)
    where TMatrix : IList<string>
    where TString : IEnumerable<char>
{
    int result = 0;
    foreach (var positions in PositionsCombinations)
    {
        foreach (var (Start, Direction) in positions)
        {
            var nextRow = row + Start.Row;
            var nextColumn = column + Start.Column;
            foreach (var c in word)
            {
                if (!(nextRow >= 0 && nextRow < matrix.Count && nextColumn >= 0 && nextColumn < matrix[nextRow].Length))
                    goto Next;
                if (matrix[nextRow][nextColumn] != c)
                    goto Next;
                nextRow += Direction.Row;
                nextColumn += Direction.Column;
            }
        }
        result++;
    Next:
        { }
    }

    return result;
}

partial class Program
{
    static readonly ((int Row, int Column) Start, (int Row, int Column) Direction)[][] PositionsCombinations =
        [
            [((-1, -1), (1, 1)), ((-1, 1), (1, -1))], // Positions
            [((-1, -1), (1, 1)), ((1, -1), (-1, 1))], // Positions
            [((1, 1), (-1, -1)), ((-1, 1), (1, -1))], // Positions
            [((1, 1), (-1, -1)), ((1, -1), (-1, 1))], // Positions
        ];
}
