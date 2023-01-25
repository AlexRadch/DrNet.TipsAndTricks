namespace myjinxin
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    //using System.Text;
    using System.Linq;
    using System.Text.RegularExpressions;
    [TestFixture]
    public class CsharpTest
    {
        public bool An(int[] seq)
        {
            var s = string.Join("", seq);
            var n = "0";
            while (n.Length < s.Length) n += string.Join("", n.ToCharArray().Select(x => 49 - x));
            return n.IndexOf(s) == 0;
        }
        public int[] rndtest()
        {
            return rand(0, 1) > 0 ? good() : bad();

        }
        public int[] make100()
        {
            var n = "0";
            while (n.Length < 100) n += string.Join("", n.ToCharArray().Select(x => 49 - x));
            return n.ToCharArray().Select(x => x - 48).ToArray();
        }

        public int[] good()
        {
            var allseq = make100();
            return allseq.Take(rand(1, 100)).ToArray();
        }
        public int[] bad()
        {
            return rand(0, 1) > 0 ? bad1() : bad2();
        }
        public int[] bad1()
        {
            var r = good();
            var idx = rand(0, r.Length - 1);
            r[idx] = 1 - r[idx];
            return r;
        }
        public int[] bad2()
        {
            var allseq = make100();
            int idx1 = rand(0, 20), idx2 = rand(21, 79);
            return allseq.Skip(idx1).Take(idx2).ToArray();
        }
        public void shuff(List<int> r)
        {
            for (int i = 0; i < 50; i++)
            {
                int idx1 = rand(0, r.Count - 1), idx2 = rand(0, r.Count - 1);
                var tt = r[idx1];
                r[idx1] = r[idx2];
                r[idx2] = tt;
            }
        }

        public int[] rndtest1()
        {
            var r = new List<int>();
            int len = rand(500, 3000);
            for (var i = 0; i < len; i++) r.Add(rand(1, 1000));
            return r.ToArray();
        }
        Random rndnum = new Random(unchecked((int)DateTime.Now.Ticks));
        public int rand(int a, int b)
        {
            return a > b ? rand(b, a) : rndnum.Next(a, b + 1);
        }
        public string rnds()
        {
            var len = rand(1, 100);
            var rs = new List<char>();
            for (int i = 0; i < len; i++) rs.Add(rndcl());
            return string.Join("", rs);
        }
        public string rndss()
        {
            var len = rand(3, 7);
            var rs = new List<string>();
            for (int i = 0; i < len; i++) rs.Add(rnds());
            return string.Join(" ", rs);
        }
        public string rnds2(int n)
        {
            var len = n;
            var rs = new List<char>();
            for (int i = 0; i < len; i++) rs.Add(rndch());
            return string.Join("", rs);
        }
        public char rndcl()
        {
            var allc = "abcdefghijklmnopqrstuvwxyz";
            return allc[rand(0, allc.Length - 1)];
        }
        public char rndchl()
        {
            var allc = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return allc[rand(0, allc.Length - 1)];
        }
        public char rndch()
        {
            var allc = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return allc[rand(0, allc.Length - 1)];
        }
        [Test]
        public void Test1__Basic_Tests()
        {
            var kata = new Kata();
            var passed = "<font size=2 color='#8FBC8F'>Test Passed!</font>\n";

            Assert.AreEqual(true, kata.IsThueMorse(new int[] { 0, 1, 1, 0, 1 }));
            Console.WriteLine(passed);
            Assert.AreEqual(true, kata.IsThueMorse(new int[] { 0 }));
            Console.WriteLine(passed);
            Assert.AreEqual(false, kata.IsThueMorse(new int[] { 1 }));
            Console.WriteLine(passed);
            Assert.AreEqual(false, kata.IsThueMorse(new int[] { 0, 1, 0, 0 }));
            Console.WriteLine(passed);
            Console.WriteLine(" ");

        }

        [Test]
        public void Test2__100_Random_Tests()
        {
            var kata = new Kata();
            var passed = "<font size=2 color='#8FBC8F'><b>Test Passed!</b></font>";

            for (int i = 0; i < 100; i++)
            {
                var ab = rndtest();
                Console.WriteLine("<font size=2 color='#CFB53B'>Testing for: " +
                "\nseq = new int[]{" + string.Join(", ", ab) + "}"
                //+"\npawn = \""+ab[1]+"\""
                //+", numberOfDigits = "+ab[1]
                //+", loved = "+ab[2]
                //+", s = "+ab[3]
                + "</font>");
                var answer = An(ab);
                Assert.AreEqual(answer, kata.IsThueMorse(ab));
                Console.WriteLine("<font size=2 color='#8FBC8F'>" +
                "Pass Value = " + answer + "</font>\n");
                Console.WriteLine(" ");
            }

            Console.WriteLine("<div style='width:360px;background-color:gray'><br><font size=2 color='#3300dd'><b>Happy Coding ^_^</b></font>");
            Console.WriteLine("<br><font size=2 color='#5500ee'><b>Thanks for solve this kata,\nI'm waiting for your:<font color='993300'>\nfeedback, voting and ranking ;-)</b></font></div>");

        }
    }
}
