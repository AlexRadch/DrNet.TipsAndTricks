﻿using System.Collections.Generic;

// Prime Streaming (PG-13)
// https://www.codewars.com/kata/5519a584a73e70fa570005f5
public class Primes
{
    public static IEnumerable<int> Stream()
    {
        return Primes_SieveO3.Stream();
        //return Primes_SieveO.Stream();
        //return Primes_Sieve.Stream();
        //return Primes_List.Stream();
    }
}