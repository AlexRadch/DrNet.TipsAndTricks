//the test case from java version
using NUnit.Framework;
using System;
using System.Collections.Generic;
public class JosephusTestSuit
{
    private static int NUM_RANDOM_TESTS = 40;
    private static int MAX_ITEMS = 50;
    private static int MAX_ITEM_VALUE = 200;
    private static int MAX_K = 20;

    [Test]
    public void Test1()
    {
        JosephusTest(new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 1, new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
    }

    [Test]
    public void Test2()
    {
        JosephusTest(new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 2, new object[] { 2, 4, 6, 8, 10, 3, 7, 1, 9, 5 });
    }

    [Test]
    public void Test3()
    {
        JosephusTest(new object[] { "C", "o", "d", "e", "W", "a", "r", "s" }, 4, new object[] { "e", "s", "W", "o", "C", "d", "r", "a" });
    }

    [Test]
    public void Test4()
    {
        JosephusTest(new object[] { 1, 2, 3, 4, 5, 6, 7 }, 3, new object[] { 3, 6, 2, 7, 5, 1, 4 });
    }

    [Test]
    public void Test5()
    {
        JosephusTest(new object[] { }, 3, new object[] { });
    }

    [Test]
    public void Test6()
    {
        JosephusTest(new object[] { "C", 0, "d", 3, "W", 4, "r", 5 }, 4, new object[] { 3, 5, "W", 0, "C", "d", "r", 4 });
    }

    [Test]
    public void Test7()
    {
        JosephusTest(new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50 }, 11, new object[] { 11, 22, 33, 44, 5, 17, 29, 41, 3, 16, 30, 43, 7, 21, 36, 50, 15, 32, 48, 14, 34, 1, 20, 39, 9, 28, 2, 25, 47, 24, 49, 27, 8, 38, 19, 6, 42, 35, 26, 23, 31, 40, 4, 18, 12, 13, 46, 37, 45, 10 });
    }

    [Test]
    public void Test8()
    {
        JosephusTest(new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 }, 40, new object[] { 10, 7, 8, 13, 5, 4, 12, 11, 3, 15, 14, 9, 1, 6, 2 });
    }

    [Test]
    public void Test9()
    {
        JosephusTest(new object[] { 1 }, 3, new object[] { 1 });
    }

    [Test]
    public void Test10()
    {
        JosephusTest(new object[] { true, false, true, false, true, false, true, false, true }, 9, new object[] { true, true, true, false, false, true, false, true, false });
    }

    [Test]
    public void RandomTest()
    {
        var random = new Random();
        for (int i = 0; i < NUM_RANDOM_TESTS; i++)
        {
            var items = new List<object>();
            for (int j = 0; j < random.Next(MAX_ITEMS); j++)
            {
                items.Add(random.Next(MAX_ITEM_VALUE));
            }
            int k = random.Next(MAX_K - 1) + 1;
            Assert.AreEqual(Solution(new List<object>(items), k), Josephus.JosephusPermutation(new List<object>(items), k));
        }
    }

    private void JosephusTest(object[] items, int k, object[] result)
    {
        Assert.AreEqual(new List<object>(result), Josephus.JosephusPermutation(new List<object>(items), k));
    }

    private List<object> Solution(List<object> items, int k)
    {
        var permutation = new List<object>();
        int position = 0;
        while (items.Count > 0)
        {
            position = (position + k - 1) % items.Count;
            var item = items[position];
            items.RemoveAt(position);
            permutation.Add(item);
        }
        return permutation;
    }
}