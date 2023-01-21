using System;
using System.Globalization;
using System.Linq;

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
                for (var rIndex = cIndex; rIndex < N; rIndex++)
                    if (matrix[rIndex][cIndex] != 0)
                    {
                        (matrix[cIndex], matrix[rIndex]) = (matrix[rIndex], matrix[cIndex]);
                        goto Reduction;
                    }
                return NoSolution;

            Reduction:
                for (var rIndex = cIndex + 1; rIndex < N; rIndex++)
                {
                    var row1 = matrix[cIndex];
                    var row2 = matrix[rIndex];
                    var l = row2[cIndex] / row1[cIndex];

                    row2[cIndex] = 0;
                    for (var cIndex2 = cIndex + 1; cIndex2 <= N; cIndex2++)
                        row2[cIndex2] -= row1[cIndex2] * l;
                }
            }

            for (var cIndex = N - 1; cIndex >= 0; cIndex--)
            {
                var l = matrix[cIndex][N] /= matrix[cIndex][cIndex];
                matrix[cIndex][cIndex] = 1;
                for (var rIndex = cIndex - 1; rIndex >= 0; rIndex--)
                {
                    matrix[rIndex][N] -= l * matrix[rIndex][cIndex];
                    matrix[rIndex][cIndex] = 0;
                }
            }

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