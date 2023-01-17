using System;
using System.Collections;
using System.Collections.Generic;

// Prime Streaming (PG-13)
// https://www.codewars.com/kata/5519a584a73e70fa570005f5
public class Primes
{
    public static IEnumerable<int> Stream()
    {
        yield return 2;

        var max = Math.Sqrt(int.MaxValue);
        var primes = new List<int>() { 3, 5, 7, 11, 13, 17 };

        var sieve = BitArray
        

        for (var i = 0; i < sieveSize; i++)


        return new List<int> { 2, 3, 5, 7, 11, 13, 17 };
    }
}