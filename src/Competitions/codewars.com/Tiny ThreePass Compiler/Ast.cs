namespace CompilerKata
{
    using System;
    using System.Collections.Generic;
    //datatructures AST and Simulator Class

    public class Ast
    {
        public string _op;
        public int _n = 0;
        public Ast _a = null;
        public Ast _b = null;
        public string op() { return _op; }
    }

    public class BinOp : Ast
    {
        Ast left;
        Ast right;
        public BinOp(string OP, Ast A, Ast B) { _op = OP; _a = A; _b = B; left = _a; right = _b; }
        public Ast a() { return left; }
        public Ast b() { return right; }
    }

    public class UnOp : Ast
    {
        int num;
        public UnOp(string OP, int N) { _op = OP; _n = N; num = _n; }
        public int n() { return num; }
    }

    public class Simulator
    {
        public static int simulate(List<string> asm, int[] argv)
        {
            int r0 = 0;
            int r1 = 0;
            List<int> stack = new List<int>();
            foreach (string ins in asm)
            {
                String code = ins.Substring(0, 2);
                switch (code)
                {
                    case "IM": r0 = Int32.Parse(ins.Substring(2).Trim()); break;
                    case "AR": r0 = argv[Int32.Parse(ins.Substring(2).Trim())]; break;
                    case "SW": int tmp = r0; r0 = r1; r1 = tmp; break;
                    case "PU": stack.Add(r0); break;
                    case "PO": r0 = stack[stack.Count - 1]; stack.RemoveAt(stack.Count - 1); break;
                    case "AD": r0 += r1; break;
                    case "SU": r0 -= r1; break;
                    case "MU": r0 *= r1; break;
                    case "DI": r0 /= r1; break;
                }
            }
            return r0;
        }

        public static string polishNotation;
        public static void nodesToPolishNotation(Ast node) //just for testing Ast==Ast
        {
            if (node.op() != null)
            {
                polishNotation += " " + node.op();
                if (node.op() != "arg" && node.op() != "imm") { nodesToPolishNotation(((BinOp)node).a()); nodesToPolishNotation(((BinOp)node).b()); } else polishNotation += "_" + ((UnOp)node).n();
            }
        }

    }
}
