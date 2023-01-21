namespace LinearSystems
{
    using System;

    //It's a reduced Version of my N x M Kata (which came long before...)
    //so not everything is necessary here and not everything is really used
    //Normally i would implement some things totally different for this reduced kata - but it was easier (quicker) to reduce parts 

    public static class Tests
    {
        public static string testIt_adf02ad6_a208_4f19_9313_8de99e582298(string input, string solution)
        {
            return testIt(input, solution);
        }

        public static string testIt(string input, string solution)
        {
            try
            {
                string sol = solution.Replace(" ", "").ToUpper();
                if (!sol.StartsWith("SOLUTION=")) throw new Exception("#'SOLUTION=' missing at the beginning");
                sol = sol.Substring(9);
                string[] parts = sol.Split(new[] { "+" }, StringSplitOptions.RemoveEmptyEntries);
                Test__Solver t = new Test__Solver();
                int dim = t.getSolutionDimension_forTesting(input);
                if (dim >= 0 && parts.Length - 1 != dim) throw new Exception("#Dimension of Solution <> " + dim);
                if (dim == -1 && sol != "NONE") throw new Exception("#Wrong Dimension, wrong Solution");

                int unknowns = input.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length - 1;
                double[,] solMat = new double[unknowns, parts.Length]; //remember all solution matrix numbers for later instert into equations
                for (int i = 0; i < parts.Length; i++)
                {
                    if (!parts[i].StartsWith("(") || !parts[i].EndsWith(")")) break;
                    parts[i] = parts[i].Substring(1, parts[i].Length - 2);
                    string[] numbers = parts[i].Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    if (numbers.Length != unknowns) throw new Exception("#Part of Solution (Vector " + (i + 1) + ") isn't ok: wrong amount of numbers (<> " + unknowns + " unknowns)");
                    int nullVector = 0, countN = 0;
                    foreach (string s1 in numbers) //reduced fraction check
                    {
                        string s = s1.Trim();
                        if ((solMat[countN, i] = double.Parse(s)) == 0) nullVector++;
                        countN++;
                    }
                }

                //Last important tests, inserting results into equations by choosing different qi's
                //double[,] solMat = new double[unknowns, parts.Length]; //contains solution matrix for calculation with equations
                if (dim >= 0)
                {
                    double[,] lm = null;
                    t.transformInput(input, ref lm); //contains equations in matrix form, values as doubles
                    int numEq = input.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length; //number of equations 
                    for (int eq = 0; eq < numEq; eq++) //look at every equation
                    {
                        double mat = 0, matInside;
                        for (int j = 0; j < unknowns; j++)
                        {
                            matInside = 0;
                            for (int i = 0; i < parts.Length; i++) matInside += solMat[j, i] * (i + 1);
                            mat += lm[j, eq] * matInside;
                        }
                        if (Math.Abs(mat - lm[unknowns, eq]) > 1e-6) throw new Exception("#Your solution isn't correct: Equation " + (eq + 1) + " isn't fullfild");
                    }
                }

                return ""; //everything is ok:-)!
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("#")) return "ERROR: '" + solution + "' is not ok!\r\nREASON: " + ex.Message.Substring(1);
                return "INTERNAL EXCEPTION\r\nSolution '" + solution + "' is not ok: " + ex.Message;
            }
        }
    }


    //Parts of original Solution Class used now for testing arbitrary Linear Systems
    public class Test__Solver //Parts of original Solution Class for testing arbitrary Linear Systems
    {
        //Some vars for calculation and extension
        static double[,] m; //matrix with [unknowns x / result LS , lines y] converted to doubles
        static double value, pivot, helpv;
        static int columns, rows, remCount;
        static int[] remember, en1, en;

        public int getSolutionDimension_forTesting(string input) //returns dimension of solution space
        {
            double[,] lm = null;
            string result = transformInput(input, ref lm); //calculation matrix in m[columns,rows]
            if (result.Length == 0)
            {
                gauss();
                return dimensionLs(); //-1= NONE, 0=Single Solution, >0 Dimension
            }
            return -9999; //for Error
        }

        //converts input string to Linear System (INPUT LIKE "a1 b1 c1 d1 res1\r\na2 b2 c2 d2 res2\r\na3 b3 c3 d3 res3". \r\n for line separation)
        public string transformInput(string input, ref double[,] mret)
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
            mret = (double[,])m.Clone();
            return "";
        }

        private int dimensionLs() //returns dimension of solution space (-1== no solution, 0 only single solution, >0= dimension of solution space)
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

        private int countRealRows() //counts Rows != (0,0,0,...) / null vector
        {
            int count = 0;
            for (int i = 0; i < rows; i++) { for (int j = 0; j < columns; j++) if (m[j, i] != 0) { count++; break; } }
            return count;
        }

        private void gauss() //uses Gauss algorithm to directly get a single solution and to transform LS matrix for later use
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
    }
}