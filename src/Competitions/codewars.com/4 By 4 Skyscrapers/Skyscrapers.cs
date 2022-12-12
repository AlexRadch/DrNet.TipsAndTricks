using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

// 4 By 4 Skyscrapers
// https://www.codewars.com/kata/5671d975d81d6c1c87000022
public class Skyscrapers
{
    public static int[][] SolvePuzzle(int[] clues)
    {
        var rowsBounds = new Bounds[4];
        for (var i = 0; i < N; ++i)
            rowsBounds[i] = new Bounds(clues[15 - i], clues[4 + i]);

        var colsBounds = new Bounds[N];
        for (var i = 0; i < N; ++i)
            colsBounds[i] = new Bounds(clues[0 + i], clues[11 - i]);

        return new Skyscrapers().Solve(rowsBounds, colsBounds);
    }

    const int N = 4;
    readonly IReadOnlyCollection<Line> allLines;

    readonly List<Line>[] rowsLines;
    readonly List<Line>[] colsLines;
    readonly CellsLine[] cells;

    public Skyscrapers()
    {
        allLines = GenerateLines(N).ToList();

        rowsLines = new List<Line>[N];
        colsLines = new List<Line>[N];

        cells = new CellsLine[N];
    }

    public int[][] Solve(Bounds[] rowsBounds, Bounds[] colsBounds)
    {
        for (var i = 0; i < N; ++i)
        {
            rowsLines[i] = new List<Line>();
            colsLines[i] = new List<Line>();

            cells[i] = new CellsLine(N);
        }

        foreach (var line in allLines)
        {
            for (var i = 0; i < N; ++i)
            {
                if ((rowsBounds[i].Left <= 0 || rowsBounds[i].Left == line.LeftView) && (rowsBounds[i].Right <= 0 || rowsBounds[i].Right == line.RightView))
                    AddRowLine(i, line);
                if ((colsBounds[i].Left <= 0 || colsBounds[i].Left == line.LeftView) && (colsBounds[i].Right <= 0 || colsBounds[i].Right == line.RightView))
                    AddColLine(i, line);
            }
        }

        var needSolve = true;
        while (needSolve)
        {
            needSolve = false;
            for (var row = 0; row < N; ++row)
            {
                var rowCells = cells[row];

                for (var col = 0; col < N; ++col)
                {
                    var cell = rowCells[col];
                    if (cell.Solved)
                        continue;

                    while (cell.Updated)
                    {
                        cell.Updated = false;
                        for (var skyscraper = 1; skyscraper <= N; skyscraper++)
                        {
                            if (cell.RowCounts(skyscraper) == 0 && cell.ColCounts(skyscraper) > 0)
                            {
                                RemoveColsWithSkyscraper(row, col, skyscraper);
                                needSolve = true;
                            }
                            if (cell.ColCounts(skyscraper) == 0 && cell.RowCounts(skyscraper) > 0)
                            { 
                                RemoveRowsWithSkyscraper(row, col, skyscraper);
                                needSolve = true;
                            }
                        }
                    }
                }
            }
        }

        var result = new int[N][];
        for (var r = 0; r < N; ++r)
        {
            Debug.Assert(rowsLines[r].Count == 1);
            result[r] = rowsLines[r][0].Skyscrapers.ToArray();
        }
        return result;
    }

    private void AddRowLine(int row , Line line)
    {
        rowsLines[row].Add(line);

        var cellsRow = cells[row];
        for (var col = 0; col < N; ++col)
            cellsRow[col].AddRowSkyscraper(line[col]);
    }

    private void AddColLine(int col, Line line)
    {
        colsLines[col].Add(line);

        for (var row = 0; row < N; ++row)
            cells[row][col].AddColSkyscraper(line[row]);
    }

    private void RemoveRowLine(int row, Line line)
    {
        if (!rowsLines[row].Remove(line))
            return;

        var cellsRow = cells[row];
        for (var col = 0; col < N; ++col)
            cellsRow[col].RemoveRowSkyscraper(line[col]);
    }

    private void RemoveColLine(int col, Line line)
    {
        if (!colsLines[col].Remove(line))
            return;

        for (var row = 0; row < N; ++row)
            cells[row][col].RemoveColSkyscraper(line[row]);
    }

