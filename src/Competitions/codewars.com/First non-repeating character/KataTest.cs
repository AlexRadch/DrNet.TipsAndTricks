namespace Solution
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class KataTest
    {
        [Test]
        public void BasicTests()
        {
            Assert.AreEqual("a", Kata.FirstNonRepeatingLetter("a"));
            Assert.AreEqual("t", Kata.FirstNonRepeatingLetter("stress"));
            Assert.AreEqual("e", Kata.FirstNonRepeatingLetter("moonmen"));
        }

        [Test]
        public void EmptyTest()
        {
            Assert.AreEqual("", Kata.FirstNonRepeatingLetter(""));
        }

        [Test]
        public void AllRepeatingTests()
        {
            Assert.AreEqual("", Kata.FirstNonRepeatingLetter("abba"));
            Assert.AreEqual("", Kata.FirstNonRepeatingLetter("aa"));
        }

        [Test]
        public void OddCharactersTest()
        {
            Assert.AreEqual("ﬁ", Kata.FirstNonRepeatingLetter("∞§ﬁ›ﬂ∞§"));
            Assert.AreEqual("w", Kata.FirstNonRepeatingLetter("hello world, eh?"));
        }

        [Test]
        public void CaseLettersTest()
        {
            Assert.AreEqual("T", Kata.FirstNonRepeatingLetter("sTreSS"));
            Assert.AreEqual(",", Kata.FirstNonRepeatingLetter("Go hang a salami, I'm a lasagna hog!"));
        }

        [Test]
        public void RandomTest()
        {
            var rand = new Random();

            Func<string, string> myFirstNonRepeatingLetter = delegate (string s)
            {
                var dict = new Dictionary<char, Tuple<int, char>>();

                for (var i = 0; i < s.Length; i++)
                {
                    if (!(dict.ContainsKey(char.ToLower(s[i]))))
                    {
                        dict[char.ToLower(s[i])] = new Tuple<int, char>(0, s[i]);
                    }
                    var tuple = dict[char.ToLower(s[i])];

                    dict[char.ToLower(s[i])] = new Tuple<int, char>(tuple.Item1 + 1, tuple.Item2);
                }

                foreach (var el in dict.Values)
                {
                    if (el.Item1 == 1)
                    {
                        return new string(el.Item2, 1);
                    }
                }

                return "";
            };

            for (int a = 0; a < 80; a++)
            {
                var length = rand.Next(0, a);

                var checkText = string.Concat(Enumerable.Range(0, length).Select(i => (char)rand.Next(65, 65 + a)));

                Assert.AreEqual(myFirstNonRepeatingLetter(checkText), Kata.FirstNonRepeatingLetter(checkText));
            }
        }
    }
}