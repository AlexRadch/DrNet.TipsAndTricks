using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class PrimesTest
{

    [Test]
    public void Test_0_10()
    {
        Test(0, 10, 2, 3, 5, 7, 11, 13, 17, 19, 23, 29);
    }

    [Test]
    public void Test_10_10()
    {
        Test(10, 10, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71);
    }

    [Test]
    public void Test_100_10()
    {
        Test(100, 10, 547, 557, 563, 569, 571, 577, 587, 593, 599, 601);
    }

    [Test]
    public void Test_1000_10()
    {
        Test(1000, 10, 7927, 7933, 7937, 7949, 7951, 7963, 7993, 8009, 8011, 8017);
    }

    [Test]
    public void Test_10000_10()
    {
        Test(10000, 10, 104743, 104759, 104761, 104773, 104779, 104789, 104801, 104803, 104827, 104831);
    }

    [Test]
    public void Test_100000_10()
    {
        Test(100000, 10, 1299721, 1299743, 1299763, 1299791, 1299811, 1299817, 1299821, 1299827, 1299833, 1299841);
    }

    [Test]
    public void Test_1000000_10()
    {
        Test(1000000, 10, 15485867, 15485917, 15485927, 15485933, 15485941, 15485959, 15485989, 15485993, 15486013, 15486041);
    }

    [Test]
    public void RandomTest()
    {
        var rnd = new Random();
        for (int i = 0; i < 5; i++)
        {
            var skip = rnd.Next(10000, 100000);
            var limit = rnd.Next(5, 20);
            Test(skip, limit, new TestPrimeEnumerator().Skip(skip).Take(limit).ToArray());
        }
    }

    private void Test(int skip, int limit, params int[] expect)
    {
        int[] found = Primes.Stream().Skip(skip).Take(limit).ToArray();
        Assert.AreEqual(expect, found);
    }

    private class TestPrimeEnumerator : IEnumerable<int>
    {
        private BitArray sieve = new BitArray(100);
        private int max = 2;
        private int next = 2;

        public IEnumerator<int> GetEnumerator()
        {
            while (true)
            {
                int p = NextClearBit(next);
                if (p > max)
                {
                    int m = max * 2;
                    for (int i = 2; i <= max; i++)
                    {
                        if (!sieve[i])
                        {
                            for (int j = 2 * i; j <= m; j += i)
                                SetWithAutoGrow(j);
                        }
                    }
                    max = m;
                    p = NextClearBit(next);
                }
                next = p + 1;
                yield return p;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private int NextClearBit(int next)
        {
            for (int i = next; i < sieve.Length; i++)
                if (!sieve[i]) return i;
            return -1;
        }

        private void SetWithAutoGrow(int index)
        {
            if (index < sieve.Length) sieve[index] = true;
            else
            {
                var tmp = new BitArray(index + index / 2);
                for (int i = 0; i < sieve.Length; i++)
                    tmp[i] = sieve[i];
                sieve = tmp;
                sieve[index] = true;
            }
        }
    }

}