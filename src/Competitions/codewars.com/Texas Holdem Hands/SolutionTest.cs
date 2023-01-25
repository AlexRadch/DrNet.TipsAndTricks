namespace Solution
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    [TestFixture]
    public class SolutionTest
    {
        #region Sample Tests

        [Test(Description = "Fixed Tests")]
        public void FixedTests()
        {
            SampleTest(("nothing", new[] { "A", "K", "Q", "J", "9" }), new[] { "K♠", "A♦" }, new[] { "J♣", "Q♥", "9♥", "2♥", "3♦" });
            SampleTest(("pair", new[] { "Q", "K", "J", "9" }), new[] { "K♠", "Q♦" }, new[] { "J♣", "Q♥", "9♥", "2♥", "3♦" });
            SampleTest(("two pair", new[] { "K", "J", "9" }), new[] { "K♠", "J♦" }, new[] { "J♣", "K♥", "9♥", "2♥", "3♦" });
            SampleTest(("three-of-a-kind", new[] { "Q", "J", "9" }), new[] { "4♠", "9♦" }, new[] { "J♣", "Q♥", "Q♠", "2♥", "Q♦" });
            SampleTest(("straight", new[] { "K", "Q", "J", "10", "9" }), new[] { "Q♠", "2♦" }, new[] { "J♣", "10♥", "9♥", "K♥", "3♦" });
            SampleTest(("flush", new[] { "Q", "J", "10", "5", "3" }), new[] { "A♠", "K♦" }, new[] { "J♥", "5♥", "10♥", "Q♥", "3♥" });
            SampleTest(("full house", new[] { "A", "K" }), new[] { "A♠", "A♦" }, new[] { "K♣", "K♥", "A♥", "Q♥", "3♦" });
            SampleTest(("four-of-a-kind", new[] { "2", "3" }), new[] { "2♠", "3♦" }, new[] { "2♣", "2♥", "3♠", "3♥", "2♦" });
            SampleTest(("straight-flush", new[] { "J", "10", "9", "8", "7" }), new[] { "8♠", "6♠" }, new[] { "7♠", "5♠", "9♠", "J♠", "10♠" });
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
            Debug.Assert(holeCards.Concat(communityCards).Distinct().Count() == 7, "Invalid input");
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

        #region Test Cases

        private static readonly string[] suits = new string[] { "♠", "♦", "♥", "♣" };
        private static Dictionary<string, int> stats = new Dictionary<string, int>();

        [Test(Description = "Fixed Edge Case Tests")]
        public void FixedEdgeCaseTests()
        {
            // ace low straight invalidated (kata spec)
            SampleTest(("nothing", new[] { "A", "8", "7", "5", "4" }), new[] { "A♠", "2♦" }, new[] { "3♣", "4♥", "5♥", "7♥", "8♦" });
            // non straight around
            SampleTest(("nothing", new[] { "A", "K", "8", "7", "4" }), new[] { "A♠", "K♦" }, new[] { "3♣", "4♥", "2♥", "7♥", "8♦" });

            // pair on board
            SampleTest(("pair", new[] { "4", "A", "9", "7" }), new[] { "A♠", "2♦" }, new[] { "3♣", "4♥", "9♥", "7♥", "4♦" });
            // pair made with 1 hole card
            SampleTest(("pair", new[] { "4", "A", "10", "9" }), new[] { "A♠", "4♦" }, new[] { "3♣", "4♥", "9♥", "7♥", "10♦" });
            // pair made with 2 hole cards
            SampleTest(("pair", new[] { "4", "A", "10", "9" }), new[] { "4♠", "4♦" }, new[] { "3♣", "A♥", "9♥", "7♥", "10♦" });

            // two pair on board
            SampleTest(("two pair", new[] { "Q", "2", "K" }), new[] { "K♠", "J♦" }, new[] { "Q♣", "Q♥", "9♥", "2♥", "2♦" });
            // two pair made with 1 hole card and 1 pair on board
            SampleTest(("two pair", new[] { "Q", "2", "K" }), new[] { "K♠", "Q♦" }, new[] { "J♣", "Q♥", "9♥", "2♥", "2♦" });
            // two pair made with 2 hole cards
            SampleTest(("two pair", new[] { "Q", "2", "K" }), new[] { "2♠", "Q♦" }, new[] { "J♣", "Q♥", "9♥", "2♥", "K♦" });
            // two pair made with pair in hole cards and 1 pair on board
            SampleTest(("two pair", new[] { "Q", "2", "K" }), new[] { "Q♠", "Q♦" }, new[] { "K♣", "J♥", "9♥", "2♥", "2♦" });
            // two pair made with 2 hole cards, invalidating a 3th pair on board
            SampleTest(("two pair", new[] { "K", "J", "9" }), new[] { "K♠", "J♦" }, new[] { "J♣", "K♥", "9♥", "2♥", "2♦" });

            // three-of-a-kind on board
            SampleTest(("three-of-a-kind", new[] { "Q", "K", "J" }), new[] { "K♠", "J♦" }, new[] { "Q♣", "Q♥", "9♥", "2♥", "Q♦" });
            // three-of-a-kind made with 1 hole card and 1 pair on board
            SampleTest(("three-of-a-kind", new[] { "Q", "K", "J" }), new[] { "K♠", "Q♦" }, new[] { "Q♣", "Q♥", "9♥", "2♥", "J♦" });
            // three-of-a-kind made with 2 hole cards
            SampleTest(("three-of-a-kind", new[] { "Q", "K", "J" }), new[] { "Q♣", "Q♦" }, new[] { "K♠", "Q♥", "9♥", "2♥", "J♦" });

            // board straight cancels out pocket aces
            SampleTest(("straight", new[] { "A", "K", "Q", "J", "10" }), new[] { "A♥", "A♠" }, new[] { "A♣", "K♥", "Q♥", "J♥", "10♦" });
            // super straight
            SampleTest(("straight", new[] { "A", "K", "Q", "J", "10" }), new[] { "A♠", "Q♥" }, new[] { "K♥", "10♠", "J♠", "9♠", "8♦" });
            // high straight
            SampleTest(("straight", new[] { "7", "6", "5", "4", "3" }), new[] { "6♠", "7♥" }, new[] { "3♥", "4♠", "5♠", "10♠", "10♦" });
            // low straight
            SampleTest(("straight", new[] { "6", "5", "4", "3", "2" }), new[] { "2♠", "3♥" }, new[] { "4♥", "5♠", "6♠", "10♠", "10♦" });
            // outside straight
            SampleTest(("straight", new[] { "6", "5", "4", "3", "2" }), new[] { "2♠", "6♥" }, new[] { "4♥", "5♠", "3♠", "10♠", "10♦" });
            // inside straight
            SampleTest(("straight", new[] { "6", "5", "4", "3", "2" }), new[] { "4♠", "3♥" }, new[] { "2♥", "5♠", "6♠", "10♠", "10♦" });
            // interspersed straight
            SampleTest(("straight", new[] { "6", "5", "4", "3", "2" }), new[] { "4♠", "2♥" }, new[] { "3♥", "5♠", "6♠", "10♠", "10♦" });

            // seven deuce runner runner
            SampleTest(("full house", new[] { "2", "7" }), new[] { "7♥", "2♠" }, new[] { "A♣", "K♥", "2♦", "7♣", "2♥" });
            // full house with 2 pairs on board where pockets make the triple
            SampleTest(("full house", new[] { "A", "K" }), new[] { "A♠", "A♦" }, new[] { "K♣", "K♥", "A♥", "Q♥", "Q♦" });
            // full house with 1 pair on board where pockets make the triple
            SampleTest(("full house", new[] { "A", "K" }), new[] { "A♠", "A♦" }, new[] { "K♣", "K♥", "A♥", "J♥", "Q♦" });
            // full house with 1 hole card making triple and other making pair
            SampleTest(("full house", new[] { "K", "A" }), new[] { "A♠", "K♦" }, new[] { "K♣", "K♥", "A♥", "J♥", "Q♦" });
            // full house with better triple than board
            SampleTest(("full house", new[] { "A", "K" }), new[] { "A♠", "A♦" }, new[] { "K♣", "K♥", "A♥", "Q♥", "K♦" });

            // flush and straight combo
            SampleTest(("flush", new[] { "J", "10", "9", "8", "6" }), new[] { "8♠", "6♠" }, new[] { "7♦", "5♠", "9♠", "J♠", "10♠" });
            // power flush
            SampleTest(("flush", new[] { "A", "K", "Q", "J", "9" }), new[] { "A♠", "Q♠" }, new[] { "K♠", "4♠", "J♠", "9♠", "3♠" });

            // four-of-a-kind on board
            SampleTest(("four-of-a-kind", new[] { "A", "K" }), new[] { "K♠", "9♥" }, new[] { "A♥", "A♣", "A♠", "A♦", "3♥" });
            // four-of-a-kind with 1 hole card and triple on board
            SampleTest(("four-of-a-kind", new[] { "A", "K" }), new[] { "K♠", "A♥" }, new[] { "9♥", "A♣", "A♠", "A♦", "3♥" });
            // carré
            SampleTest(("four-of-a-kind", new[] { "A", "K" }), new[] { "A♠", "A♦" }, new[] { "A♥", "A♣", "K♠", "9♥", "3♥" });

            // royal flush
            SampleTest(("straight-flush", new[] { "A", "K", "Q", "J", "10" }), new[] { "A♠", "Q♠" }, new[] { "K♠", "10♠", "J♠", "9♠", "3♦" });

            // regression tests
            SampleTest(("straight", new[] { "6", "5", "4", "3", "2" }), new[] { "3♠", "4♥" }, new[] { "6♣", "5♠", "2♣", "2♦", "3♦" });
            SampleTest(("straight", new[] { "10", "9", "8", "7", "6" }), new[] { "6♣", "10♠" }, new[] { "9♠", "8♦", "5♦", "7♥", "9♦" });
            SampleTest(("straight", new[] { "K", "Q", "J", "10", "9" }), new[] { "2♦", "J♦" }, new[] { "Q♥", "9♠", "K♥", "10♥", "J♥" });
        }

        [Test(Description = "Random Tests (Batch #1)")]
        public void RandomBatch1Tests()
        {
            var rand = new Random((int)(DateTime.Now.Ticks % int.MaxValue));
            var bulkSize = 500;
            for (var i = 0; i < bulkSize; i++)
            {
                var hand = GenerateRandomHand(rand);
                var holeCards = hand.Take(2).ToArray();
                var communityCards = hand.Skip(2).ToArray();
                Test(holeCards, communityCards);
            }
        }

        [Test(Description = "Random Tests (Batch #2)")]
        public void RandomBatch2Tests()
        {
            var rand = new Random((int)(DateTime.Now.Ticks % int.MaxValue));
            var bulkSize = 500;
            for (var i = 0; i < bulkSize; i++)
            {
                do
                {
                    var hand = GenerateRandomHand(rand);
                    var holeCards = hand.Take(2).ToArray();
                    var communityCards = hand.Skip(2).ToArray();
                    var expected = Expect(holeCards, communityCards);

                    if (new[] { "nothing", "pair", "two pair", "three-of-a-kind" }.Contains(expected.type))
                    {
                        continue;
                    }
                    else
                    {
                        Test(holeCards, communityCards);
                        break;
                    }
                } while (true);
            }
        }

        [Test(Description = "Random Tests (Batch #3)")]
        public void RandomBatch3Tests()
        {
            var rand = new Random((int)(DateTime.Now.Ticks % int.MaxValue));
            var hands = new List<string[]>();
            var batchSize = 100;
            for (var i = 0; i < batchSize; i++) hands.Add(GenerateStraightFlush(rand));
            for (var i = 0; i < batchSize; i++) hands.Add(GenerateFourOfAKind(rand));
            for (var i = 0; i < batchSize; i++) hands.Add(GenerateFullHouse(rand));
            for (var i = 0; i < batchSize; i++) hands.Add(GenerateFlush(rand));
            for (var i = 0; i < batchSize; i++) hands.Add(GenerateStraight(rand));
            hands = hands.Select(x => x.OrderBy(y => rand.Next()).ToArray()).OrderBy(x => rand.Next()).ToList();
            foreach (var hand in hands)
            {
                var holeCards = hand.Take(2).ToArray();
                var communityCards = hand.Skip(2).ToArray();
                Test(holeCards, communityCards);
            }
        }

        private static Dictionary<int, (string rank, string suit, int id)> Deck()
        {
            var id = 0;
            var hand = new List<string>();
            return (from suit in suits
                    from rank in ranks
                    select (rank, suit, id: id++)).ToDictionary(x => x.id);
        }

        private static void RemoveSuit(Dictionary<int, (string rank, string suit, int id)> deck, int suit)
        {
            var list = deck.Values.Where(card => card.id / ranks.Length == suit).ToList();
            foreach (var card in list)
            {
                deck.Remove(card.id);
            }
        }

        private static void RemoveRank(Dictionary<int, (string rank, string suit, int id)> deck, int rank)
        {
            var list = deck.Values.Where(card => card.id % ranks.Length == rank).ToList();
            foreach (var card in list)
            {
                deck.Remove(card.id);
            }
        }

        private static (string rank, string suit, int id) RandomCard(Dictionary<int, (string rank, string suit, int id)> deck, Random rand)
        {
            return deck.Skip(rand.Next(0, deck.Count)).First().Value;
        }

        private static string[] GenerateRandomHand(Random rand)
        {
            var hand = new List<string>();
            var deck = Deck();

            while (hand.Count < 7)
            {
                var next = RandomCard(deck, rand);
                deck.Remove(next.id);
                hand.Add($"{next.rank}{next.suit}");
            }

            return hand.ToArray();
        }

        private static string[] GenerateStraightFlush(Random rand)
        {
            var hand = new List<string>();
            var deck = Deck();
            var suit = rand.Next(0, suits.Length);
            var rank = rand.Next(0, ranks.Length - 5);
            var head = suit * ranks.Length + rank;
            // 5 cards make the straight flush
            for (var i = 0; i < 5; i++)
            {
                var current = head + i;
                var card = deck[current];
                deck.Remove(current);
                hand.Add($"{card.rank}{card.suit}");
            }
            // any 2 other cards may be added
            for (var i = 0; i < 2; i++)
            {
                var card = RandomCard(deck, rand);
                deck.Remove(card.id);
                hand.Add($"{card.rank}{card.suit}");
            }
            return hand.ToArray();
        }

        private static string[] GenerateFourOfAKind(Random rand)
        {
            var hand = new List<string>();
            var deck = Deck();
            var rank = rand.Next(0, ranks.Length);
            var head = rank;
            // 4 cards make the four-of-a-kind
            for (var i = 0; i < 4; i++)
            {
                var current = head + i * ranks.Length;
                var card = deck[current];
                deck.Remove(current);
                hand.Add($"{card.rank}{card.suit}");
            }
            // any 3 other cards may be added
            for (var i = 0; i < 3; i++)
            {
                var card = RandomCard(deck, rand);
                deck.Remove(card.id);
                hand.Add($"{card.rank}{card.suit}");
            }
            return hand.ToArray();
        }

        private static string[] GenerateFullHouse(Random rand)
        {
            var hand = new List<string>();
            var deck = Deck();
            var rank = rand.Next(0, ranks.Length);
            var head = rank;
            // 3 cards make the triple
            for (var i = 0; i < 3; i++)
            {
                var current = head + i * ranks.Length;
                var card = deck[current];
                deck.Remove(current);
                hand.Add($"{card.rank}{card.suit}");
            }
            // remaining rank would result in a four-of-a-kind
            RemoveRank(deck, rank);
            // 2 cards make a pair
            var rank2 = Array.IndexOf(ranks, RandomCard(deck, rand).rank);
            var head2 = rank2;
            for (var i = 0; i < 2; i++)
            {
                var current = head2 + i * ranks.Length;
                var card = deck[current];
                deck.Remove(current);
                hand.Add($"{card.rank}{card.suit}");
            }
            // remaining rank would result in a three-of-a-kind
            RemoveRank(deck, rank2);
            // any 2 other cards may be added
            for (var i = 0; i < 2; i++)
            {
                var card = RandomCard(deck, rand);
                deck.Remove(card.id);
                hand.Add($"{card.rank}{card.suit}");
            }
            return hand.ToArray();
        }

        private static string[] GenerateFlush(Random rand)
        {
            var hand = new List<string>();
            var deck = Deck();
            var primaryDeck = Deck();
            var suit = rand.Next(0, suits.Length);
            for (var i = 0; i < 4; i++)
            {
                if (i != suit) RemoveSuit(primaryDeck, i);
            }
            // 5 cards make a flush
            for (var i = 0; i < 5; i++)
            {
                var card = RandomCard(primaryDeck, rand);
                primaryDeck.Remove(card.id);
                deck.Remove(card.id);
                hand.Add($"{card.rank}{card.suit}");
            }
            // any 2 other cards may be added
            // small chance on straight flush, but that's ok
            for (var i = 0; i < 2; i++)
            {
                var card = RandomCard(deck, rand);
                deck.Remove(card.id);
                hand.Add($"{card.rank}{card.suit}");
            }
            return hand.ToArray();
        }

        private static string[] GenerateStraight(Random rand)
        {
            var hand = new List<string>();
            var deck = Deck();
            var rank = rand.Next(0, ranks.Length - 5);
            var head = rank;
            // 5 cards make the straight
            for (var i = 0; i < 5; i++)
            {
                var suit = rand.Next(0, suits.Length);
                var current = head + i + suit * ranks.Length;
                var card = deck[current];
                deck.Remove(current);
                hand.Add($"{card.rank}{card.suit}");
            }
            // any 2 other cards may be added
            // small chance on straight flush, but that's ok
            for (var i = 0; i < 2; i++)
            {
                var card = RandomCard(deck, rand);
                deck.Remove(card.id);
                hand.Add($"{card.rank}{card.suit}");
            }
            return hand.ToArray();
        }

        private static void Test(string[] holeCards, string[] communityCards)
        {
            var expected = Expect(holeCards, communityCards);
            var actual = Act(holeCards, communityCards);
            Verify(expected, actual, holeCards, communityCards);
            if (!stats.TryGetValue(expected.type, out var cnt)) cnt = 0;
            stats[expected.type] = cnt + 1;
        }

        private static (string type, string[] ranks) Expect(string[] holeCards, string[] communityCards)
        {
            var cards = holeCards.Concat(communityCards).Select(Parse).OrderBy(x => ranksLookup[x.rank]).ToArray();
            var cardsByRank = cards.ToLookup(x => x.rank);
            var cardsBySuit = cards.ToLookup(x => x.suit);
            var ans = findStraightFlush();
            if (ans == null) ans = findFourOfAKind();
            if (ans == null) ans = findFullHouse();
            if (ans == null) ans = findFlush();
            if (ans == null) ans = findStraight();
            if (ans == null) ans = findThreeOfAKind();
            if (ans == null) ans = findTwoPair();
            if (ans == null) ans = findPair();
            if (ans == null) ans = findNothing();
            return ans.GetValueOrDefault(default);

            (string rank, string suit) Parse(string card) => (card.Substring(0, card.Length - 1), card.Substring(card.Length - 1, 1));

            (string type, string[] ranks)? findStraightFlush()
            {
                var flush = cardsBySuit.SingleOrDefault(x => x.Count() >= 5)?.ToArray();
                if (flush == null) return null;
                for (var i = 0; i + 4 < flush.Length; i++)
                {
                    var match = true;
                    for (var j = 1; j <= 4; j++)
                    {
                        if (!flush.Any(card => ranksLookup[card.rank] == ranksLookup[flush[i].rank] + j))
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match) return ("straight-flush", Enumerable.Range(0, 5).Select(k => ranks[k + ranksLookup[flush[i].rank]]).ToArray());
                }
                return null;
            }

            (string type, string[] ranks)? findFourOfAKind()
            {
                var t4_cards = cardsByRank.SingleOrDefault(x => x.Count() == 4);
                if (t4_cards == null) return null;
                var t4 = t4_cards.First().rank;
                var h1 = cardsByRank.First(x => x.Key != t4).Key;
                return ("four-of-a-kind", new[] { t4, h1 });
            }

            (string type, string[] ranks)? findFullHouse()
            {
                var t3_set = cardsByRank.Where(x => x.Count() == 3);
                if (!t3_set.Any()) return null;
                var t3 = t3_set.First().First().rank;
                var t2_ranks = cardsByRank.Where(x => x.Count() == 2).Select(x => x.Key).ToList();
                if (t3_set.Count() > 1) t2_ranks.Add(t3_set.Skip(1).First().Key);
                if (!t2_ranks.Any()) return null;
                var t2 = t2_ranks.OrderBy(x => ranksLookup[x]).First();
                return ("full house", new[] { t3, t2 });
            }

            (string type, string[] ranks)? findFlush()
            {
                var flush = cardsBySuit.SingleOrDefault(x => x.Count() >= 5)?.ToArray();
                if (flush == null) return null;
                return ("flush", flush.Take(5).Select(x => x.rank).ToArray());
            }

            (string type, string[] ranks)? findStraight()
            {
                for (var i = 0; i + 4 < cards.Length; i++)
                {
                    var match = true;
                    for (var j = 1; j <= 4; j++)
                    {
                        if (!cards.Any(card => ranksLookup[card.rank] == ranksLookup[cards[i].rank] + j))
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match) return ("straight", Enumerable.Range(0, 5).Select(k => ranks[k + ranksLookup[cards[i].rank]]).ToArray());
                }
                return null;
            }

            (string type, string[] ranks)? findThreeOfAKind()
            {
                var t3_cards = cardsByRank.SingleOrDefault(x => x.Count() == 3);
                if (t3_cards == null) return null;
                var t3 = t3_cards.First().rank;
                var h1 = cardsByRank.First(x => x.Key != t3).Key;
                var h2 = cardsByRank.First(x => x.Key != t3 && x.Key != h1).Key;
                return ("three-of-a-kind", new[] { t3, h1, h2 });
            }

            (string type, string[] ranks)? findTwoPair()
            {
                var t2_set = cardsByRank.Where(x => x.Count() == 2);
                if (t2_set.Count() < 2) return null;
                var t2_high = t2_set.First().First().rank;
                var t2_low = t2_set.Skip(1).First().First().rank;
                var h1 = cardsByRank.First(x => x.Key != t2_high && x.Key != t2_low).Key;
                return ("two pair", new[] { t2_high, t2_low, h1 });
            }

            (string type, string[] ranks)? findPair()
            {
                var t2_cards = cardsByRank.SingleOrDefault(x => x.Count() == 2);
                if (t2_cards == null) return null;
                var t2 = t2_cards.First().rank;
                var h1 = cardsByRank.First(x => x.Key != t2).Key;
                var h2 = cardsByRank.First(x => x.Key != t2 && x.Key != h1).Key;
                var h3 = cardsByRank.First(x => x.Key != t2 && x.Key != h1 && x.Key != h2).Key;
                return ("pair", new[] { t2, h1, h2, h3 });
            }

            (string type, string[] ranks) findNothing()
            {
                return ("nothing", cards.Take(5).Select(x => x.rank).ToArray());
            }
        }

        #endregion
    }
}