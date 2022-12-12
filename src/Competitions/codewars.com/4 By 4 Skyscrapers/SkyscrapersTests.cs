using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

[TestFixture]
public class SkyscrapersTests
{
    [TestCase(1)]
    [TestCase(2)]
    public void SolvePuzzleTest(int test)
    {
        var actual = Skyscrapers.SolvePuzzle(ett3ewedfa(test));
        CollectionAssert.AreEqual(gfff345ddd(test), actual);
    }

    [Test]
    public void RandomizedSolvePuzzleTest()
    {
        var random = new Random((int)DateTime.Now.Ticks);
        var tests = Enumerable.Range(0, 11).ToList<int>();

        try
        {
            while (tests.Count > 0)
            {
                var n = tests[random.Next(0, tests.Count)];
                tests.Remove(n);
                var test = n % 3 + 1;
                var turn = n / 4;
                var clues = TurnClues(turn, ett3ewedfa(test));
                var expected = TurnExpected(turn, gfff345ddd(test));
                CollectionAssert.AreEqual(expected, Skyscrapers.SolvePuzzle(clues));
            }
        }
        catch (NUnit.Framework.AssertionException ex)
        {
            Assert.Fail("Incorrect solution.");
        }
        catch (Exception ex)
        {
            throw new Exception("Error during executing.");
        }
    }

    private int[] TurnClues(int x, int[] clues)
    {
        var turnedClues = clues.Skip(x * 4).Take(((4 - x) * 4)).ToList();
        turnedClues.AddRange(clues.ToList().Take(x * 4));
        return turnedClues.ToArray();
    }

    private int[][] TurnExpected(int x, int[][] expected)
    {
        var turnedExpected = new int[4][];
        for (var i = 0; i < 4; i++)
        {
            turnedExpected[i] = new int[4];
            for (var j = 0; j < 4; j++)
            {
                if (x == 0) turnedExpected[i][j] = expected[i][j];
                if (x == 1) turnedExpected[i][j] = expected[j][3 - i];
                if (x == 2) turnedExpected[i][j] = expected[3 - i][3 - j];
                if (x == 3) turnedExpected[i][j] = expected[3 - j][i];
            }
        }
        return turnedExpected;
    }

    private int[] ett3ewedfa(int test)
    {
        if (test == 1) return new[]{ 2, 2, 1, 3,
                                     2, 2, 3, 1,
                                     1, 2, 2, 3,
                                     3, 2, 1, 3};
        if (test == 2) return new[]{ 0, 0, 1, 2,
                                                 0, 2, 0, 0,
                                                 0, 3, 0, 0,
                                                 0, 1, 0, 0};
        if (test == 3) return new[]{ 1, 2, 4, 2,
                                     2, 1, 3, 2,
                                     3, 1, 2, 3,
                                     3, 2, 2, 1};
        throw new Exception("Unknown test");
    }

    private int[][] gfff345ddd(int test)
    {
        if (test == 1) return new[]{new []{1, 3, 4, 2},
                                    new []{4, 2, 1, 3},
                                    new []{3, 4, 2, 1},
                                    new []{2, 1, 3, 4 }};
        if (test == 2) return new[]{new []{2, 1, 4, 3},
                                    new []{3, 4, 1, 2},
                                    new []{4, 2, 3, 1},
                                    new []{1, 3, 2, 4}};
        if (test == 3) return new[]{new []{4, 2, 1, 3},
                                                  new []{3, 1, 2, 4},
                                                  new []{1, 4, 3, 2},
                                    new []{2, 3, 4, 1}};
        throw new Exception("Unknown test");
    }
}
