using System;
using System.Numerics;

// BECOME IMMORTAL
// https://www.codewars.com/kata/59568be9cc15b57637000054
public static class Immortal
{
    /// set true to enable debug
    public static bool Debug = false;

    public static long ElderAge(long n, long m, long k, long newp)
    {
        if (n <= 0 || m <= 0)
            return 0;
        if (m > n)
            (n, m) = (m, n);

        var np = (long)BitOperations.RoundUpToPowerOf2((ulong)n);
        if (k >= np)
            return 0;
        var mp = (long)BitOperations.RoundUpToPowerOf2((ulong)m);

        if (np == mp)
        {
            var s1 = Rem(RangeSum(1, np - k - 1) * (n + m - np), newp);
            var s2 = ElderAge(np - n, mp - m, k, newp);
            return (s1 + s2) % newp;
        }
        {
            mp = np / 2;
            var s1 = Rem(RangeSum(1, np - k - 1) * m - (np - n) * RangeSum(Math.Max(0, mp - k), np - k - 1), newp);
            if (s1 < 0)
                s1 += newp;
            var s2 = k <= mp ?
                Rem(((BigInteger)(mp - k)) * (mp - m) * (np - n), newp) + ElderAge(mp - m, np - n, 0, newp) :
                ElderAge(mp - m, np - n, k - mp, newp);
            return (s1 + s2) % newp;
        }
    }

    public static BigInteger RangeSum(long a1, long a2) => ((BigInteger)(a1 + a2)) * (a2 - a1 + 1) / 2;

    public static long Rem(BigInteger b, long l) => (long)(b % l);
}