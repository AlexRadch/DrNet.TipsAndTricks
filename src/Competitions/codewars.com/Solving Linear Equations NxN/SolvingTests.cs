namespace LinearSystems
{
    using NUnit.Framework;
    using System;
    using System.Text.RegularExpressions;

    [TestFixture]

    public class LinearSystemO //added some (very) easy new random tests "issue @dcieslak";-), (smile67 15.11.2017)
    {

        static double[,] m;
        static double value, pivot, helpv;
        static int columns, rows, remCount;
        static int[] remember, en1, en;


        public string Solve(string input) //solves LS
        {
            try
            {
                string result = transformInput(input);
                if (result.Length == 0)
                {
                    gauss();
                    result = solutionVector();
                    int dimension = dimensionLs();
                    if (dimension < 0) result = "SOLUTION=NONE";
                }
                return result;
            }
            catch (Exception ex) { return "ERROR: " + ex.Message; }
        }


        private string transformInput(string input)
        {
            m = null;
            string[] lines = input.Replace(".", ",").Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] column = lines[i].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (m == null)
                {
                    if (column.Length != lines.Length + 1) return "INPUT ERROR: NUMBER OF UNKNOWNS != LINES";
                    if ((rows = lines.Length) + 1 != (columns = column.Length)) rows = columns - 1;
                    m = new double[columns, rows];
                }
                for (int j = 0; j < columns; j++)
                {
                    string[] rat = (column[j] + @"/1").Split(new[] { @"/" }, StringSplitOptions.None);
                    m[j, i] = float.Parse(rat[0]) / float.Parse(rat[1]);
                }
            }
            remember = new int[columns]; en1 = new int[columns]; en = new int[columns];
            return "";
        }


        private int dimensionLs() //returns dimension of solution space (-1== no solution, 0 only single solution, >0= dimension of solution space - but here not used)
        {
            columns--;
            int dimension = -1, dim2 = -2, dim1 = countRealRows();
            columns++;
            if (dim1 >= 0) dim2 = countRealRows();
            bool checkNot0 = false;
            for (int i = 0; i < rows; i++) if (m[columns - 1, i] != 0) { checkNot0 = true; break; }
            if (!checkNot0 && dim1 < columns - 1) dimension = columns - 1 - dim1;
            else
            {
                if (!checkNot0 && dim1 == columns - 1) dimension = 0; //only single solution
                else
                    if (dim1 == dim2)
                {
                    if (dim1 == columns - 1) dimension = 0; //only single solution
                    if (dim1 < columns - 1) dimension = columns - 1 - dim1; //dimension of solution space
                }
            } //nothing => =-1 ==> no solution
            return dimension;
        }


        private int countRealRows()
        {
            int count = 0;
            for (int i = 0; i < rows; i++) { for (int j = 0; j < columns; j++) if (m[j, i] != 0) { count++; break; } }
            return count;
        }


        private void gauss()
        {
            int i, k, j, nr_pivot;
            remCount = -1;
            for (i = 0; i < columns; i++) remember[i] = i;
            for (k = 0; k < columns - 2; k++)
            {
                pivot = Math.Abs(m[k, k]); nr_pivot = -1;
                for (i = k + 1; i < rows; i++) if ((value = Math.Abs(m[k, i])) > pivot) { pivot = value; nr_pivot = i; }
                if (nr_pivot > -1) swapRows(nr_pivot, k);
                if (m[k, k] == 0)
                {
                    pivot = Math.Abs(m[k, k]); nr_pivot = -1;
                    for (i = k + 1; i < columns - 1; i++) if ((value = Math.Abs(m[i, k])) > pivot) { pivot = value; nr_pivot = i; }
                    if (nr_pivot > -1)
                    {
                        swapColumns(nr_pivot, k);
                        int old = remember[nr_pivot]; remember[nr_pivot] = remember[k]; remember[k] = old;
                        en1[++remCount] = nr_pivot; en[remCount] = k;
                    }
                }
                if (m[k, k] != 0)
                    for (i = k + 1; i < rows; i++)
                    {
                        helpv = m[k, i] / m[k, k];
                        for (j = k; j < columns; j++) if (j == k) m[j, i] = 0; else m[j, i] = m[j, i] - helpv * m[j, k];
                    }
            }
        }


        private void swapRows(int nr_pivot, int k) //swaps two matrix rows
        {
            if (k != nr_pivot) for (int i = 0; i < columns; i++) { helpv = -m[i, k]; m[i, k] = m[i, nr_pivot]; m[i, nr_pivot] = helpv; }
        }


        private void swapColumns(int nr_pivot, int k) //swaps two matrix columns
        {
            if (k != nr_pivot) for (int i = 0; i < rows; i++) { helpv = m[k, i]; m[k, i] = m[nr_pivot, i]; m[nr_pivot, i] = helpv; }
        }


        private string solutionVector()
        {
            double[] y = new double[columns], x = new double[columns]; x[0] = 0;
            for (int i = rows - 1; i >= 0; i--)
                if (m[i, i] == 0) x[i] = 0;
                else
                {
                    helpv = m[columns - 1, i];
                    for (int j = rows - 1; j > i; j--) helpv = helpv - x[j] * m[j, i];
                    x[i] = helpv / m[i, i];
                }
            for (int i = 0; i < rows; i++) y[remember[i]] = x[i];
            string result = "SOLUTION=(";
            for (int i = 0; i < rows; i++) result += y[i].ToString() + "; ";
            return result.Substring(0, result.Length - 2) + ")";
        }
    }

    public class SolvingTests
    {
        [Test]
        public void TestAndVerify1()
        {
            LinearSystem ls = new LinearSystem();
            string input = "1 2 0 7\r\n0 2 4 8\r\n0 5 6 9";
            string result = ls.Solve(input);
            //should be SOLUTION=(10; -1,5; 2,75)
            string testResult = Tests.testIt(input, result);
            if (testResult.Length > 0) Assert.Fail(testResult); else Console.WriteLine("'" + result + "' accepted!");
        }

        [Test]
        public void TestAndVerify2()
        {
            LinearSystem ls = new LinearSystem();
            string input = "1 2 0 0 4 2 7\r\n0 0 3 4 -4 2 8\r\n0 5 6 0 -1 3 9\r\n1 2 0 0 1 1 7\r\n-2 0 3 4 2 8 8\r\n0 5 6 0 1 2 9";
            string result = ls.Solve(input);
            //should be SOLUTION=(0; 3,5; -1,41666666666667; 3,0625; 0; 0)
            string testResult = Tests.testIt(input, result);
            if (testResult.Length > 0) Assert.Fail(testResult); else Console.WriteLine("'" + result + "' accepted!");

        }

        [Test]
        public void TestAndVerify3()
        {
            LinearSystem ls = new LinearSystem();
            string input = "1 2 0 4 7\r\n0 2 0 2 8\r\n0 0 -1 4 6\r\n1 2 3 2 3";
            string result = ls.Solve(input);
            //should be SOLUTION=(-3,8; 2,6; -0,4; 1,4)
            string testResult = Tests.testIt(input, result);
            if (testResult.Length > 0) Assert.Fail(testResult); else Console.WriteLine("'" + result + "' accepted!");
        }

        [Test]
        public void TestAndVerify4()
        {
            LinearSystem ls = new LinearSystem();
            string input = "1,5 5 -2,5 8 3\r\n-1,5 2,4 -2 4 5\r\n3,5 4 -2 12 10\r\n1 1 -2 -4 3";
            string result = ls.Solve(input);
            //should be SOLUTION=(0,512254798645204; -4,20955896374341; -5,64215706937626; 1,14675249341358)
            string testResult = Tests.testIt(input, result);
            if (testResult.Length > 0) Assert.Fail(testResult); else Console.WriteLine("'" + result + "' accepted!");
        }

        [Test]
        public void TestAndVerify5()
        {
            LinearSystem ls = new LinearSystem();
            string input = "1 2 1\r\n1 2 0";
            string result = ls.Solve(input);
            //should be SOLUTION=NONE
            string testResult = Tests.testIt(input, result);
            if (testResult.Length > 0) Assert.Fail(testResult); else Console.WriteLine("'" + result + "' accepted!");
        }
        [Test]
        public void TestAndVerify6()
        {
            LinearSystem ls = new LinearSystem();
            string input = "1 2 0 7\r\n0 3 4 8\r\n5 6 0 9";
            string result = ls.Solve(input);
            //should be SOLUTION=(-6; 6,5; -2,875)
            string testResult = Tests.testIt(input, result);
            if (testResult.Length > 0) Assert.Fail(testResult); else Console.WriteLine("'" + result + "' accepted!");
        }

        [Test]
        public void SomeRandomTests()
        {
            LinearSystem ls = new LinearSystem();
            LinearSystemO ols = new LinearSystemO();
            Random r = new Random();
            //Correct wrong roundings at the end of each test, results are strings...
            string pattern = @"(\.\d+)";
            Regex rgx = new Regex(pattern);

            for (var i = 0; i < 35; i++)
            {
                string input = "a b c d\r\ne f g h\r\ni j k l";
                int v3 = r.Next(0, 10);
                if (v3 != 1 && v3 != 2 && v3 != 3)
                    for (int j = 0; j < 12; j++)
                    {
                        int v1 = r.Next(-20, 20);
                        input = input.Replace((Char)(j + 97) + "", v1 + "");
                    }
                if (v3 == 1) input = "1 2 0 7\r\n0 3 4 8\r\n5 6 0 9";
                if (v3 == 2) input = "1,5 5 -2,5 8 3\r\n-1,5 2,4 -2 4 5\r\n3,5 4 -2 12 10\r\n1 1 -2 -4 3";
                if (v3 == 3) input = "1 2 1\r\n1 2 0";

                Console.WriteLine("Testing for LS:\r\n" + input);
                string testResult = ols.Solve(input);
                testResult = rgx.Replace(testResult, m => m.Groups[1].Value.Length > 7 ? m.Groups[1].Value.Substring(0, 7) : m.Groups[1].Value);

                string result1, result = ls.Solve(input);
                result1 = rgx.Replace(result, m => m.Groups[1].Value.Length > 7 ? m.Groups[1].Value.Substring(0, 7) : m.Groups[1].Value);
                if (testResult.Replace(" ", "") != result1.Replace(" ", "")) Assert.Fail("Expected (max. 6 decimal places necessary): " + testResult + "\r\nBut got: " + result + "\r\nSo checked: " + result1); else Console.WriteLine("Your Solution was: '" + result + "'\r\nChecked (max. 6 decimal places necessary): '" + result1 + "' accepted!");
                Console.WriteLine("--------------------------------\r\n");
            }
        }

    }
}
