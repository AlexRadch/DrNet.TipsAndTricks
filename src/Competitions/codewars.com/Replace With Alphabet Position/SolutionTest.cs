namespace Solution
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class SolutionTest
    {
        [Test]
        public void SampleTest()
        {
            Assert.AreEqual("20 8 5 19 21 14 19 5 20 19 5 20 19 1 20 20 23 5 12 22 5 15 3 12 15 3 11", Kata.AlphabetPosition("The sunset sets at twelve o' clock."));
            Assert.AreEqual("20 8 5 14 1 18 23 8 1 12 2 1 3 15 14 19 1 20 13 9 4 14 9 7 8 20", Kata.AlphabetPosition("The narwhal bacons at midnight."));
            Assert.AreEqual("", Kata.AlphabetPosition("-.-'"));
        }

        private static Random rnd = new Random();

        public static string solution(string text)
        {
            List<string> result = new List<string>();

            foreach (char c in text)
            {
                if (Char.IsLetter(c))
                {
                    result.Add(((int)Char.ToUpper(c) - 64).ToString());
                }
            }

            return String.Join(" ", result);
        }

        [Test, Description("Random Tests")]
        public void RandomTest()
        {
            const int Tests = 1000;

            for (int i = 0; i < Tests; ++i)
            {
                string text = String.Concat(new char[rnd.Next(20, 100)].Select(_ => (char)rnd.Next(128)));

                string expected = solution(text);
                string actual = Kata.AlphabetPosition(text);

                Assert.AreEqual(expected, actual);
            }
        }
    }
}