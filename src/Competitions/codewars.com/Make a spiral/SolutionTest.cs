namespace Solution
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SpiralizorTest
    {

        [Test]
        public void Test00()
        {
            int input = 0;
            int[,] expected = new int[0,0];

            int[,] actual = Spiralizor.Spiralize(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test01()
        {
            int input = 1;
            int[,] expected = new int[,]{
                {1},
            };

            int[,] actual = Spiralizor.Spiralize(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test02()
        {
            int input = 2;
            int[,] expected = new int[,]{
                {1, 1},
                {0, 1},
            };

            int[,] actual = Spiralizor.Spiralize(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test03()
        {
            int input = 3;
            int[,] expected = new int[,]{
                {1, 1, 1},
                {0, 0, 1},
                {1, 1, 1},
            };

            int[,] actual = Spiralizor.Spiralize(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test04()
        {
            int input = 4;
            int[,] expected = new int[,]{
                {1, 1, 1, 1},
                {0, 0, 0, 1},
                {1, 0, 0, 1},
                {1, 1, 1, 1},
            };

            int[,] actual = Spiralizor.Spiralize(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test05()
        {
            int input = 5;
            int[,] expected = new int[,]{
                {1, 1, 1, 1, 1},
                {0, 0, 0, 0, 1},
                {1, 1, 1, 0, 1},
                {1, 0, 0, 0, 1},
                {1, 1, 1, 1, 1}
            };

            int[,] actual = Spiralizor.Spiralize(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test08()
        {
            int input = 8;
            int[,] expected = new int[,]{
                {1, 1, 1, 1, 1, 1, 1, 1},
                {0, 0, 0, 0, 0, 0, 0, 1},
                {1, 1, 1, 1, 1, 1, 0, 1},
                {1, 0, 0, 0, 0, 1, 0, 1},
                {1, 0, 1, 0, 0, 1, 0, 1},
                {1, 0, 1, 1, 1, 1, 0, 1},
                {1, 0, 0, 0, 0, 0, 0, 1},
                {1, 1, 1, 1, 1, 1, 1, 1},
            };

            int[,] actual = Spiralizor.Spiralize(input);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestRandom([ValueSource(nameof(Sizes))] int n)
        {
            int[,] expected = SpiralizorForTests.Spiralize(n);
            int[,] actual = Spiralizor.Spiralize(n);
            Assert.AreEqual(expected, actual);
        }

        private readonly static Random rnd = new Random();
        private static IEnumerable<int> Sizes => Enumerable.Range(5, 50).OrderBy(_ => rnd.Next());

        private class SpiralizorForTests
        {
            public static int[,] Spiralize(int size)
            {

                SpiralizorForTests spiralizor = new SpiralizorForTests(size);
                return spiralizor.Spiralize();
            }

            public int[,] Grid { get; private set; }
            public int N { get; private set; }

            public SpiralizorForTests(int n)
            {
                N = n;
                Grid = new int[N, N];
            }

            private int[,] Spiralize()
            {
                int minX = 0;
                int minY = 0;
                int maxX = N;
                int maxY = N;

                bool keepGoing = true;
                bool firstRun = true;

                while (keepGoing)
                {
                    for (int i = minX; i < maxX; i++)
                    {
                        Grid[minY, i] = 1;
                    }

                    if (!firstRun)
                    {
                        minX += 2;
                    }
                    else
                    {
                        firstRun = false;
                    }

                    for (int i = minY; i < maxY; i++)
                    {
                        Grid[i, maxX - 1] = 1;

                    }

                    for (int i = maxX - 1; i > minX; i--)
                    {
                        Grid[maxY - 1, i] = 1;
                    }

                    minY += 2;
                    for (int i = maxY - 1; i > minY - 1; i--)
                    {
                        Grid[i, minX] = 1;
                    }

                    maxX -= 2;
                    maxY -= 2;
                    if (minX + 2 >= maxX)
                    {
                        keepGoing = false;
                    }
                }

                return Grid;
            }
        }
    }
}
