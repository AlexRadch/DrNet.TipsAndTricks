namespace LinearSystems
{
    using System;
    using System.Numerics;

    public static class Tests
    {
        //testcases:
        //1. Syntax check => "SOL=(;;;)+q1*(;;;)+...+qn(;;;)"?!
        //2. Correct dimension of solution space, check solution matrix (qi vectors= (0;0;0...)?)
        //3. Point or comma included ( => no fractions?!)
        //4. max. reduced fractions?!
        //5. random qi's, for calculating concret solution- vector,
        //   insert into equations and check if correct?!
        //OTHERS???!
        public static string testIt_adf02ad6_a208_4f19_9313_8de99e582298(string input, string solution)
        {
            return testIt(input, solution);
        }

        public static string testIt(string input, string solution) //many things together, not best form here, but ok;-)...
        {
            try
            {
                string sol = solution.Replace(" ", "").ToUpper();
                if (!sol.StartsWith("SOL=")) throw new Exception("#'SOL=' missing at the beginning");
                if (sol.IndexOf(".") >= 0 || sol.IndexOf(",") >= 0) throw new Exception("#Solution contains '.' or ',' (only reduced fractions, no doubles/decimals in solution)");
                sol = sol.Substring(4);
                if (sol.IndexOf("-Q") >= 0) throw new Exception("#Solution contains '-q', always use '+q'");
                string[] parts = sol.Split(new[] { "+" }, StringSplitOptions.RemoveEmptyEntries);
                Test__Solver t = new Test__Solver();
                int dim = t.getSolutionDimension_forTesting(input);
                if (dim >= 0 && parts.Length - 1 != dim) throw new Exception("#Dimension of Solution <> " + dim);
                if (dim == -1)
                {
                    if (sol != "NONE") throw new Exception("#Wrong Dimension, wrong Solution");
                    return "";
                }
                if (sol == "NONE") throw new Exception("#Solution exists");
                int testOk = 0, testC = 0;
                int unknowns = input.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)[0].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length - 1;
                Fraction[,] solMat = new Fraction[unknowns, parts.Length]; //remember all solution matrix numbers for later instert into equations
                Fraction[,] qVecs = new Fraction[unknowns, dim];
                for (int i = 0; i < parts.Length; i++)
                {
                    if (i > 0)
                    {
                        if (!parts[i].StartsWith("Q" + i + "*")) { testOk = i; break; }
                        parts[i] = parts[i].Replace("Q" + i + "*", "");
                    }
                    else
                    {
                        if (parts[i].IndexOf("Q") >= 0) throw new Exception("#Part of Solution isn't ok: Starting vector contains 'q'");
                    }
                    if (!parts[i].StartsWith("(") || !parts[i].EndsWith(")")) { testC = i; break; }
                    parts[i] = parts[i].Substring(1, parts[i].Length - 2);
                    string[] numbers = parts[i].Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    if (numbers.Length != unknowns) throw new Exception("#Part of Solution (Vector " + (i + 1) + ") isn't ok: wrong amount of numbers (<> " + unknowns + " unknowns)");
                    int countN = 0;
                    foreach (string s1 in numbers) //reduced fraction check
                    {
                        string s = s1.Trim();
                        Fraction value = new Fraction(s);
                        solMat[countN, i] = value;
                        if (i > 0) qVecs[countN, i - 1] = value;
                        string rf = value.ToString();
                        if (s != rf)
                            throw new Exception("#Part of Solution (Vector " + (i + 1) + ") isn't ok: no reduced fraction (" + s + "<>" + rf + ")");
                        countN++;
                    }
                }
                if (testOk > 0 && dim > 0) throw new Exception("#Part of Solution (Vector " + (testOk + 1) + ") isn't ok: start 'q" + testOk + "*' missing");
                if (testC > 0 && dim > 0) throw new Exception("#Part of Solution (Vector " + (testC + 1) + ") isn't ok: '(' or ')' at start or end missing");

                int spaceDim = t.getSpaceDimension(qVecs);
                if (spaceDim != dim) throw new Exception("#Vectors spanning the solution space should be linearly independent");

                //Last important tests, inserting results into equations by choosing different qi's
                //double[,] solMat = new double[unknowns, parts.Length]; //contains solution matrix for calculation with equations

                Fraction[,] lm;
                t.transformInput(input, out lm); //contains equations in matrix form, values as Fractions
                int numEq = lm.GetLength(1); //number of equations
                for (int eq = 0; eq < numEq; eq++) //look at every equation
                {
                    Fraction[] res = new Fraction[dim + 1];
                    for (int k = 0; k <= dim; k++) res[k] = new Fraction(0);
                    for (int j = 0; j < unknowns; j++)
                    {
                        for (int k = 0; k <= dim; k++)
                        {
                            res[k] += lm[j, eq] * solMat[j, k];
                        }
                    }
                    bool correct = true;
                    for (int k = 1; k <= dim; k++)
                    {
                        if (res[k] != 0)
                        {
                            correct = false;
                            break;
                        }
                    }
                    if (!correct || res[0] != lm[unknowns, eq])
                        throw new Exception("#Your solution isn't correct: Equation " + (eq + 1) + " isn't fullfild");
                }

                return ""; //everything is ok:-)!
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("#")) return "ERROR: '" + solution + "' is not ok!\r\nREASON: " + ex.Message.Substring(1);
                return "INTERNAL EXCEPTION\r\nSolution '" + solution + "' is not ok: " + ex.Message;
            }
        }

        //Parts of original Solution Class used now for testing arbitrary Linear Systems
        private class Test__Solver
        {
            public int getSpaceDimension(Fraction[,] m)
            {
                m = (Fraction[,])m.Clone();
                gauss(m);
                return countRealRows(m, m.GetLength(0));
            }

            public int getSolutionDimension_forTesting(string input) //returns dimension of solution space
            {
                Fraction[,] lm;
                string result = transformInput(input, out lm); //calculation matrix in m[columns,rows]
                if (result.Length == 0)
                {
                    gauss(lm);
                    return dimensionLs(lm); //-1= NONE, 0=Single Solution, >0 Dimension
                }
                return -9999; //for Error
            }

            //converts input string to Linear System (INPUT LIKE "a1 b1 c1 d1 res1\r\na2 b2 c2 d2 res2\r\na3 b3 c3 d3 res3". \r\n for line separation)
            public string transformInput(string input, out Fraction[,] mret)
            {
                mret = null;
                int columns = 0, rows = 0;
                string[] lines = input.Replace(",", ".").Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] column = lines[i].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (mret == null)
                    {
                        columns = column.Length;
                        rows = lines.Length;
                        mret = new Fraction[columns, rows];
                    }
                    for (int j = 0; j < columns; j++)
                    {
                        mret[j, i] = new Fraction(column[j]);
                    }
                }
                return "";
            }

            private int dimensionLs(Fraction[,] m) //returns dimension of solution space (-1== no solution, 0 only single solution, >0= dimension of solution space)
            {
                int columns = m.GetLength(0);
                int rows = m.GetLength(1);
                int dimension = -1, dim2 = -2, dim1 = countRealRows(m, columns - 1);
                if (dim1 >= 0) dim2 = countRealRows(m, columns);
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

            private int countRealRows(Fraction[,] m, int columns) //counts Rows != (0,0,0,...) / null vector
            {
                int rows = m.GetLength(1);
                int count = 0;
                for (int i = 0; i < rows; i++) { for (int j = 0; j < columns; j++) if (m[j, i] != 0) { count++; break; } }
                return count;
            }

            private void gauss(Fraction[,] m) //uses Gauss algorithm to directly get a single solution and to transform LS matrix for later use
            {
                int columns = m.GetLength(0);
                int rows = m.GetLength(1);
                int i, k, j, nr_pivot;
                Fraction value, pivot;
                int[] remember = new int[columns], en1 = new int[columns], en = new int[columns];

                int remCount = -1;
                for (i = 0; i < columns; i++) remember[i] = i;
                for (k = 0; k < columns - 1 && k < rows; k++)
                {
                    pivot = m[k, k].Abs(); nr_pivot = -1;
                    for (i = k + 1; i < rows; i++) if ((value = m[k, i].Abs()) > pivot) { pivot = value; nr_pivot = i; }
                    if (nr_pivot > -1) swapRows(m, nr_pivot, k);
                    if (m[k, k] == 0)
                    {
                        pivot = m[k, k].Abs(); nr_pivot = -1;
                        for (i = k + 1; i < columns - 1; i++) if ((value = m[i, k].Abs()) > pivot) { pivot = value; nr_pivot = i; }
                        if (nr_pivot > -1)
                        {
                            swapColumns(m, nr_pivot, k);
                            int old = remember[nr_pivot]; remember[nr_pivot] = remember[k]; remember[k] = old;
                            en1[++remCount] = nr_pivot; en[remCount] = k;
                        }
                    }
                    if (m[k, k] != 0)
                        for (i = k + 1; i < rows; i++)
                        {
                            var helpv = m[k, i] / m[k, k];
                            for (j = k; j < columns; j++) if (j == k) m[j, i] = new Fraction(0); else m[j, i] = m[j, i] - helpv * m[j, k];
                        }
                }
            }

            private void swapRows(Fraction[,] m, int nr_pivot, int k) //swaps two matrix rows
            {
                int columns = m.GetLength(0);
                if (k != nr_pivot) for (int i = 0; i < columns; i++) { var helpv = -m[i, k]; m[i, k] = m[i, nr_pivot]; m[i, nr_pivot] = helpv; }
            }

            private void swapColumns(Fraction[,] m, int nr_pivot, int k) //swaps two matrix columns
            {
                int rows = m.GetLength(1);
                if (k != nr_pivot) for (int i = 0; i < rows; i++) { var helpv = m[k, i]; m[k, i] = m[nr_pivot, i]; m[nr_pivot, i] = helpv; }
            }
        }

        // Exact rational arithmetic
        private struct Fraction
        {
            public readonly BigInteger Numerator;
            public readonly BigInteger Denominator;

            public Fraction(BigInteger a, BigInteger b)
            {
                if (b == 0)
                {
                    throw new Exception($"#Bad fraction: division by zero {a}/{b}");
                }
                if (b < 0)
                {
                    a = -a;
                    b = -b;
                }
                BigInteger d = BigInteger.GreatestCommonDivisor(a, b);
                if (d > 1)
                {
                    Numerator = a / d;
                    Denominator = b / d;
                }
                else
                {
                    Numerator = a;
                    Denominator = b;
                }
            }

            public Fraction(long a, long b = 1) : this(new BigInteger(a), new BigInteger(b)) { }

            public Fraction(string str)
            {
                string[] parts = str.Split('/');
                if (parts.Length != 1 && parts.Length != 2)
                {
                    throw new Exception($"#Bad fraction value: {str}");
                }
                Numerator = BigInteger.Parse(parts[0]);
                Denominator = parts.Length == 2 ? BigInteger.Parse(parts[1]) : BigInteger.One;
                if (Denominator == 0)
                {
                    throw new Exception($"#Bad fraction: division by zero {Numerator}/{Denominator}");
                }
                if (Denominator < 0)
                {
                    Numerator = -Numerator;
                    Denominator = -Denominator;
                }
                BigInteger d = BigInteger.GreatestCommonDivisor(Numerator, Denominator);
                if (d > 1)
                {
                    Numerator /= d;
                    Denominator /= d;
                }
            }

            public bool IsZero { get => Numerator.IsZero; }

            public Fraction Abs()
            {
                return Numerator >= 0 ? this : -this;
            }

            public override string ToString()
            {
                return Numerator.IsZero && !Denominator.IsZero ? "0" : Denominator.IsOne ? Numerator.ToString() : $"{Numerator}/{Denominator}";
            }

            public override bool Equals(object obj)
            {
                return (obj as Fraction?) == this;
            }

            public override int GetHashCode()
            {
                return Numerator.GetHashCode() + 31 * Denominator.GetHashCode();
            }

            public static Fraction operator *(Fraction a, Fraction b)
            {
                return new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
            }

            public static Fraction operator /(Fraction a, Fraction b)
            {
                return new Fraction(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
            }

            public static Fraction operator +(Fraction a, Fraction b)
            {
                return new Fraction(a.Numerator * b.Denominator + b.Numerator * a.Denominator, a.Denominator * b.Denominator);
            }

            public static Fraction operator -(Fraction a, Fraction b)
            {
                return new Fraction(a.Numerator * b.Denominator - b.Numerator * a.Denominator, a.Denominator * b.Denominator);
            }

            public static Fraction operator -(Fraction a)
            {
                return new Fraction(-a.Numerator, a.Denominator);
            }

            public static bool operator ==(Fraction a, Fraction b)
            {
                return a.Numerator == b.Numerator && a.Denominator == b.Denominator;
            }

            public static bool operator ==(Fraction a, long b)
            {
                return a.Numerator == b && a.Denominator.IsOne;
            }

            public static bool operator !=(Fraction a, Fraction b)
            {
                return !(a == b);
            }

            public static bool operator !=(Fraction a, long b)
            {
                return !(a == b);
            }

            public static bool operator >(Fraction a, Fraction b)
            {
                return a.Numerator * b.Denominator > b.Numerator * a.Denominator;
            }

            public static bool operator <(Fraction a, Fraction b)
            {
                return a.Numerator * b.Denominator < b.Numerator * a.Denominator;
            }
        }

    }
}