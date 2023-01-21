using System;
using System.Linq;
using System.Reflection;

// Linear equations N x M, complete solution space, fraction representation
// https://www.codewars.com/kata/56464cf3f982b2e10d000015
namespace LinearSystems
{
    public class LinearSystem
    {

        public string Solve(string input)
        {
            var lines = input.Split("\n");
            var M = lines.Length;
            var matrix = new Fraction[M][];
            for (var rIndex = 0; rIndex < M; rIndex++)
                matrix[rIndex] = lines[rIndex].Split().Select(cStr => (Fraction)int.Parse(cStr)).ToArray();

            int C;
            var N = matrix[0].Length - 1;
            for (var cIndex = 0; cIndex < N; cIndex++)
            {
                for (var cIndex2 = cIndex; cIndex < N; cIndex2++)
                {
                    for (var rIndex = cIndex; rIndex < M; rIndex++)
                        if (matrix[rIndex][cIndex].Numerator != 0)
                        {
                            (matrix[cIndex], matrix[rIndex]) = (matrix[rIndex], matrix[cIndex]);
                            if (cIndex2 != cIndex)
                                foreach (var row in matrix)
                                    (row[cIndex], row[cIndex2]) = (row[cIndex2], row[cIndex]);
                            goto Reduction;
                        }
                }
                C = cIndex;
                goto Phase2;
            Reduction:
                for (var rIndex = cIndex + 1; rIndex < M; rIndex++)
                {
                    var row1 = matrix[cIndex];
                    var row2 = matrix[rIndex];
                    var l = row2[cIndex] / row1[cIndex];

                    row2[cIndex] = 0;
                    for (var cIndex2 = cIndex + 1; cIndex2 <= N; cIndex2++)
                        row2[cIndex2] -= row1[cIndex2] * l;
                }
            }

            C = N;
            Phase2:
            for (var rIndex = C; rIndex < M; rIndex++)
                if (matrix[rIndex][N].Numerator != 0)
                    return NoSolution;

            for (var cIndex = C - 1; cIndex >= 0; cIndex--)
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
            var result = $"SOL=({string.Join("; ", answer)})";

            return result;
        }

        const string NoSolution = "SOLUTION=NONE";
    }

    public struct Fraction
    {
        public long Numerator { get; private set; }
        public long Denominator { get; private set; }

        public Fraction(long numerator)
        {
            Numerator = numerator;
            Denominator = 1;
            Simplify();
        }

        public Fraction(long numerator, long denominator)
        {
            if (denominator == 0)
                throw new DivideByZeroException();

            Numerator = numerator;
            Denominator = denominator;
            Simplify();
        }

        private void Simplify()
        {
            long gcd = GCD(Numerator, Denominator);
            Numerator /= gcd;
            Denominator /= gcd;

            if (Denominator < 0)
            {
                Numerator = -Numerator;
                Denominator = -Denominator;
            }
        }

        public override string ToString() => Denominator == 1 ? Numerator.ToString() : $"{Numerator}/{Denominator}";

        public static long GCD(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static implicit operator Fraction(long value) =>
            new Fraction(value);
        public static Fraction operator +(Fraction a) => a;
        public static Fraction operator -(Fraction a) => new Fraction(-a.Numerator, a.Denominator);
        public static Fraction operator +(Fraction a, Fraction b) =>
            new Fraction(a.Numerator * b.Denominator + a.Denominator * b.Numerator, a.Denominator * b.Denominator);
        public static Fraction operator -(Fraction a, Fraction b) =>
            new Fraction(a.Numerator * b.Denominator - a.Denominator * b.Numerator, a.Denominator * b.Denominator);
        public static Fraction operator *(Fraction a, Fraction b) =>
            new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
        public static Fraction operator /(Fraction a, Fraction b) =>
            new Fraction(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
    }
}