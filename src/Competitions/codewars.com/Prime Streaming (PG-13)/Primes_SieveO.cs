using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Prime Streaming (PG-13)
// https://www.codewars.com/kata/5519a584a73e70fa570005f5
//
// Uses big Sieve (BitArray) to store primes and to qickly sieve candidates
public class Primes_SieveO
{
    const int Max = (1 << 24) - 1;

    public static IEnumerable<int> Stream()
    {
        yield return 2;

        var sieveSize = IntToIndex(Max + 1) + 1;
        var sieve = new BitArray(sieveSize);
        for (var canIndex = 0; canIndex < sieveSize; canIndex++)
        {
            if (sieve[canIndex]) continue; // Not prime

            yield return IndexToInt(canIndex);

            var indexDelta = canIndex + canIndex + 3;
            for (int notPrimeIndex = canIndex + indexDelta; notPrimeIndex < sieveSize; notPrimeIndex += indexDelta)
                sieve.Set(notPrimeIndex, true);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int IntToIndex(int value) => (value - 3) / 2;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int IndexToInt(int index) => index * 2 + 3;
}