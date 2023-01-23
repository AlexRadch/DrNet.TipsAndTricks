using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

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
            var matrix = new List<BigRational[]>(M);
            for (var rIndex = 0; rIndex < M; rIndex++)
                matrix.Add(lines[rIndex].Split().Select(cStr => BigRational.Parse(cStr)).ToArray());
            var N = M >= 0 ? matrix[0].Length - 1 : 0;

            int R = 0;
            for (int cIndex = 0; cIndex < N && R < M; cIndex++)
            {
                var rowR = matrix[R];
                if (rowR[cIndex].Numerator != 0)
                    goto Reduction;

                for (var rIndex = R + 1; rIndex < M; rIndex++)
                {
                    var row = matrix[rIndex];
                    if (row[cIndex].Numerator == 0)
                        continue;

                    (rowR, _) = (matrix[R], matrix[rIndex]) = (row, rowR);
                    goto Reduction;
                }
                continue;

            Reduction:
                {
                    var k = rowR[cIndex]; rowR[cIndex] = BigRational.One;
                    for (var cIndex2 = cIndex + 1; cIndex2 <= N; cIndex2++)
                        rowR[cIndex2] /= k;
                }
                for (var rIndex = 0; rIndex < M; rIndex++)
                {
                    if (rIndex == R)
                        continue;
                    var row = matrix[rIndex];
                    var k = row[cIndex]; row[cIndex] = 0;
                    for (var cIndex2 = cIndex + 1; cIndex2 <= N; cIndex2++)
                        row[cIndex2] -= rowR[cIndex2] * k;
                }
                R++;
            }

            while (M > R)
            {
                if (matrix[--M][N].Numerator != 0)
                    return NoSolution;
                matrix.RemoveAt(M);
            }

            for (var cIndex = 0; cIndex < N; cIndex++)
            {
                if (cIndex < M && matrix[cIndex][cIndex].Numerator != 0)
                    continue;
                var row = new BigRational[N + 1];
                row[cIndex] = -1;
                matrix.Insert(cIndex, row);
                M++;
            }

            var result = $"SOL=({string.Join("; ", matrix.Select(row => row[N]))})";

            R = 0;
            for (var cIndex = 0; cIndex < N; cIndex++)
            {
                if (matrix[cIndex][cIndex].Numerator != -1)
                    continue;
                R++;
                var line = $" + q{R} * ({string.Join("; ", matrix.Select(row => -row[cIndex]))})";
                result += line;
            }

            return result;
        }

        const string NoSolution = "SOL=NONE";
    }

    public struct BigRational
    {
        private BigInteger denominator;
        public BigInteger Numerator { get; }
        public BigInteger Denominator => denominator + 1;

        public BigRational(BigInteger numerator)
        {
            Numerator = numerator;
            denominator = 0;
        }

        public BigRational(BigInteger numerator, BigInteger denominator)
        {
            if (denominator == 0)
                throw new DivideByZeroException();

            BigInteger gcd = BigInteger.GreatestCommonDivisor(numerator, denominator);
            numerator /= gcd;
            denominator /= gcd;
            if (denominator < 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }
            Numerator = numerator;
            this.denominator = denominator - 1;
        }

        public override string ToString() => denominator == 0 ? Numerator.ToString() : $"{Numerator}/{Denominator}";

        public static BigRational Zero { get; } = new BigRational(0);
        public static BigRational One { get; } = new BigRational(1);

        public static implicit operator BigRational(byte value) => new BigRational(value);
        public static implicit operator BigRational(sbyte value) => new BigRational(value);
        public static implicit operator BigRational(short value) => new BigRational(value);
        public static implicit operator BigRational(ushort value) => new BigRational(value);
        public static implicit operator BigRational(int value) => new BigRational(value);
        public static implicit operator BigRational(uint value) => new BigRational(value);
        public static implicit operator BigRational(long value) => new BigRational(value);
        public static implicit operator BigRational(ulong value) => new BigRational(value);
        public static implicit operator BigRational(BigInteger value) => new BigRational(value);
        public static BigRational operator +(BigRational a) => a;
        public static BigRational operator -(BigRational a) => new BigRational(-a.Numerator, a.Denominator);
        public static BigRational operator +(BigRational a, BigRational b) =>
            new BigRational(a.Numerator * b.Denominator + a.Denominator * b.Numerator, a.Denominator * b.Denominator);
        public static BigRational operator -(BigRational a, BigRational b) =>
            new BigRational(a.Numerator * b.Denominator - a.Denominator * b.Numerator, a.Denominator * b.Denominator);
        public static BigRational operator *(BigRational a, BigRational b) =>
            new BigRational(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
        public static BigRational operator /(BigRational a, BigRational b) =>
            new BigRational(a.Numerator * b.Denominator, a.Denominator * b.Numerator);

        public static bool TryParse(string input, out BigRational fraction)
        {
            fraction = new BigRational();
            if (string.IsNullOrEmpty(input))
                return false;

            string[] parts = input.Split('/');
            switch (parts.Length)
            {
                case 1:
                    {
                        if (!BigInteger.TryParse(parts[0], out BigInteger numerator))
                            return false;
                        fraction = numerator;
                        return true;
                    }

                case 2:
                    {
                        if (!BigInteger.TryParse(parts[0], out BigInteger numerator))
                            return false;

                        if (!BigInteger.TryParse(parts[1], out BigInteger denominator))
                            return false;

                        fraction = new BigRational(numerator, denominator);
                        return true;
                    }

                default:
                    return false;
            }
        }

        public static BigRational Parse(string input) =>
            TryParse(input, out BigRational fraction) ? fraction : 
                throw new FormatException(
                    "Input string is not in the correct format. " +
                    "It should be in the format 'numerator/denominator' or 'BigInteger'");

    }
}