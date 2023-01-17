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
}