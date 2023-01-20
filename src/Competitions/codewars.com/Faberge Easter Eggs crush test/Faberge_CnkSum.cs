using System.Numerics;

// Fabergé Easter Eggs crush test
// https://www.codewars.com/kata/54cb771c9b30e8b5250011d4
//
// Fast Sum of Combinations (m, 1..n)
public class Faberge_CnkSum
{
    public static BigInteger Height(int n, int m)
    {
        if (n > m) n = m;

        BigInteger result = 0, Clk = 1; var l = m;
        for (int k = 1; k <= n; result += Clk, l--, k++)
            Clk = Clk * l / k;

        return result;
    }
}