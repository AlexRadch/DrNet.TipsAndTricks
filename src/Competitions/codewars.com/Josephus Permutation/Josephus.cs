using System.Collections.Generic;

// Josephus Permutation
// https://www.codewars.com/kata/5550d638a99ddb113e0000a2
public class Josephus
{
    public static List<object> JosephusPermutation(List<object> items, int k)
    {
        var result = new List<object>(items.Count);

        var pos = 0;
        while(items.Count > 0)
        {
            pos = (pos + k - 1) % items.Count;
            result.Add(items[pos]);
            items.RemoveAt(pos);
        }

        return result;
    }
}