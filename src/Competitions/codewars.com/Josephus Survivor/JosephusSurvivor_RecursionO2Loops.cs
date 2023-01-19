// Josephus Survivor
// https://www.codewars.com/kata/555624b601231dc7a400017a
//
// Recursion Optimized with 2 Loops https://en.wikipedia.org/wiki/Josephus_problem
using System;
using System.Numerics;

public class JosephusSurvivor_RecursionO2Loops
{
    public static int JosSurvivor(int n, int k) => k switch
    {
        > 2 => JosSurvivor0_ngk(n, k) + 1,
        2 => (n - (1 << BitOperations.Log2((uint)n))) * 2 + 1, // 2 * (n - Integer.highestOneBit(n)) + 1
        _ => n, // 1
    };

    private static int JosSurvivor0_ngk(int n, int k)
    {
        var (times, rem) = Math.DivRem(n, k);
        if (times < 2) return JosSurvivor0(n, k);

        int result;
        var t2 = rem / times;
        if (t2 > 1)
        {
            var d = t2 * times;
            var n2 = n - d;
            var r2 = rem - d;

            result = JosSurvivor0_ngk(n2 - times, k) - r2;
            result += result < 0 ? n2 : result / (k - 1);

            n2 += times;
            while (n2 < n)
            {
                r2 += times;
                result -= r2;
                result += result < 0 ? n2 : result / (k - 1);
                n2 += times;
            }
        }
        else
            result = JosSurvivor0_ngk(n - times, k);

        result -= rem;
        return result + (result < 0 ? n : result / (k - 1));
    }

    private static int JosSurvivor0(int n, int k)
    {
        var result = 0;
        for (var i = 2; i <= n; i++)
            result = (result + k) % i;
        return result;
    }
}