using System;
using NUnit.Framework;

[TestFixture]
public class RomanDecodeTests
{
    [TestCase(1, "I")]
    [TestCase(2, "II")]
    [TestCase(4, "IV")]
    [TestCase(21, "XXI")]
    [TestCase(500, "D")]
    [TestCase(1000, "M")]
    [TestCase(1954, "MCMLIV")]
    [TestCase(1990, "MCMXC")]
    [TestCase(2008, "MMVIII")]
    [TestCase(2014, "MMXIV")]
    public void Test(int expected, string roman)
    {
        Assert.AreEqual(expected, RomanDecode.Solution(roman));
    }
}