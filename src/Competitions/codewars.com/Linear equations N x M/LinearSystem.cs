using System;

// Linear equations N x M, complete solution space, fraction representation
// https://www.codewars.com/kata/56464cf3f982b2e10d000015
namespace LinearSystems
{
    public class LinearSystem
    {

        public string Solve(string input)
        {
            string result = "";
            return result;
        }

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

        public override string ToString() => $"{Numerator}/{Denominator}";

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