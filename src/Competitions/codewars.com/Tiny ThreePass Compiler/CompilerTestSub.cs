namespace CompilerKata
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class CompilerTestSub
    {
        private Compiler compiler = new Compiler();

        [Test]
        public void testSimpleProg()
        {
            // {'op':'/','a':{'op':'-','a':{'op':'+','a':{'op':'*','a':{'op':'*','a':{'op':'imm','n':2},'b':{'op':'imm','n':3}},'b':{'op':'arg','n':0}},'b':{'op':'*','a':{'op':'imm','n':5},'b':{'op':'arg','n':1}}},'b':{'op':'*','a':{'op':'imm','n':3},'b':{'op':'arg','n':2}}},'b':{'op':'+','a':{'op':'+','a':{'op':'imm','n':1},'b':{'op':'imm','n':3}},'b':{'op':'*','a':{'op':'imm','n':2},'b':{'op':'imm','n':2}}}}
            string prog = "[ x y z ] ( 2*3*x + 5*y - 3*z ) / (1 + 3 + 2*2)";
            Console.WriteLine("Testing: " + prog);
            Ast t1 = new BinOp("/", new BinOp("-", new BinOp("+", new BinOp("*", new BinOp("*", new UnOp("imm", 2), new UnOp("imm", 3)), new UnOp("arg", 0)), new BinOp("*", new UnOp("imm", 5), new UnOp("arg", 1))), new BinOp("*", new UnOp("imm", 3), new UnOp("arg", 2))), new BinOp("+", new BinOp("+", new UnOp("imm", 1), new UnOp("imm", 3)), new BinOp("*", new UnOp("imm", 2), new UnOp("imm", 2))));
            Ast p1 = compiler.pass1(prog);

            Simulator.polishNotation = "";
            Simulator.nodesToPolishNotation(t1);
            string pNt1 = Simulator.polishNotation;

            Simulator.polishNotation = "";
            Simulator.nodesToPolishNotation(p1);
            string pNp1 = Simulator.polishNotation;
            if (pNt1 != pNp1) Assert.Fail("t1 != p1, wrong solution for pass1, aborted!"); else Console.WriteLine("Pass1 was ok!");

            // {'op':'/','a':{'op':'-','a':{'op':'+','a':{'op':'*','a':{'op':'imm','n':6},'b':{'op':'arg','n':0}},'b':{'op':'*','a':{'op':'imm','n':5},'b':{'op':'arg','n':1}}},'b':{'op':'*','a':{'op':'imm','n':3},'b':{'op':'arg','n':2}}},'b':{'op':'imm','n':8}}
            Ast t2 = new BinOp("/", new BinOp("-", new BinOp("+", new BinOp("*", new UnOp("imm", 6), new UnOp("arg", 0)), new BinOp("*", new UnOp("imm", 5), new UnOp("arg", 1))), new BinOp("*", new UnOp("imm", 3), new UnOp("arg", 2))), new UnOp("imm", 8));
            Ast p2 = compiler.pass2(p1);

            Simulator.polishNotation = "";
            Simulator.nodesToPolishNotation(t2);
            string pNt2 = Simulator.polishNotation;

            Simulator.polishNotation = "";
            Simulator.nodesToPolishNotation(p2);
            string pNp2 = Simulator.polishNotation;
            if (pNt2 != pNp2) Assert.Fail("t2 != p2, wrong solution for pass2, aborted!"); else Console.WriteLine("Pass2 was ok!");

            List<string> p3 = compiler.pass3(compiler.pass2(compiler.pass1(prog)));
            int[] args = new int[3] { 4, 0, 0 };
            int res = Simulator.simulate(p3, args);
            if (res != 3) Assert.Fail("prog(4,0,0) == 3 and not " + res + " => wrong solution, aborted!"); else Console.WriteLine("prog(4,0,0) == 3 was ok");

            args = new int[3] { 4, 8, 0 };
            res = Simulator.simulate(p3, args);
            if (res != 8) Assert.Fail("prog(4,8,0) == 8 and not " + res + " => wrong solution, aborted!"); else Console.WriteLine("prog(4,8,0) == 8 was ok");

            args = new int[3] { 4, 8, 16 };
            res = Simulator.simulate(p3, args);
            if (res != 2) Assert.Fail("prog(4,8,16) == 2 and not " + res + " => wrong solution, aborted!"); else Console.WriteLine("prog(4,8,16) == 2 was ok");
        }

        [Test]
        public void testOrderOfOps()
        {
            string prog = "[ x y z ] x - y - z + 10 / 5 / 2 - 7 / 1 / 7";
            Console.WriteLine("Testing: " + prog);
            List<string> asm = compiler.pass3(compiler.pass2(compiler.pass1(prog)));
            int[] args = new int[3] { 5, 4, 1 };
            int res = Simulator.simulate(asm, args);
            if (res != 0) Assert.Fail("prog(5,4,1) == 0 and not " + res + " => wrong solution, aborted!"); else Console.WriteLine("prog(5,4,1) == 0 was ok");
        }
    }
}