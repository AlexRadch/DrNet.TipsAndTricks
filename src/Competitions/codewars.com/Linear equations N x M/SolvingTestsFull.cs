namespace LinearSystems
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class SolvingTestsFull
    {
        [Test]
        public void FixedTests()
        {
            string[] inputs = new string[]
            {
        "1 2 0 0 7\n0 3 4 0 8\n0 0 5 6 9",
        "1 5/2 1/2 0 4 1/8\n0 5 2 -5/2 6 2",
        "0 0 1 2 1\n1 2 1 3 1\n1 2 2 5 2",
        "0 0 1 2 1\n1 2 1 3 1\n1 2 2 5 3",
        "1 1 2 4 0\n-1 -1 -2 -4 0\n0 1 1 0 0\n0 -1 -1 0 0",
        "0 0 0\n0 0 0",
        "0 0\n0 0\n0 0",
        "0 0 0 0 0\n0 0 0 0 0",
        "1 2 2\n1 2 2\n2 4 4",
        "1 2 2\n1 2 2\n2 4 5",
        "1/20 -10/3 -10/9 -13\n-29 8 -27/4 0\n-26 -14 25 10/7"
            };
            foreach (string input in inputs)
            {
                LinearSystem ls = new LinearSystem();
                string result = ls.Solve(input);
                string testResult = Tests.testIt_adf02ad6_a208_4f19_9313_8de99e582298(input, result);
                if (testResult.Length > 0) Assert.Fail(testResult); else Console.WriteLine("'" + result + "' accepted!");
            }
        }


        [Test]
        public void RandomTests()
        {
            int[] eqs = { 1, 2, 3, 4, 5, 7, 9 };
            int[] vars = { 1, 2, 3, 5, 7, 9, 12 };
            Random rnd = new Random();
            foreach (int i in eqs)
            {
                foreach (int j in vars)
                {
                    Console.WriteLine($"{i} x {j} system");
                    string input = RandomInput(rnd, i, j);
                    LinearSystem ls = new LinearSystem();
                    string result = ls.Solve(input);
                    string testResult = Tests.testIt_adf02ad6_a208_4f19_9313_8de99e582298(input, result);
                    if (testResult.Length > 0) Assert.Fail(testResult); else Console.WriteLine("'" + result + "' accepted!");
                }
            }
        }

        private static int GCD(int a, int b) => a != 0 ? GCD(b % a, a) : b;

        private static string RandomInput(Random rnd, int eqs, int vars, int range = 30)
        {
            string[] res = new string[eqs];
            for (int i = 0; i < eqs; i++)
            {
                string[] row = new string[vars + 1];
                for (int j = 0; j <= vars; j++)
                {
                    int a = rnd.Next(-range, range + 1);
                    int b = rnd.Next(4) > 0 ? 1 : rnd.Next(1, range + 1);
                    if (b > 1)
                    {
                        int d = GCD(Math.Abs(a), b);
                        a /= d;
                        b /= d;
                    }
                    if (b > 1)
                    {
                        row[j] = $"{a}/{b}";
                    }
                    else
                    {
                        row[j] = $"{a}";
                    }
                }
                res[i] = String.Join(' ', row);
            }
            return String.Join('\n', res);
        }

    }
}
