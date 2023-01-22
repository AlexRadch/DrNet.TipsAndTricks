using System;
using System.Linq;

// Linear equations N x M, complete solution space, fraction representation
// https://www.codewars.com/kata/56464cf3f982b2e10d000015
namespace LinearSystems
{
    public class LinearSystem
    {

        public string Solve(string input)
        {
            Console.WriteLine(input);
            var lines = input.Split("\n");
            var M = lines.Length;
            var matrix = new Fraction[M][];
            for (var rIndex = 0; rIndex < M; rIndex++)
                matrix[rIndex] = lines[rIndex].Split().Select(cStr => Fraction.Parse(cStr)).ToArray();

            var N = matrix[0].Length - 1;
            var R = 0;
            for (var cIndex = 0; cIndex < N; cIndex++)
            {
                for (var rIndex = R; rIndex < M; rIndex++)
                {
                    var row = matrix[rIndex];
                    if (row[cIndex].Numerator == 0)
                        continue;

                    (matrix[R], matrix[rIndex]) = (row, matrix[R]);

                    var l = row[cIndex]; row[cIndex] = Fraction.One;
                    {
                        for (var cIndex2 = cIndex + 1; cIndex2 <= N; cIndex2++)
                            rowC[cIndex2] /= l;
                    }
                    for (var rIndex = cIndex + 1; rIndex < M; rIndex++)
                    {
                        var row = matrix[rIndex];
                        var l = row[cIndex]; row[cIndex] = Fraction.Zero;
                        for (var cIndex2 = cIndex + 1; cIndex2 <= N; cIndex2++)
                            row[cIndex2] -= rowC[cIndex2] * l;
                    }
                    break;
                }
            }

            for (var rIndex = C; rIndex < M; rIndex++)
                if (matrix[rIndex][N].Numerator != 0)
                    return NoSolution;

            for (var cIndex = C - 1; cIndex > 0; cIndex--)
            {
                var rowC = matrix[cIndex];
                for (var rIndex = 0; rIndex < cIndex; rIndex++)
                {
                    var row = matrix[rIndex];
                    var l = row[cIndex]; row[cIndex] = Fraction.Zero;
                    for (var cIndex2 = C; cIndex2 <= N; cIndex2++)
                        row[cIndex2] -= rowC[cIndex2] * l;
                }
            }

            var answer = matrix.Select(row => row[N]).Take(C).Concat(Enumerable.Repeat(Fraction.Zero, N - C).ToArray());
            var result = $"SOL=({string.Join("; ", answer)})";

            for (var cIndex = C; cIndex < N; cIndex++)
            {
                var row = new Fraction[N];
                for (var cIndex2 = 0; cIndex2 < C; cIndex2++)
                    row[cIndex2] = -matrix[cIndex2][cIndex];

                for (var cIndex2 = C; cIndex2 < N; cIndex2++)
                    row[cIndex2] = Fraction.Zero;

                row[cIndex] = Fraction.One;

                var line = $" + q{cIndex - C + 1} * ({string.Join("; ", row)})";

                result += line;
            }

            return result;
        }

        const string NoSolution = "SOL=NONE";
    }

    public struct Fraction
    {
        public long Numerator { get; private set; }
        public long Denominator { get; private set; }

        public Fraction(long numerator)
        {
            Numerator = numerator;
            Denominator = 1;
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
                (a, b) = (b, a % b);
            return a;
        }

        public static Fraction Zero { get; } = new Fraction(0);
        public static Fraction One { get; } = new Fraction(1);

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

        public static bool TryParse(string input, out Fraction fraction)
        {
            fraction = new Fraction();
            if (string.IsNullOrEmpty(input))
                return false;

            string[] parts = input.Split('/');
            switch (parts.Length)
            {
                case 1:
                    {
                        if (!long.TryParse(parts[0], out long numerator))
                            return false;
                        fraction = numerator;
                        return true;
                    }

                case 2:
                    {
                        if (!long.TryParse(parts[0], out long numerator))
                            return false;

                        if (!long.TryParse(parts[1], out long denominator))
                            return false;

                        fraction = new Fraction(numerator, denominator);
                        return true;
                    }

                default:
                    return false;
            }
        }

        public static Fraction Parse(string input) =>
            TryParse(input, out Fraction fraction) ? fraction : 
                throw new FormatException(
                    "Input string is not in the correct format. " + 
                    "It should be in the format 'numerator/denominator' or 'integer'");

    }
}