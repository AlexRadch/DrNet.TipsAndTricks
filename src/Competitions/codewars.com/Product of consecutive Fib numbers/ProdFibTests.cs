using System;
using NUnit.Framework;

[TestFixture]
public class ProdFibTests
{

    [Test]
    public void Test1()
    {
        ulong[] r = new ulong[] { 55, 89, 1 };
        Assert.AreEqual(r, ProdFib.productFib(4895));
    }
    [Test]
    public void Test2()
    {
        ulong[] r = new ulong[] { 89, 144, 0 };
        Assert.AreEqual(r, ProdFib.productFib(5895));
    }
    [Test]
    public void Test3()
    {
        ulong[] r = new ulong[] { 6765, 10946, 1 };
        Assert.AreEqual(r, ProdFib.productFib(74049690));
    }
    [Test]
    public void Test4()
    {
        ulong[] r = new ulong[] { 10946, 17711, 0 };
        Assert.AreEqual(r, ProdFib.productFib(84049690));
    }
    [Test]
    public void Test5()
    {
        ulong[] r = new ulong[] { 10946, 17711, 1 };
        Assert.AreEqual(r, ProdFib.productFib(193864606));
    }
    [Test]
    public void Test6()
    {
        ulong[] r = new ulong[] { 610, 987, 0 };
        Assert.AreEqual(r, ProdFib.productFib(447577));
    }
    [Test]
    public void Test7()
    {
        ulong[] r = new ulong[] { 610, 987, 1 };
        Assert.AreEqual(r, ProdFib.productFib(602070));
    }
    [Test]
    public void Test8()
    {
        ulong[] r = new ulong[] { 832040, 1346269, 0 };
        Assert.AreEqual(r, ProdFib.productFib(602070602070));
    }
    [Test]
    public void Test9()
    {
        ulong[] r = new ulong[] { 832040, 1346269, 1 };
        Assert.AreEqual(r, ProdFib.productFib(1120149658760));
    }
    [Test]
    public void Test10()
    {
        ulong[] r = new ulong[] { 1836311903, 2971215073, 0 };
        Assert.AreEqual(r, ProdFib.productFib(2563195080744681828));
    }
    [Test]
    public void Test11()
    {
        ulong[] r = new ulong[] { 1346269, 2178309, 1 };
        Assert.AreEqual(r, ProdFib.productFib(2932589879121));
    }
    [Test]
    public void Test12()
    {
        ulong[] r = new ulong[] { 1, 1, 1 };
        Assert.AreEqual(r, ProdFib.productFib(1));
    }
    [Test]
    public void Test13()
    {
        ulong[] r = new ulong[] { 2971215073, 4807526976, 0 };
        Assert.AreEqual(r, ProdFib.productFib(5456077604922913920));
    }

    [Test]
    public void RandomTests()
    {
        ulong[] someFibs = {55,89,144,233,377,610,987,1597,2584,4181,6765,
                       10946,17711,28657,46368,75025,121393,196418,317811,514229,
                       832040,1346269,2178309,3524578,5702887,9227465,14930352,
                       24157817,39088169,63245986};

        Random rnd = new Random();
        for (int k = 0; k < 15; k++)
        {
            int rn = rnd.Next(0, 28);
            ulong f1 = someFibs[rn];
            ulong f2 = someFibs[rn + 1];
            ulong p = f1 * f2;
            ulong[] r = new ulong[] { f1, f2, 1UL };
            Assert.AreEqual(r, ProdFib.productFib(p));
        }
    }
}


