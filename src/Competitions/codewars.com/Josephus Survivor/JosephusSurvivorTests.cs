using System;
using NUnit.Framework;

[TestFixture]
public static class JosephusSurvivorTests
{

    private static void testing(int actual, int expected)
    {
        Assert.AreEqual(expected, actual);
    }
    [Test]
    public static void test1()
    {
        Console.WriteLine("Basic Tests JosSurvivor");
        testing(JosephusSurvivor.JosSurvivor(7, 3), 4);
        testing(JosephusSurvivor.JosSurvivor(11, 19), 10);
        testing(JosephusSurvivor.JosSurvivor(40, 3), 28);
        testing(JosephusSurvivor.JosSurvivor(14, 2), 13);
        testing(JosephusSurvivor.JosSurvivor(100, 1), 100);
        testing(JosephusSurvivor.JosSurvivor(1, 300), 1);
        testing(JosephusSurvivor.JosSurvivor(2, 300), 1);
        testing(JosephusSurvivor.JosSurvivor(5, 300), 1);
        testing(JosephusSurvivor.JosSurvivor(7, 300), 7);
        testing(JosephusSurvivor.JosSurvivor(300, 300), 265);
    }
    //-----------------------
    private static Random rnd = new Random();

    public static int JosSurvivorSol(int n, int k)
    {
        if (n == 1)
            return 1;
        return (JosSurvivorSol(n - 1, k) + k - 1) % n + 1;
    }
    //-----------------------
    private static void test2()
    {
        for (int i = 0; i < 40; i++)
        {
            int n = rnd.Next(1, 5000);
            int k = rnd.Next(1, 5000);
            testing(JosephusSurvivor.JosSurvivor(n, k), JosSurvivorSol(n, k));
        }
    }
    [Test]
    public static void RandomTests()
    {
        Console.WriteLine("Random Tests******* JosSurvivor");
        test2();
    }
}