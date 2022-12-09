using System;
using System.Collections.Generic;
using System.Linq;

// Count characters in your string
// https://www.codewars.com/kata/52efefcbcdf57161d4000091
public class Kata
{
    public static Dictionary<char, int> Count(string str) =>
      str.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());
}