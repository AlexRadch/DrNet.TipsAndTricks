using System.Numerics;

// Fabergé Easter Eggs crush test
// https://www.codewars.com/kata/54cb771c9b30e8b5250011d4
//
// Slow Recursion
public class Faberge_SlowRecursion
{
    public static BigInteger Height(int n, int m) => (n, m) switch
    {
        ( <= 0, _) => 0,
        (_, <= 0) => 0,
        (_, _) => Height(n - 1, m - 1) + 1 + Height(n, m - 1),
    };
}