using System.Collections.Generic;
using System.Linq;

// Product of consecutive Fib numbers
// https://www.codewars.com/kata/5541f58a944b85ce6d00006a
public class ProdFib
{
    public static ulong[] productFib(ulong prod)
    {
        return Fibs().Where(item => item.Item1 * item.Item2 >= prod)
          .Select(item => new ulong[] { item.Item1, item.Item2, item.Item1 * item.Item2 == prod ? 1ul : 0ul })
          .First();
    }

    public static IEnumerable<(ulong, ulong)> Fibs()
    {
        (ulong, ulong) item = (0ul, 1ul);
        while (true)
        {
            yield return item;
            item = (item.Item2, item.Item1 + item.Item2);
        }
    }
}