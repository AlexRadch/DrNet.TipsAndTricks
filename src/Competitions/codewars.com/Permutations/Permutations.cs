using System;
using System.Collections.Generic;
using System.Linq;

// Permutations
// https://www.codewars.com/kata/5254ca2719453dcc0b00027d
class Permutations
{
    public static List<string> SinglePermutations(string s) =>
        GetPermutations(s).Select(p => string.Join("", p)).ToList();

    public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items)
    {
        var count = items.Count();
        if (count == 0)
        {
            yield return Array.Empty<T>();
            yield break;
        }

        var used = new HashSet<T>();
        foreach (var item in items.Select((it, i) => (it, i)))
        {
            if (used.Contains(item.Item1))
                continue;

            foreach (var p in GetPermutations(items.Take(item.Item2).Concat(items.Skip(item.Item2 + 1))))
                yield return Enumerable.Repeat(item.Item1, 1).Concat(p);

            used.Add(item.Item1);
        }
    }
}