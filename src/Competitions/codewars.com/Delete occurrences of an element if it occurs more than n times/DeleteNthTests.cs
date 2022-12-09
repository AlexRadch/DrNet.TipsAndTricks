using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

[TestFixture]
public class DeleteNthTests
{
    const int TESTS = 20;
    const int MOTIVES = 10;

    [Test]
    public void TestRandom()
    {
        List<int[]> tests = new List<int[]>();
        Random rand = new Random();

        for (var x = 0; x < TESTS; x++)
        {
            int[] motives = new int[MOTIVES];

            for (var y = 0; y < MOTIVES; y++)
            {
                motives[y] = rand.Next(3) + 1;
            }

            tests.Add(motives);
        }

        foreach (var test in tests)
        {
            int max = rand.Next(3) + 1;
            var expected = Solve(test, max);
            var actual = Kata.DeleteNth(test, max);

            Console.WriteLine(
              String.Format("([{0}], {1}) \n-  Expected: {2} Actual: {3}",
                String.Join(",", test),
                max,
                String.Join(",", expected),
                String.Join(",", actual)));

            CollectionAssert.AreEqual(expected, actual);
        }
    }

    private static int[] Solve(int[] arr, int x)
    {
        var cache = new System.Collections.Generic.Dictionary<int, int>();

        return arr.Where(n => {
            int count = cache.ContainsKey(n) ? cache[n] : 0;
            cache[n] = count + 1;
            return cache[n] <= x;
        }).ToArray();
    }
}