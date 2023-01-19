// Josephus Survivor
// https://www.codewars.com/kata/555624b601231dc7a400017a
//
// Recursion Optimized with Loop https://e-maxx.ru/algo/joseph_problem
using System;
using System.Numerics;

public class JosephusSurvivor_RecursionOLoop
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

        var res = JosSurvivor0_ngk(n - times, k) - rem;
        return res + (res < 0 ? n : res / (k - 1));
    }

    private static int JosSurvivor0(int n, int k)
    {
        var result = 0;
        for (var i = 2; i <= n; i++)
            result = (result + k) % i;
        return result;
    }
}