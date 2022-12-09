using System.Linq;

// Find The Parity Outlier
// https://www.codewars.com/kata/5526fc09a1bbd946250002dc
public class Kata
{
    public static int Find(int[] integers)
    {
        if (integers.Take(3).Where(x => (x & 1) == 0).Count() > 1)
            return integers.Where(x => (x & 1) == 1).First();
        return integers.Where(x => (x & 1) == 0).First();
    }
}