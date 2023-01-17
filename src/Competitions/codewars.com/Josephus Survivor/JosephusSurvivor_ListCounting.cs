using System.Collections.Generic;

// Josephus Survivor
// https://www.codewars.com/kata/555624b601231dc7a400017a
//
// List Counting
public class JosephusSurvivor_ListCounting
{
    public static int JosSurvivor(int n, int k)
    {
        var list = new List<int>(n);
        for (var i = 1; i <= n; i++)
            list.Add(i);

        var index = -1;
        while (list.Count > 1)
        {
            index = (index + k) % list.Count;
            list.RemoveAt(index--);
        }

        return list[0];
    }
}