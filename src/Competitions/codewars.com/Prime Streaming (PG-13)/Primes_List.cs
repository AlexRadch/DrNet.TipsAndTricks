using System.Collections.Generic;

// Prime Streaming (PG-13)
// https://www.codewars.com/kata/5519a584a73e70fa570005f5
//
// use list to store primes and check candidates
public class Primes_List
{
    public static IEnumerable<int> Stream()
    {
        yield return 2;

        var primes = new List<int>();
        var limitIndex = 0;
        var limit = 3 * 3;
        for (var candidate = 3; candidate > 0; candidate += 2)
        {
            if (limit <= candidate)
            {
                limit = primes[++limitIndex];
                limit *= limit;
            }

            for (var i = 0; i < limitIndex; i++)
                if (candidate % primes[i] == 0)
                    goto NotPrime;

            primes.Add(candidate);
            yield return candidate;
        NotPrime:
            ;
        }
    }
}