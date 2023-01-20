using System.Collections;
using System.Collections.Generic;

// Prime Streaming (PG-13)
// https://www.codewars.com/kata/5519a584a73e70fa570005f5
//
// Uses big Sieve (BitArray) to store primes and to qickly sieve candidates
public class Primes_Sieve
{
    const int Max = (1 << 24) - 1;

    public static IEnumerable<int> Stream()
    {
        yield return 2;
    
        var sieve = new BitArray(ToSieveIndex(Max + 1) + 1);
        for (var candidate = 3; ChechLimit(candidate); candidate += 2)
        {
            if (sieve[ToSieveIndex(candidate)]) continue; // Not prime

            yield return candidate;

            for (int notPrime = candidate * 3; ChechLimit(notPrime); notPrime += candidate + candidate)
                sieve.Set(ToSieveIndex(notPrime), true);
        }
    }

    private static bool ChechLimit(int num) => num > 0 && num <= Max;

    private static int ToSieveIndex(int num) => (num - 3) / 2;
}