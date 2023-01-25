using System;
using System.Linq;
using System.Collections.Generic;

// Ranking Poker Hands
// https://www.codewars.com/kata/5739174624fc28e188000465
public readonly struct PokerHand : IComparable<PokerHand>
{
    public const int MinCardValue = 2;
    public const int MaxCardValue = 14;

    public readonly HandRank Rank;
    public readonly IReadOnlyList<int> Counts;

    public PokerHand(string hand)
    {
        var cards = hand.Split(' ');

        var counts = new int[MaxCardValue + 1];
        foreach (var card in cards)
            counts[CardValue(card[0])]++;

        Counts = counts;
        Rank = default;
        Rank = EvaluteHandRank(Suits(cards));
    }

    public Result CompareWith(PokerHand hand) => CompareTo(hand) switch
    {
        < 0 => Result.Loss,
        0 => Result.Tie,
        > 0 => Result.Win,
    };

    public int CompareTo(PokerHand other)
    {
        {
            if (Rank - other.Rank is var result && result != 0)
                return result;
        }
        switch (Rank)
        {
            case HandRank.FourOfKind:
                {
                    if (Counts.IndexOf(4) - other.Counts.IndexOf(4) is var result && result != 0)
                        return result;
                }
                return Counts.IndexOf(1) - other.Counts.IndexOf(1);

            case HandRank.FullHouse:
                {
                    if (Counts.IndexOf(3) - other.Counts.IndexOf(3) is var result && result != 0)
                        return result;
                }
                return Counts.IndexOf(2) - other.Counts.IndexOf(2);

            case HandRank.ThreeOfKind:
                {
                    if (Counts.IndexOf(3) - other.Counts.IndexOf(3) is var result && result != 0)
                        return result;
                }
                break;

            case HandRank.Pair:
                {
                    if (Counts.IndexOf(2) - other.Counts.IndexOf(2) is var result && result != 0)
                        return result;
                }
                break;
        }

        for (var value = MaxCardValue; value >= MinCardValue; value--)
            if (Counts[value] - other.Counts[value] is var result && result != 0)
                return result;

        return 0;
    }

    public static int CardValue(char valueChar) => "23456789TJQKA".IndexOf(valueChar) + 2;

    private HandRank EvaluteHandRank(IEnumerable<char> suits)
    {
        if (IsStraight(Counts))
        {
            if (IsFlush(suits))
                return HandRank.StraightFlush;
            return HandRank.Straight;
        }

        if (IsFourOfAKind(Counts))
            return HandRank.FourOfKind;

        if (IsFullHouse(Counts))
            return HandRank.FullHouse;

        if (IsFlush(suits))
            return HandRank.Flush;

        if (IsThreeOfKind(Counts))
            return HandRank.ThreeOfKind;

        if (IsTwoPairs(Counts))
            return HandRank.TwoPairs;

        if (IsPair(Counts))
            return HandRank.Pair;

        return HandRank.HighCard;
    }

    public static IEnumerable<char> Suits(IEnumerable<string> cards) =>
        cards.Select(card => card[1]);

    public static bool IsStraightFlush(IEnumerable<int> counts, IEnumerable<char> suits) =>
        IsFlush(suits) && IsStraight(counts);

    public static bool IsFourOfAKind(IEnumerable<int> counts) =>
        counts.Any(count => count == 4);

    public static bool IsFullHouse(IEnumerable<int> counts) =>
        IsThreeOfKind(counts) && IsPair(counts);

    public static bool IsFlush(IEnumerable<char> suits) =>
        suits.First() is var first && suits.All(suit => suit == first);

    public static bool IsStraight(IEnumerable<int> counts) => 
        counts.SkipWhile(count => count <= 0).Take(5).All(count => count == 1);

    public static bool IsThreeOfKind(IEnumerable<int> counts) =>
        counts.Any(count => count == 3);

    public static bool IsTwoPairs(IEnumerable<int> counts) =>
        counts.Where(count => count == 2).Count() == 2;

    public static bool IsPair(IEnumerable<int> counts) =>
        counts.Any(count => count == 2);
}

public enum Result
{
    Win,
    Loss,
    Tie
}

public enum HandRank { HighCard, Pair, TwoPairs, ThreeOfKind, Straight, Flush, FullHouse, FourOfKind, StraightFlush, }

public static class Extentions
{
    public static int IndexOf<T>(this IReadOnlyList<T> list, T item, EqualityComparer<T> comparer = null)
    {
        if (comparer is null)
            comparer = EqualityComparer<T>.Default;

        var count = list.Count;
        for (var index = 0; index < count; index++)
            if (comparer.Equals(list[index], item))
                return index;

        return -1;
    }
}