    private void RemoveRowsWithSkyscraper(int row, int col, int skyscraper)
    {
        var lines = rowsLines[row];
        Debug.Assert(lines.Count > 1);

        for (var i = lines.Count - 1; i >= 0; --i)
        {
            var line = lines[i];
            if (line[col] == skyscraper)
                RemoveRowLine(row, line);
        }

        Debug.Assert(lines.Count >= 1);
    }

    private void RemoveColsWithSkyscraper(int row, int col, int skyscraper)
    {
        var lines = colsLines[col];
        Debug.Assert(lines.Count > 1);

        for (var i = lines.Count - 1; i >= 0; --i)
        {
            var line = lines[i];
            if (line[row] == skyscraper)
                RemoveColLine(col, line);
        }

        Debug.Assert(lines.Count >= 1);
    }

    private static IEnumerable<Line> GenerateLines(int N) =>
        GenerateLines(N, 0, new List<int>(N)).Select(line => new Line(line));

    private static IEnumerable<IEnumerable<int>> GenerateLines(int N, int level, List<int> workLine)
    {
        for (var h = 1; h <= N; ++h)
        {
            if (workLine.Contains(h))
                continue;

            workLine.Add(h);
            if (level + 1 == N)
                yield return workLine;
            else
                foreach (var line in GenerateLines(N, level + 1, workLine))
                    yield return line;
            workLine.RemoveAt(level);
        }
    }
}

public readonly struct Bounds
{
    public readonly int Left;
    public readonly int Right;

    public Bounds(int Left, int Right)
    {
        this.Left = Left;
        this.Right = Right;
    }
}

class Line
{
    public readonly IReadOnlyList<int> Skyscrapers;
    public readonly int LeftView;
    public readonly int RightView;

    public Line(IEnumerable<int> skyscrapers)
    {
        Skyscrapers = skyscrapers.ToArray();
        LeftView = EvaluateView(skyscrapers);
        RightView = EvaluateView(skyscrapers.Reverse());
    }

    public int this[int i] => Skyscrapers[i];

    public static int EvaluateView(IEnumerable<int> skyscrapers)
    {
        var count = 0;
        var max = int.MinValue;
        foreach(var item in skyscrapers)
            if (item > max)
            {
                ++count;
                max = item;
            }

        return count;
    }
}

class Cell
{
    readonly int[] rowCounts;
    readonly int[] colCounts;

    public bool Updated { get; set; }
    public bool Solved { get; private set; }

    public Cell(int N)
    {
        rowCounts = new int[N];
        colCounts = new int[N];
    }

    public int RowCounts(int skyscraper)
        => rowCounts[skyscraper - 1];

    public int ColCounts(int skyscraper)
        => colCounts[skyscraper - 1];

    public void AddRowSkyscraper(int skyscraper)
    {
        var count = ++rowCounts[skyscraper - 1];
        Debug.Assert(count >= 0);

        Updated = true;
        Solved = count <= 0 && rowCounts.Sum() == 1 && colCounts.Sum() == 1;
    }

    public void AddColSkyscraper(int skyscraper)
    {
        var count = ++colCounts[skyscraper - 1];
        Debug.Assert(count >= 0);

        Updated = true;
        Solved = count <= 0 && rowCounts.Sum() == 1 && colCounts.Sum() == 1;
    }

    public void RemoveRowSkyscraper(int skyscraper)
    {
        var count = --rowCounts[skyscraper - 1];
        Debug.Assert(count >= 0);

        Updated = Updated || (count == 0 && colCounts[skyscraper - 1] > 0);
        Solved = count <= 0 && rowCounts.Sum() == 1 && colCounts.Sum() == 1;
    }

    public void RemoveColSkyscraper(int skyscraper)
    {
        var count = --colCounts[skyscraper - 1];
        Debug.Assert(count >= 0);

        Updated = Updated || (count == 0 && rowCounts[skyscraper - 1] > 0);
        Solved = count <= 0 && rowCounts.Sum() == 1 && colCounts.Sum() == 1;
    }

    public void Clear()
    {
        for (var i = 0; i < rowCounts.Length; ++i)
            rowCounts[i] = 0;

        for (var i = 0; i < colCounts.Length; ++i)
            colCounts[i] = 0;

        Updated = false;
        Solved = false;
    }
}

readonly struct CellsLine
{
    public readonly IReadOnlyList<Cell> Line;

    public CellsLine(int N)
    {
        var cells = new Cell[N];
        for (var i = 0; i < N; ++i)
            cells[i] = new Cell(N);

        Line = cells;
    }

    public Cell this[int i] => Line[i];
}
