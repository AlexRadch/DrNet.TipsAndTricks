//The test cases from java version
using NUnit.Framework;
using System;
using System.Text;
using System.Collections.Generic;
public class SolutionTest
{
    [Test]
    public void TestCases()
    {
        var warriorResult = new List<string>();
        var testList = new List<string>();
        var testValues = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        Shuffle(testValues);
        foreach (int target in testValues)
        {
            testList = TestParens(target);
            testList.Sort();
            warriorResult = Balanced.BalancedParens(target);
            warriorResult.Sort();
            Assert.AreEqual(testList, warriorResult);
            Console.WriteLine("Value of n = " + target);
        }
    }

    private static void Shuffle<T>(List<T> deck)
    {
        var rnd = new Random();
        for (int n = deck.Count - 1; n > 0; --n)
        {
            int k = rnd.Next(n + 1);
            T temp = deck[n];
            deck[n] = deck[k];
            deck[k] = temp;
        }
    }

    private static List<string> TestParens(int n)
    {
        var lst = new List<string>();
        var sb = new StringBuilder();
        Dfs(sb, lst, 0, 0, n);
        return lst;
    }
    private static void Dfs(StringBuilder sb, List<string> lst, int open, int close, int max)
    {
        if (close == max)
        {
            lst.Add(sb.ToString());
            return;
        }
        if (open > close)
        {
            sb.Append(')');
            Dfs(sb, lst, open, close + 1, max);
            sb.Remove(sb.Length - 1, 1);
        }
        if (open < max)
        {
            sb.Append('(');
            Dfs(sb, lst, open + 1, close, max);
            sb.Remove(sb.Length - 1, 1);
        }
    }
}