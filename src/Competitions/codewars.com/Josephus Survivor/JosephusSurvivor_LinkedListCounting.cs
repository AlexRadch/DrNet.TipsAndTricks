using System.Collections.Generic;
using System.Linq;

// Josephus Survivor
// https://www.codewars.com/kata/555624b601231dc7a400017a
//
// LinkedList Counting
public class JosephusSurvivor_LinkedListCounting
{
    public static int JosSurvivor(int n, int k)
    {
        var list = new LinkedList<int>(Enumerable.Range(1, n));

        var node = list.First!;
        while (list.Count > 1)
        {
            for (var i = 1; i < k; i++)
                node = node.Next ?? list.First!;

            var next = node.Next ?? list.First!;
            list.Remove(node);
            node = next;
        }

        return list.First!.Value;
    }
}