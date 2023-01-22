namespace LinearSystems
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class SolvingTests
    {
        [Test]
        public void TestAndVerify1()
        {
            LinearSystem ls = new LinearSystem();
            string input = "1 2 3 4\n6 6 7 8\n9 10 11 12";
            string result = ls.Solve(input);
            //should be SOL=(0; -1; 2)
            //string testResult = Tests.testIt(input, result);
            //if (testResult.Length > 0) Assert.Fail(testResult); else Console.WriteLine("'" + result + "' accepted!");
            if (result != "SOL=(0; -1; 2)") Assert.Fail(result); else Console.WriteLine("'" + result + "' accepted!");
        }

        [Test]
        public void TestAndVerify2()
        {
            LinearSystem ls = new LinearSystem();
            string input = "1 2 3 4";
            string result = ls.Solve(input);
            //should be like SOL=(4; 0; 0) + q1 * (-2; 1; 0) + q2 * (-3; 0; 1)
            //string testResult = Tests.testIt(input, result);
            //if (testResult.Length > 0) Assert.Fail(testResult); else Console.WriteLine("'" + result + "' accepted!");
            if (result != "SOL=(4; 0; 0) + q1 * (-2; 1; 0) + q2 * (-3; 0; 1)") Assert.Fail(result); else Console.WriteLine("'" + result + "' accepted!");
        }

        [Test]
        public void TestAndVerify3()
        {
            LinearSystem ls = new LinearSystem();
            string input = "3/2 1/2 3";
            string result = ls.Solve(input);
            //should be like SOL=(2; 0) + q1 * (-1/3; 1)
            //string testResult = Tests.testIt(input, result);
            //if (testResult.Length > 0) Assert.Fail(testResult); else Console.WriteLine("'" + result + "' accepted!");
            if (result != "SOL=(2; 0) + q1 * (-1/3; 1)") Assert.Fail(result); else Console.WriteLine("'" + result + "' accepted!");
        }

        [Test]
        public void TestAndVerify4()
        {
            LinearSystem ls = new LinearSystem();
            string input = "1 2 2\n1 2 2\n2 4 4";
            string result = ls.Solve(input);
            //should be like SOL = (2; 0) + q1 * (-2; 1)
            //string testResult = Tests.testIt(input, result);
            //if (testResult.Length > 0) Assert.Fail(testResult); else Console.WriteLine("'" + result + "' accepted!");
            if (result != "SOL=(2; 0) + q1 * (-2; 1)") Assert.Fail(result); else Console.WriteLine("'" + result + "' accepted!");
        }

        [Test]
        public void TestAndVerify5()
        {
            LinearSystem ls = new LinearSystem();
            string input = "0 0 0\n0 0 0";
            string result = ls.Solve(input);
            //should be like SOL = (0; 0) + q1 * (1; 0) + q2 * (0; 1)
            //string testResult = Tests.testIt(input, result);
            //if (testResult.Length > 0) Assert.Fail(testResult); else Console.WriteLine("'" + result + "' accepted!");
            if (result != "SOL=(0; 0) + q1 * (1; 0) + q2 * (0; 1)") Assert.Fail(result); else Console.WriteLine("'" + result + "' accepted!");
        }

        [Test]
        public void TestAndVerify6()
        {
            LinearSystem ls = new LinearSystem();
            string input = "0 0 0 0\n0 0 0 0";
            string result = ls.Solve(input);
            //should be like SOL = (0; 0; 0) + q1 * (1; 0; 0) + q2 * (0; 1; 0) + q3 * (0; 0; 1)
            //string testResult = Tests.testIt(input, result);
            //if (testResult.Length > 0) Assert.Fail(testResult); else Console.WriteLine("'" + result + "' accepted!");
            if (result != "SOL=(0; 0; 0) + q1 * (1; 0; 0) + q2 * (0; 1; 0) + q3 * (0; 0; 1)") Assert.Fail(result); else Console.WriteLine("'" + result + "' accepted!");
        }

        [Test]
        public void TestAndVerify7()
        {
            LinearSystem ls = new LinearSystem();
            string input = "0 0 1 2 1\n1 2 1 3 1\n1 2 2 5 3";
            string result = ls.Solve(input);
            //should be SOL=NONE
            //string testResult = Tests.testIt(input, result);
            //if (testResult.Length > 0) Assert.Fail(testResult); else Console.WriteLine("'" + result + "' accepted!");
            if (result != "SOL=NONE") Assert.Fail(result); else Console.WriteLine("'" + result + "' accepted!");
        }

        [Test]
        public void TestAndVerify8()
        {
            LinearSystem ls = new LinearSystem();
            string input = "1/20 -10/3 -10/9 -13\n-29 8 -27/4 0\n-26 -14 25 10/7";
            string result = ls.Solve(input);
            //should be SOL=(3343180/9270107; 4197595/1324301; 20461200/9270107)
            //string testResult = Tests.testIt(input, result);
            //if (testResult.Length > 0) Assert.Fail(testResult); else Console.WriteLine("'" + result + "' accepted!");
            if (result != "SOL=(3343180/9270107; 4197595/1324301; 20461200/9270107)") Assert.Fail(result); else Console.WriteLine("'" + result + "' accepted!");
        }

        [Test]
        public void TestAndVerify9()
        {
            LinearSystem ls = new LinearSystem();
            string input = "1 2 0 0 7\n0 3 4 0 8\n0 0 5 6 9";
            string result = ls.Solve(input);
            //should be SOL=(97/15; 4/15; 9/5; 0) + q1 * (-16/5; 8/5; -6/5; 1)
            //string testResult = Tests.testIt(input, result);
            //if (testResult.Length > 0) Assert.Fail(testResult); else Console.WriteLine("'" + result + "' accepted!");
            if (result != "SOL=(97/15; 4/15; 9/5; 0) + q1 * (-16/5; 8/5; -6/5; 1)") Assert.Fail(result); else Console.WriteLine("'" + result + "' accepted!");
        }

        [Test]
        public void TestAndVerify10()
        {
            LinearSystem ls = new LinearSystem();
            string input = "0 0 1 2 1\n1 2 1 3 1\n1 2 2 5 2";
            string result = ls.Solve(input);
            //should be SOL=(0; 1; 0; 0) + q1 * (-2; 0; 1; 0) + q2 * (-1; -2; 0; 1)
            //string testResult = Tests.testIt(input, result);
            //if (testResult.Length > 0) Assert.Fail(testResult); else Console.WriteLine("'" + result + "' accepted!");
            if (result != "SOL=(0; 1; 0; 0) + q1 * (-2; 0; 1; 0) + q2 * (-1; -2; 0; 1)") Assert.Fail(result); else Console.WriteLine("'" + result + "' accepted!");
        }
    }

}
