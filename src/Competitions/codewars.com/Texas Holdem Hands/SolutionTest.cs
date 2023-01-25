namespace Solution
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [TestFixture]
    public class SolutionTest
    {
        #region Sample Tests

        [Test(Description = "Fixed Tests")]
        public void FixedTests()
        {
            // expected: hand name         cards                       input -> hole cards             community cards
            SampleTest(("nothing", new[] { "A", "K", "Q", "J", "9" }), new[] { "K♠", "A♦" }, new[] { "J♣", "Q♥", "9♥", "2♥", "3♦" });
            SampleTest(("pair", new[] { "Q", "K", "J", "9" }), new[] { "K♠", "Q♦" }, new[] { "J♣", "Q♥", "9♥", "2♥", "3♦" });
            SampleTest(("two pair", new[] { "K", "J", "9" }), new[] { "K♠", "J♦" }, new[] { "J♣", "K♥", "9♥", "2♥", "3♦" });
            SampleTest(("three-of-a-kind", new[] { "Q", "J", "9" }), new[] { "4♠", "9♦" }, new[] { "J♣", "Q♥", "Q♠", "2♥", "Q♦" });
            SampleTest(("straight", new[] { "K", "Q", "J", "10", "9" }), new[] { "Q♠", "2♦" }, new[] { "J♣", "10♥", "9♥", "K♥", "3♦" });
            SampleTest(("flush", new[] { "Q", "J", "10", "5", "3" }), new[] { "A♠", "K♦" }, new[] { "J♥", "5♥", "10♥", "Q♥", "3♥" });
            SampleTest(("full house", new[] { "A", "K" }), new[] { "A♠", "A♦" }, new[] { "K♣", "K♥", "A♥", "Q♥", "3♦" });
            SampleTest(("four-of-a-kind", new[] { "2", "3" }), new[] { "2♠", "3♦" }, new[] { "2♣", "2♥", "3♠", "3♥", "2♦" });
            SampleTest(("straight-flush", new[] { "J", "10", "9", "8", "7" }), new[] { "8♠", "6♠" }, new[] { "7♠", "5♠", "9♠", "J♠", "10♠" });
            SampleTest(("straight-flush", new[] { "Q", "J", "10", "9", "8" }), new[] { "Q♥", "10♥" }, new[] { "J♥", "Q♣", "6♥", "8♥", "9♥" });
        }

        private static void SampleTest((string type, string[] ranks) expected, string[] holeCards, string[] communityCards)
        {
            var actual = Act(holeCards, communityCards);
            Verify(expected, actual, holeCards, communityCards);
        }

        #endregion

        private static readonly StringBuilder template = new StringBuilder();
        private static readonly StringBuilder buffer = new StringBuilder();
        private static readonly string[] ranks = new string[] { "A", "K", "Q", "J", "10", "9", "8", "7", "6", "5", "4", "3", "2" };
        private static readonly string[] types = new string[] { "straight-flush", "four-of-a-kind", "full house", "flush", "straight", "three-of-a-kind", "two pair", "pair", "nothing" };
        private static readonly Dictionary<string, int> ranksLookup = ranks.ToDictionary(x => x, x => Array.FindIndex(ranks, y => y == x));
        private static string Show(string str) => $@"""{str}""";
        private static string ShowSeq(IEnumerable<string> seq) => $"[{string.Join(", ", seq.Select(Show))}]";
        private static (string type, string[] ranks) Act(string[] holeCards, string[] communityCards) => Kata.Hand(holeCards.Select(m => m).ToArray(), communityCards.Select(m => m).ToArray());

        private static string Error(string message)
        {
            buffer.Clear();
            buffer.Append(template.ToString());
            buffer.AppendLine($"Error: {message}");
            return buffer.ToString();
        }

        private static void Verify(
            (string type, string[] ranks) expected, (string type, string[] ranks) actual, string[] holeCards, string[] communityCards)
        {
            template.Clear();
            template.AppendLine($"\tHole cards: {ShowSeq(holeCards)}");
            template.AppendLine($"\tCommunity cards: {ShowSeq(communityCards)}");
            template.AppendLine($"Expected: (type: {Show(expected.type)}, ranks: {ShowSeq(expected.ranks)})");
            Assert.IsNotNull(actual.type, Error("Type must not be null"));
            Assert.IsNotNull(actual.ranks, Error("Ranks must not be null"));
            template.AppendLine($"Actual: (type: {Show(actual.type)}, ranks: {ShowSeq(actual.ranks)})");
            Assert.IsTrue(types.Any(x => string.Equals(x, actual.type)),
                Error($"{Show(actual.type)} is not valid, valid options are: {ShowSeq(types)}"));
            Assert.AreEqual(expected.type, actual.type, Error("Type is incorrect"));
            Assert.AreEqual(expected.ranks.Length, actual.ranks.Length, Error("Number of ranks is incorrect"));
            for (var i = 0; i < expected.ranks.Length; i++)
                Assert.IsTrue(ranks.Any(x => string.Equals(x, actual.ranks[i])),
                    Error($"{Show(actual.ranks[i])} is not valid, valid options are: {ShowSeq(ranks)}"));
            for (var i = 0; i < expected.ranks.Length; i++)
                Assert.AreEqual(expected.ranks[i], actual.ranks[i], Error($"Rank at position {i} is incorrect"));
        }
    }
}