using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

// Texas Hold'em Hands
// https://www.codewars.com/kata/524c74f855025e2495000262
public static class Kata
{
    public static (string type, string[] ranks) Hand(string[] holeCards, string[] communityCards)
    {
        var rankSuits = new IReadOnlyList<char>[CardRanksCount];
        for (var i = 0; i < CardRanksCount; i++)
            rankSuits[i] = new List<char>();

        // Initialize rankCount and suitCount dictionaries
        foreach (string card in holeCards.Concat(communityCards))
            ((IList<char>)rankSuits[CardRank(card)]).Add(CardSuitChar(card));

        foreach (var straight in Straight(rankSuits))
        {
            if (IsFlush(straight, rankSuits))
                return ("straight-flush", StringRanks(straight));
        }

        //foreach (var flush in Flush(rankSuits))
        //    if (IsStraight(flush))
        //        return ("straight-flush", StringRanks(flush));

        {
            if (FourOfAKind(rankSuits).FirstOrDefault() is var fourOfAKind && fourOfAKind is not null)
                return ("four-of-a-kind", StringRanks(fourOfAKind));
        }

        {
            if (FullHouse(rankSuits).FirstOrDefault() is var fullHouse && fullHouse is not null)
                return ("full house", StringRanks(fullHouse));
        }

        {
            if (Flush(rankSuits).FirstOrDefault() is var flush && flush is not null)
                return ("flush", StringRanks(flush));
        }

        {
            if (Straight(rankSuits).FirstOrDefault() is var straight && straight is not null)
                return ("straight", StringRanks(straight));
        }

        {
            if (ThreeOfAKind(rankSuits).FirstOrDefault() is var threeOfAKind && threeOfAKind is not null)
                return ("three-of-a-kind", StringRanks(threeOfAKind));
        }

        {
            if (TwoPair(rankSuits).FirstOrDefault() is var twoPair && twoPair is not null)
                return ("two pair", StringRanks(twoPair));
        }

        {
            if (Pair(rankSuits).FirstOrDefault() is var pair && pair is not null)
                return ("pair", StringRanks(pair));
        }

        {
            if (Nothing(rankSuits).FirstOrDefault() is var nothing && nothing is not null)
                return ("nothing", StringRanks(nothing));
        }

        return ("", new string[0]);
    }

    public static IEnumerable<IEnumerable<int>> FourOfAKind(IReadOnlyList<IEnumerable<char>> ranksCounts)
    {
        const int len1 = 4;
        const int len2 = 1;
        for (var i = CardRanksCount - 1; i >= 0; i--)
        {
            if (ranksCounts[i].Count() != len1)
                continue;
            for (var j = CardRanksCount - 1; j >= 0; j--)
                if (j != i && ranksCounts[j].Count() >= len2)
                    yield return new int[] { i, j };
        }
    }

    public static IEnumerable<IEnumerable<int>> FullHouse(IReadOnlyList<IEnumerable<char>> ranksCounts)
    {
        const int len1 = 3;
        const int len2 = 2;
        for (var i = CardRanksCount - 1; i >= 0; i--)
        {
            if (ranksCounts[i].Count() != len1)
                continue;
            for (var j = CardRanksCount - 1; j >= 0; j--)
                if (j != i && ranksCounts[j].Count() >= len2)
                    yield return new int[] { i, j };
        }
    }

    public static IEnumerable<IEnumerable<int>> Flush(IReadOnlyList<IEnumerable<char>> ranksCounts)
    {
        var cards = ranksCounts.Select((suits, rank) => (rank, suits)).OrderByDescending(ranksCounts => ranksCounts.rank)
            .SelectMany(ranksCounts => ranksCounts.suits.Select(suit => (ranksCounts.rank, suit))).ToArray();
        for (var i1 = 0; i1 < cards.Length; i1++)
        {
            var card1 = cards[i1];
            for (var i2 = i1 + 1; i2 < cards.Length; i2++)
            {
                var card2 = cards[i2];
                if (card2.suit != card1.suit)
                    continue;
                for (var i3 = i2 + 1; i3 < cards.Length; i3++)
                {
                    var card3 = cards[i3];
                    if (card3.suit != card1.suit)
                        continue;
                    for (var i4 = i3 + 1; i4 < cards.Length; i4++)
                    {
                        var card4 = cards[i4];
                        if (card4.suit != card1.suit)
                            continue;
                        for (var i5 = i4 + 1; i5 < cards.Length; i5++)
                        {
                            var card5 = cards[i5];
                            if (card5.suit != card1.suit)
                                continue;
                            yield return new int[] { card1.rank, card2.rank, card3.rank, card4.rank, card5.rank, };
                        }
                    }
                }
            }
        }
    }

    private static bool IsFlush(IEnumerable<int> straightRanks, IReadOnlyList<IEnumerable<char>> ranksCounts)
    {
        foreach (var firstSuit in ranksCounts[straightRanks.First()])
        {
            if (straightRanks.Skip(1).All(nextRank => ranksCounts[nextRank].Contains(firstSuit)))
                return true;
        }
        return false;
    }

