using System;
using System.Collections.Generic;

// Validate Sudoku with size `NxN`
// https://www.codewars.com/kata/540afbe2dc9f615d5e000425
class Sudoku
{
    private int[][] sudokuData;

    public Sudoku(int[][] sudokuData)
    {
        this.sudokuData = sudokuData;
    }

    public bool IsValid()
    {
        var n = (int)Math.Sqrt(sudokuData.Length);
        var N = n * n;

        if (sudokuData.Length != N)
            return false;

        SortedSet<int> line = new SortedSet<int>();

        foreach (var lineData in sudokuData)
        {
            line.Clear();
            line.UnionWith(lineData);

            if (!IsValid(line, N))
                return false;
        }

        for (var i = 0; i < N; ++i)
        {
            line.Clear();
            for (var j = 0; j < N; ++j)
                line.Add(sudokuData[j][i]);

            if (!IsValid(line, N))
                return false;
        }

        for (var i = 0; i < n; ++i)
        {
            line.Clear();
            for (var j = 0; j < n; ++j)
                for (var k = 0; k < n; ++k)
                    line.Add(sudokuData[i / n * n + j][i % n * n + j / n * n + k]);

            if (!IsValid(line, N))
                return false;
        }

        return true;
    }

    private bool IsValid(SortedSet<int> sudoku, int N) =>
        sudoku.Count == N && sudoku.Min == 1 && sudoku.Max == N;
}