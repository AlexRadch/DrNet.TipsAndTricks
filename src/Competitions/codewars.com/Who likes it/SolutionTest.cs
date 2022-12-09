namespace Solution
{
    using NUnit.Framework;
    using System;
    using System.Linq;

    public static class Solution
    {
        public static string Likes(string[] names)
        {
            switch (names.Length)
            {
                case 0: return "no one likes this"; // :(
                case 1: return $"{names[0]} likes this";
                case 2: return $"{names[0]} and {names[1]} like this";
                case 3: return $"{names[0]}, {names[1]} and {names[2]} like this";
                default: return $"{names[0]}, {names[1]} and {names.Length - 2} others like this";
            }
        }
    }

    [TestFixture]
    public class SolutionTest
    {
        [Test, Description("It should return correct text")]
        public void SampleTest()
        {
            Assert.AreEqual("no one likes this", Kata.Likes(new string[0]));
            Assert.AreEqual("Peter likes this", Kata.Likes(new string[] { "Peter" }));
            Assert.AreEqual("Jacob and Alex like this", Kata.Likes(new string[] { "Jacob", "Alex" }));
            Assert.AreEqual("Max, John and Mark like this", Kata.Likes(new string[] { "Max", "John", "Mark" }));
            Assert.AreEqual("Alex, Jacob and 2 others like this", Kata.Likes(new string[] { "Alex", "Jacob", "Mark", "Max" }));
        }

        private static Random rnd = new Random();
        public static string[] names = new string[100].Select(_ => MakeWord()).ToArray();

        public static string MakeWord() =>
          String.Concat(new char[5].Select(_ => (char)rnd.Next(97, 123)));

        [Test, Description("Should return correct text for 0 names")]
        public void ZeroNameTest()
        {
            Assert.AreEqual(Solution.Likes(new string[0]), Kata.Likes(new string[0]));
        }

        [Test, Description("Should return correct text for 1 name")]
        public void OneNameTest()
        {
            Assert.AreEqual(Solution.Likes(names.Take(1).ToArray()), Kata.Likes(names.Take(1).ToArray()));
        }

        [Test, Description("Should return correct text for 2 names")]
        public void TwoNameTest()
        {
            Assert.AreEqual(Solution.Likes(names.Take(2).ToArray()), Kata.Likes(names.Take(2).ToArray()));
        }

        [Test, Description("Should return correct text for 3 names")]
        public void ThreeNameTest()
        {
            Assert.AreEqual(Solution.Likes(names.Take(3).ToArray()), Kata.Likes(names.Take(3).ToArray()));
        }

        [Test, Description("Should return correct text for 4 or more names")]
        public void FourNameTest()
        {
            // 4 names
            Assert.AreEqual(Solution.Likes(names.Take(4).ToArray()), Kata.Likes(names.Take(4).ToArray()));

            const int Tests = 1000;

            for (int i = 0; i < Tests; ++i)
            {
                names = names.OrderBy(_ => rnd.Next()).ToArray();
                string[] test = names.Take(rnd.Next(0, 101)).ToArray();

                string expected = Solution.Likes(test);
                string actual = Kata.Likes(test);

                Assert.AreEqual(expected, actual);
            }
        }
    }
}