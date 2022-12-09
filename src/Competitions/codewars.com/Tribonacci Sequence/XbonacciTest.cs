using NUnit.Framework;
using System;

[TestFixture]
public class XbonacciTest
{
    private Xbonacci variabonacci;

    [SetUp]
    public void SetUp()
    {
        variabonacci = new Xbonacci();
    }

    [TearDown]
    public void TearDown()
    {
        variabonacci = null;
    }

    [Test]
    public void BasicTests()
    {
        Assert.AreEqual(new double[] { 1, 1, 1, 3, 5, 9, 17, 31, 57, 105 }, variabonacci.Tribonacci(new double[] { 1, 1, 1 }, 10));
        Assert.AreEqual(new double[] { 0, 0, 1, 1, 2, 4, 7, 13, 24, 44 }, variabonacci.Tribonacci(new double[] { 0, 0, 1 }, 10));
        Assert.AreEqual(new double[] { 0, 1, 1, 2, 4, 7, 13, 24, 44, 81 }, variabonacci.Tribonacci(new double[] { 0, 1, 1 }, 10));
        Assert.AreEqual(new double[] { 1, 0, 0, 1, 1, 2, 4, 7, 13, 24 }, variabonacci.Tribonacci(new double[] { 1, 0, 0 }, 10));
        Assert.AreEqual(new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, variabonacci.Tribonacci(new double[] { 0, 0, 0 }, 10));
        Assert.AreEqual(new double[] { 1, 2, 3, 6, 11, 20, 37, 68, 125, 230 }, variabonacci.Tribonacci(new double[] { 1, 2, 3 }, 10));
        Assert.AreEqual(new double[] { 3, 2, 1, 6, 9, 16, 31, 56, 103, 190 }, variabonacci.Tribonacci(new double[] { 3, 2, 1 }, 10));
        Assert.AreEqual(new double[] { 1 }, variabonacci.Tribonacci(new double[] { 1, 1, 1 }, 1));
        Assert.AreEqual(new double[] { }, variabonacci.Tribonacci(new double[] { 300, 200, 100 }, 0));
        Assert.AreEqual(new double[] { 0.5, 0.5, 0.5, 1.5, 2.5, 4.5, 8.5, 15.5, 28.5, 52.5, 96.5, 177.5, 326.5, 600.5, 1104.5, 2031.5, 3736.5, 6872.5, 12640.5, 23249.5, 42762.5, 78652.5, 144664.5, 266079.5, 489396.5, 900140.5, 1655616.5, 3045153.5, 5600910.5, 10301680.5 }, variabonacci.Tribonacci(new double[] { 0.5, 0.5, 0.5 }, 30));
    }

    [Test]
    public void RandomTests()
    {
        double[] sign;
        int n;
        Random r = new Random();
        for (int i = 0; i < 40; i++)
        {
            sign = new double[] { r.Next(0, 20), r.Next(0, 20), r.Next(0, 20) };
            n = r.Next(0, 50);

            Console.WriteLine("Testing for signature: " + string.Join(", ", sign) + " and n: " + n);
            Assert.AreEqual(Soluzionacci(sign, n), variabonacci.Tribonacci(sign, n), "It should work with random inputs too");
        }
    }

    private double[] Soluzionacci(double[] s, int n)
    {
        double[] res = new double[n];
        Array.Copy(s, res, Math.Min(3, n));

        for (int i = 3; i < n; i++)
            res[i] = res[i - 3] + res[i - 2] + res[i - 1];

        return n == 0 ? new double[] { } : res;
    }
}