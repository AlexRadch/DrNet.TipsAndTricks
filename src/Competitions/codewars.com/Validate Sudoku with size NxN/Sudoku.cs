using System;
using System.Linq;

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

        foreach (var lineData in sudokuData)
            if (!IsValid(lineData, N))
                return false;

        var line = new int[N];
        for (var i = 0; i < N; ++i)
        {
            for (var j = 0; j < N; ++j)
                line[j] = sudokuData[j][i];
            if (!IsValid(line, N))
                return false;
        }

        for (var i = 0; i < n; ++i)
        {
            for (var j = 0; j < n; ++j)
                for (var k = 0; k < n; ++k)
                    line[j * n + k] = sudokuData[(i / n) * n + j][i % n * n + (j / n) * n + k];

            if (!IsValid(line, N))
                return false;
        }

        return true;
    }

    private bool IsValid(int[] sudoku, int n)
    {
        if (sudoku.Length != n)
            return false;

        for (var i = 1; i <= n; ++i) 
            if (!sudoku.Contains(i))
                return false;

        return true;
    }
}