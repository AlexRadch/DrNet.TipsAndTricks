using System;
using NUnit.Framework;

namespace Solution
{
    [TestFixture]
    public class KataTests
    {
        [Test]
        public void FixedTests()
        {
            Assert.AreEqual("FFFFFF", Kata.Rgb(255, 255, 255));
            Assert.AreEqual("FFFFFF", Kata.Rgb(255, 255, 300));
            Assert.AreEqual("000000", Kata.Rgb(0, 0, 0));
            Assert.AreEqual("9400D3", Kata.Rgb(148, 0, 211));
            Assert.AreEqual("9400D3", Kata.Rgb(148, -20, 211), "Handle negative numbers.");
            Assert.AreEqual("90C3D4", Kata.Rgb(144, 195, 212));
            Assert.AreEqual("D4350C", Kata.Rgb(212, 53, 12), "Consider single hex digit numbers.");
        }

        private static Random rnd = new Random();

        private static string rgb(int r, int g, int b)
        {
            return hex(r) + hex(g) + hex(b);
        }

        private static string hex(int n)
        {
            n = n < 0 ? 0 : n > 255 ? 255 : n;
            return (n < 16 ? "0" : "") + n.ToString("X");
        }

        [Test, Description("Random Tests (100 assertions)")]
        public void RandomTest()
        {
            const int Tests = 100;

            for (int i = 0; i < 100; ++i)
            {
                int r = rnd.Next(-50, 400);
                int g = rnd.Next(-50, 400);
                int b = rnd.Next(-50, 400);
                Console.WriteLine("Testing for {0}, {1}, {2}", r, g, b);

                string expected = rgb(r, g, b);
                string actual = Kata.Rgb(r, g, b);
                Assert.AreEqual(expected, actual);
            }
        }
    }
}