    public static IEnumerable<IEnumerable<int>> Straight(IReadOnlyList<IEnumerable<char>> ranksCounts)
    {
        for (var i = CardRanksCount - 1; i >= StraightLen - 1; i--)
        {
            if (ranksCounts[i].Count() <= 0)
                continue;
            for (var j = i - 1; j > i - StraightLen; j--)
                if (ranksCounts[j].Count() <= 0)
                    goto NotFound;
            yield return Enumerable.Range(i - StraightLen + 1, StraightLen).Reverse();
        NotFound:;
        }
    }

    private static bool IsStraight(IEnumerable<int> orderedRanks)
    {
        var rankP = 0;
        foreach (var rank in orderedRanks)
        {
            if (rankP != 0 && rank != --rankP)
                return false;
            rankP = rank;
        }
        return true;
    }

    public static IEnumerable<IEnumerable<int>> ThreeOfAKind(IReadOnlyList<IEnumerable<char>> ranksCounts)
    {
        const int len1 = 3;
        const int len2 = 1;
        const int len3 = 1;
        for (var i = CardRanksCount - 1; i >= 0; i--)
        {
            if (ranksCounts[i].Count() != len1)
                continue;
            for (var j = CardRanksCount - 1; j >= 0; j--)
            {
                if (j == i || ranksCounts[j].Count() != len2)
                    continue;
                for (var k = j - 1; k >= 0; k--)
                    if (k != i && ranksCounts[k].Count() == len3)
                        yield return new int[] { i, j, k, };
            }
        }
    }

    public static IEnumerable<IEnumerable<int>> TwoPair(IReadOnlyList<IEnumerable<char>> ranksCounts)
    {
        const int len1 = 2;
        const int len2 = 2;
        const int len3 = 1;
        for (var i = CardRanksCount - 1; i >= 0; i--)
        {
            if (ranksCounts[i].Count() != len1)
                continue;
            for (var j = i - 1; j >= 0; j--)
            {
                if (ranksCounts[j].Count() != len2)
                    continue;
                for (var k = CardRanksCount - 1; k >= 0; k--)
                    if (k != i && k != j && ranksCounts[k].Count() >= len3)
                        yield return new int[] { i, j, k, };
            }
        }
    }

    public static IEnumerable<IEnumerable<int>> Pair(IReadOnlyList<IEnumerable<char>> ranksCounts)
    {
        const int len1 = 2;
        const int len2 = 1;
        const int len3 = 1;
        const int len4 = 1;
        for (var i = CardRanksCount - 1; i >= 0; i--)
        {
            if (ranksCounts[i].Count() != len1)
                continue;
            for (var j = CardRanksCount - 1; j >= 0; j--)
            {
                if (j == i || ranksCounts[j].Count() != len2)
                    continue;
                for (var k = j - 1; k >= 0; k--)
                {
                    if (k == i || ranksCounts[k].Count() != len3)
                        continue;
                    for (var l = k - 1; l >= 0; l--)
                    {
                        if (l != i && ranksCounts[l].Count() == len4)
                            yield return new int[] { i, j, k, l, };
                    }
                }
            }
        }
    }

    public static IEnumerable<IEnumerable<int>> Nothing(IReadOnlyList<IEnumerable<char>> ranksCounts)
    {
        for (var i1 = CardRanksCount - 1; i1 >= 0; i1--)
        {
            if (ranksCounts[i1].Count() != 1)
                continue;
            for (var i2 = i1 - 1; i2 >= 0; i2--)
            {
                if (ranksCounts[i2].Count() != 1)
                    continue;
                for (var i3 = i2 - 1; i3 >= 0; i3--)
                {
                    if (ranksCounts[i3].Count() != 1)
                        continue;
                    for (var i4 = i3 - 1; i4 >= 0; i4--)
                    {
                        if (ranksCounts[i4].Count() != 1)
                            continue;
                        for (var i5 = i4 - 1; i5 >= 0; i5--)
                        {
                            if (ranksCounts[i5].Count() == 1)
                                yield return new int[] { i1, i2, i3, i4, i5, };
                        }
                    }
                }
            }
        }
    }

    public const int FlushLen = 5;
    public const int StraightLen = 5;

    public const int CardRanksCount = 15;
    public static char CardRankChar(string card) => card.Length > 2 ? 'T' : card[0];
    public static int CardRank(string card) => CardRank(CardRankChar(card));
    public static int CardRank(char cardCharRank) => "0123456789TJQKA".IndexOf(cardCharRank);
    public static string CardRank(int cardRank) => "0123456789TJQKA"[cardRank] is var rank && rank == 'T' ? "10" : rank.ToString();
    public static string[] StringRanks(IEnumerable<int> ranks) =>
        ranks.Select(rank => CardRank(rank)).ToArray();

    public static char CardSuitChar(string card) => card[card.Length - 1];
}