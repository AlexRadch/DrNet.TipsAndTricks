using System;

// BECOME IMMORTAL
// https://www.codewars.com/kata/59568be9cc15b57637000054
public static class Immortal
{
    /// set true to enable debug
    public static bool Debug = false;

    public static long ElderAge(long n, long m, long k, long newp)
    {
        if (n != m)
        {
            var max = Math.Max(n, m);
            var min = Math.Min(n, m);
            return (ElderAge(max, max, k, newp) + ElderAge(min, min, k, newp)) / 2 % newp;
        }
        var nk = n - k;
        if (nk <= 0)
            return 0;
        var result = nk * (nk - 1) / 2 * n;
        return result;
    }
}