using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// Prime Streaming (PG-13)
// https://www.codewars.com/kata/5519a584a73e70fa570005f5
//
// Uses big Sieve (BitArray) to store primes and to qickly sieve candidates
public class Primes_SieveO3
{
    const int Max = (1 << 24) - 1;

    public static IEnumerable<int> Stream()
    {
        yield return 2;
        yield return 3;

        var sieveSize = IntToIndex(Max + 1) + 1;
        var sieve = new BitArray(sieveSize);

        var canIndex = 0;
        while (canIndex < sieveSize)
        {
            if (sieve[canIndex]) goto NotPrime1;

            yield return IndexToInt(canIndex);

            var indexDelta = canIndex + canIndex + 5;
            for (int notPrimeIndex = canIndex + indexDelta; notPrimeIndex < sieveSize; notPrimeIndex += indexDelta)
                sieve.Set(notPrimeIndex, true);

            NotPrime1:
            canIndex++;
            if (canIndex >= sieveSize)
                break;

            if (sieve[canIndex]) goto NotPrime2;

            yield return IndexToInt(canIndex);

            indexDelta = canIndex + canIndex + 5;
            for (int notPrimeIndex = canIndex + indexDelta; notPrimeIndex < sieveSize; notPrimeIndex += indexDelta)
                sieve.Set(notPrimeIndex, true);

            NotPrime2:
            canIndex += 2;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int IntToIndex(int value) => (value - 5) / 2;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int IndexToInt(int index) => index * 2 + 5;
}