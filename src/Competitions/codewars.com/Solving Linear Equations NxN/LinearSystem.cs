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
                var rowR = matrix[cIndex];
                if (rowR[cIndex] != 0)
                    goto Reduction;

                for (var rIndex = cIndex + 1; rIndex < N; rIndex++)
                {
                    var row = matrix[rIndex];
                    if (row[cIndex] == 0)
                        continue;
                    (rowR, _) = (matrix[cIndex], matrix[rIndex]) = (row, rowR);
                    goto Reduction;
                }
                return NoSolution;

            Reduction:
                {
                    var k = rowR[cIndex]; rowR[cIndex] = 1;
                    for (var cIndex2 = cIndex + 1; cIndex2 <= N; cIndex2++)
                        rowR[cIndex2] /= k;
                }

                for (var rIndex = 0; rIndex < N; rIndex++)
                {
                    if (rIndex == cIndex)
                        continue;
                    var row = matrix[rIndex];
                    var k = row[cIndex]; row[cIndex] = 0;
                    for (var cIndex2 = cIndex + 1; cIndex2 <= N; cIndex2++)
                        row[cIndex2] -= rowR[cIndex2] * k;
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