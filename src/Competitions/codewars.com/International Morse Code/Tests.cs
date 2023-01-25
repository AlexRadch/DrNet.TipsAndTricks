using System;
using System.Text;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class Tests
{
    void Test(string morse, string english)
    {
        Assert.AreEqual(morse, MorseEncrypt.ToMorse(english));
    }

    [Test]
    public void Test1()
    {
        string english = "HELLO WORLD";
        string morse = ".... . .-.. .-.. ---   .-- --- .-. .-.. -..";
        Test(morse, english);
    }

    [Test]
    public void Test2()
    {
        string english = "SOS";
        string morse = "... --- ...";
        Test(morse, english);
    }

    [Test]
    public void Test3()
    {
        string english = "1836";
        string morse = ".---- ---.. ...-- -....";
        Test(morse, english);
    }

    [Test]
    public void Test4()
    {
        string english = "THE QUICK BROWN FOX";
        string morse = "- .... .   --.- ..- .. -.-. -.-   -... .-. --- .-- -.   ..-. --- -..-";
        Test(morse, english);
    }

    [Test]
    public void Test5()
    {
        string english = "JUMPED OVER THE";
        string morse = ".--- ..- -- .--. . -..   --- ...- . .-.   - .... .";
        Test(morse, english);
    }

    [Test]
    public void Test6()
    {
        string english = "LAZY DOG";
        string morse = ".-.. .- --.. -.--   -.. --- --.";
        Test(morse, english);
    }

    [Test]
    public void Test7()
    {
        string english = "WOLFRAM ALPHA 1";
        string morse = ".-- --- .-.. ..-. .-. .- --   .- .-.. .--. .... .-   .----";
        Test(morse, english);
    }

    [Test]
    public void Test8()
    {
        string english = "CodeWars Rocks";
        string morse = "-.-. --- -.. . .-- .- .-. ...   .-. --- -.-. -.- ...";
        Test(morse, english);
    }

    [Test]
    public void Test9()
    {
        string english = "";
        string morse = "";
        Test(morse, english);
    }

    [Test]
    public void Test10()
    {
        string english = "Final basic test";
        string morse = "..-. .. -. .- .-..   -... .- ... .. -.-.   - . ... -";
        Test(morse, english);
    }

    string Solution(string englishStr)
    {
        StringBuilder strBuilder = new StringBuilder();
        for (int c = 0; c < englishStr.Length; ++c)
        {
            string morseChar;
            strBuilder.Append(Preloaded.CHAR_TO_MORSE.TryGetValue(englishStr[c], out morseChar) ? morseChar : " ");

            if (c < englishStr.Length - 1)
            {
                strBuilder.Append(' ');
            }
        }

        return strBuilder.ToString();
    }

    [Test]
    public void TestsRandom()
    {
        Random rnd = new Random();
        string characters = "abcdefghijklmnopqrstuvwxyz ABCDEFGHIJKLMNOPQRSTUVWXYZ 0123456789 ";

        for (int i = 0; i < 40; ++i)
        {
            string english = new string(Enumerable.Repeat(characters, rnd.Next(10) + 10).Select(s => s[rnd.Next(s.Length)]).ToArray());
            Console.Write("Testing: " + english + "\n");
            Test(Solution(english), english);
        }
    }
}
