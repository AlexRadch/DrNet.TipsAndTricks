using System;
using System.Collections.Generic;
using System.Linq;

// Remove the minimum
// https://www.codewars.com/kata/563cf89eb4747c5fb100001b
public class Remover
{
    public static List<int> RemoveSmallest(List<int> numbers)
    {
        var result = new List<int>();
        if (numbers.Count <= 0)
            return result;

        var min = numbers[0];
        var minIndex = 0;
        for (var i = 1; i < numbers.Count; i++)
        {
            var item = numbers[i];
            if (item < min)
                (min, minIndex) = (item, i);
        }

        for (var i = 0; i < numbers.Count; i++)
            if (i != minIndex)
                result.Add(numbers[i]);

        return result;
    }
}