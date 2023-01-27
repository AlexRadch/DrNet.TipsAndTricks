using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;

namespace CompilerKata
{
    // Tiny Three-Pass Compiler
    // https://www.codewars.com/kata/5265b0885fda8eac5900093b
    public class Compiler
    {
        #region pass1

        public Ast pass1(string prog)
        {
            List<string> tokens = tokenize(prog);
            ReadOnlySpan<string> unparsed = CollectionsMarshal.AsSpan(tokens);
            var args = ArgList(ref unparsed);

            var result = Expression(args, ref unparsed);
            Console.WriteLine(result);
            return result;
        }

        private static ReadOnlyCollection<string> ArgList(ref ReadOnlySpan<string> unparsed)
        {
            var result = new List<string>();
            if (unparsed[0] == "[")
                unparsed = unparsed[1..];
            while (unparsed[0] != "]")
            {
                result.Add(unparsed[0]);
                unparsed = unparsed[1..];
            }
            unparsed = unparsed[1..];
            return new ReadOnlyCollection<string>(result);
        }

        private static Ast Expression(IReadOnlyList<string> args, ref ReadOnlySpan<string> unparsed)
        {
            var expression = Term(args, ref unparsed);
            while (unparsed.Length > 0 && unparsed[0] is var op && (op == "+" || op == "-"))
            {
                unparsed = unparsed[1..];
                expression = new BinOp(op, expression, Term(args, ref unparsed));
            }
            return expression;
        }

        private static Ast Term(IReadOnlyList<string> args, ref ReadOnlySpan<string> unparsed)
        {
            var expression = Factor(args, ref unparsed);
            while (unparsed.Length > 0 && unparsed[0] is var op && (op == "*" || op == "/"))
            {
                unparsed = unparsed[1..];
                expression = new BinOp(op, expression, Factor(args, ref unparsed));
            }
            return expression;
        }

        private static Ast Factor(IReadOnlyList<string> args, ref ReadOnlySpan<string> unparsed)
        {
            if (unparsed.Length > 0 && unparsed[0] is var token)
            {
                unparsed = unparsed[1..];
                if (token == "(")
                {
                    var expression = Expression(args, ref unparsed);
                    if (unparsed.Length > 0)
                        unparsed = unparsed[1..];
                    return expression;
                }
                if (int.TryParse(token, out int n))
                    return new UnOp("imm", n);
                return new UnOp("arg", ((ReadOnlyCollection<string>)args).IndexOf(token));
            }
            throw new InvalidOperationException();
        }

        #endregion

        #region pass2

        public Ast pass2(Ast ast)
        {
            if (ast is BinOp binOp)
            {
                var a = pass2(binOp.a());
                var b = pass2(binOp.b());
                if (a is UnOp aOp && aOp.op() == "imm" && b is UnOp bOp && bOp.op() == "imm")
                {
                    return new UnOp("imm", ast.op() switch
                    {
                        "+" => aOp.n() + bOp.n(),
                        "-" => aOp.n() - bOp.n(),
                        "*" => aOp.n() * bOp.n(),
                        "/" => aOp.n() / bOp.n(),
                        _ => 0,
                    });
                }
                return new BinOp(binOp.op(), a, b);
            }
            return ast;
        }

        #endregion

        #region pass3

        public List<string> pass3(Ast ast)
        {
            var result = new List<string>();
            pass3(ast, result);
            return result;
        }

        private void pass3(Ast ast, List<string> result)
        {
            if (ast is BinOp binOp)
            {
                pass3(binOp.a(), result);
                result.Add("PU"); // push R0 onto the stack
                pass3(binOp.b(), result);
                result.Add("SW"); // swap R0 and R1
                result.Add("PO"); // pop the top value off of the stack into R0
                result.Add(binOp.op() switch
                { 
                    "+" => "AD", // add R1 to R0 and put the result in R0
                    "-" => "SU", // subtract R1 from R0 and put the result in R0
                    "*" => "MU", // multiply R0 by R1 and put the result in R0
                    "/" => "DI", // divide R0 by R1 and put the result in R0
                    _ => "",
                });
            }
            if (ast is UnOp unOp)
            {
                result.Add(unOp.op() switch
                {
                    "imm" => $"IM {unOp.n()}", // load the constant value n into R0
                    "arg" => $"AR {unOp.n()}", // load the n-th input argument into R0
                    _ => "",
                });
            }
        }

        #endregion

        #region tokenize

        private List<string> tokenize(string input)
        {
            List<string> tokens = new List<string>();
            Regex rgxMain = new Regex("\\[|\\]|[-+*/=\\(\\)]|[A-Za-z_][A-Za-z0-9_]*|[0-9]*(\\.?[0-9]+)");
            MatchCollection matches = rgxMain.Matches(input);
            foreach (Match m in matches) tokens.Add(m.Groups[0].Value);
            return tokens;
        }

        #endregion
    }
}