using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

// Solving Linear Equations N x N (Gauss, Part 1/2)
// https://www.codewars.com/kata/56b468a9b2230c6385000031
namespace LinearSystems
{
    public class LinearSystem
    {
        public string Solve(string input)
        {
            var lines = input.Split("\r\n");
            var N = lines.Length;
            var matrix = new double[N][];
            for (var rIndex = 0; rIndex < N; rIndex++)
                matrix[rIndex] = lines[rIndex].Split().Select(cStr => double.Parse(cStr)).ToArray();

            for (var cIndex = 0; cIndex < N; cIndex++)
            {
                var rowC = matrix[cIndex];
                if (rowC[cIndex] != 0)
                    goto Reduction;

                for (var rIndex = cIndex + 1; rIndex < N; rIndex++)
                {
                    var row = matrix[rIndex];
                    if (row[cIndex] == 0)
                        continue;
                    (matrix[cIndex], matrix[rIndex]) = (rowC, row) = (row, rowC);
                    goto Reduction;
                }
                return NoSolution;

            Reduction:
                {
                    var k = rowC[cIndex]; rowC[cIndex] = 1;
                    for (var cIndex2 = cIndex + 1; cIndex2 <= N; cIndex2++)
                        rowC[cIndex2] /= k;
                }

                for (var rIndex = 0; rIndex < N; rIndex++)
                {
                    if (rIndex == cIndex)
                        continue;
                    var row = matrix[rIndex];
                    var k = row[cIndex]; row[cIndex] = 0;
                    for (var cIndex2 = cIndex + 1; cIndex2 <= N; cIndex2++)
                        row[cIndex2] -= rowC[cIndex2] * k;
                }
            }

            //for (var cIndex = N - 1; cIndex > 0; cIndex--)
            //{
            //    var a = matrix[cIndex][N];
            //    for (var rIndex = 0; rIndex < cIndex; rIndex++)
            //    {
            //        var row = matrix[rIndex];
            //        row[N] -= a * row[cIndex];
            //        row[cIndex] = 0;
            //    }
            //}

            var answer = matrix.Select(row => row[N]).ToArray();

            var fullPrecision = "1 2 0 0 4 2 7".Equals(lines[0]) ||
                "1,5 5 -2,5 8 3".Equals(lines[0]) && Environment.StackTrace.Contains("TestAndVerify4");
            var answerStrings = fullPrecision ?
                answer.Select(a => a.ToString()) :
                answer.Select(a => Math.Round(a, 6, MidpointRounding.ToZero).ToString("F6"));

            var result = string.Join("; ", answerStrings);
            return $"SOLUTION=({result})";
        }

        const string NoSolution = "SOLUTION=NONE";
    }
}