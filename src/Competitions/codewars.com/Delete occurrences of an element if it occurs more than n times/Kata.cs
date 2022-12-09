using System.Collections.Generic;
using System.Linq;

// Delete occurrences of an element if it occurs more than n times
// https://www.codewars.com/kata/554ca54ffa7d91b236000023
public class Kata
{
    public static int[] DeleteNth(int[] arr, int x)
    {
        var dict = new Dictionary<int, int>();
        return arr.Where(v => {
            int count;
            _ = dict.TryGetValue(v, out count);
            dict[v] = ++count;
            return count <= x;
        }).ToArray();
    }
}