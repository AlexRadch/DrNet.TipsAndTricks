using System;
using NUnit.Framework;

[TestFixture]
public class MorseCodeDecoderTests
{
    [Test]
    public void MorseCodeDecoderBasicTest_1()
    {
        try
        {
            string input = ".-";
            string expected = "A";

            string actual = MorseCodeDecoder.Decode(input);

            Assert.AreEqual(expected, actual);
        }
        catch (Exception ex)
        {
            Assert.Fail("There seems to be an error somewhere in your code. Exception message reads as follows: " + ex.Message);
        }
    }

    [Test]
    public void MorseCodeDecoderBasicTest_2()
    {
        try
        {
            string input = ".";
            string expected = "E";

            string actual = MorseCodeDecoder.Decode(input);

            Assert.AreEqual(expected, actual);
        }
        catch (Exception ex)
        {
            Assert.Fail("There seems to be an error somewhere in your code. Exception message reads as follows: " + ex.Message);
        }
    }

    [Test]
    public void MorseCodeDecoderBasicTest_3()
    {
        try
        {
            string input = "..";
            string expected = "I";

            string actual = MorseCodeDecoder.Decode(input);

            Assert.AreEqual(expected, actual);
        }
        catch (Exception ex)
        {
            Assert.Fail("There seems to be an error somewhere in your code. Exception message reads as follows: " + ex.Message);
        }
    }

    [Test]
    public void MorseCodeDecoderBasicTest_4()
    {
        try
        {
            string input = ". .";
            string expected = "EE";

            string actual = MorseCodeDecoder.Decode(input);

            Assert.AreEqual(expected, actual);
        }
        catch (Exception ex)
        {
            Assert.Fail("There seems to be an error somewhere in your code. Exception message reads as follows: " + ex.Message);
        }
    }

    [Test]
    public void MorseCodeDecoderBasicTest_5()
    {
        try
        {
            string input = ".   .";
            string expected = "E E";

            string actual = MorseCodeDecoder.Decode(input);

            Assert.AreEqual(expected, actual);
        }
        catch (Exception ex)
        {
            Assert.Fail("There seems to be an error somewhere in your code. Exception message reads as follows: " + ex.Message);
        }
    }

    [Test]
    public void MorseCodeDecoderBasicTest_6()
    {
        try
        {
            string input = "...---...";
            string expected = "SOS";

            string actual = MorseCodeDecoder.Decode(input);

            Assert.AreEqual(expected, actual);
        }
        catch (Exception ex)
        {
            Assert.Fail("There seems to be an error somewhere in your code. Exception message reads as follows: " + ex.Message);
        }
    }

    [Test]
    public void MorseCodeDecoderBasicTest_7()
    {
        try
        {
            string input = "... --- ...";
            string expected = "SOS";

            string actual = MorseCodeDecoder.Decode(input);

            Assert.AreEqual(expected, actual);
        }
        catch (Exception ex)
        {
            Assert.Fail("There seems to be an error somewhere in your code. Exception message reads as follows: " + ex.Message);
        }
    }

    [Test]
    public void MorseCodeDecoderBasicTest_8()
    {
        try
        {
            string input = "...   ---   ...";
            string expected = "S O S";

            string actual = MorseCodeDecoder.Decode(input);

            Assert.AreEqual(expected, actual);
        }
        catch (Exception ex)
        {
            Assert.Fail("There seems to be an error somewhere in your code. Exception message reads as follows: " + ex.Message);
        }
    }

    [Test]
    public void MorseCodeDecoderComplexTest_1()
    {
        try
        {
            string input = " . ";
            string expected = "E";

            string actual = MorseCodeDecoder.Decode(input);

            Assert.AreEqual(expected, actual);
        }
        catch (Exception ex)
        {
            Assert.Fail("There seems to be an error somewhere in your code. Exception message reads as follows: " + ex.Message);
        }
    }

    [Test]
    public void MorseCodeDecoderComplexTest_2()
    {
        try
        {
            string input = "   .   . ";
            string expected = "E E";

            string actual = MorseCodeDecoder.Decode(input);

            Assert.AreEqual(expected, actual);
        }
        catch (Exception ex)
        {
            Assert.Fail("There seems to be an error somewhere in your code. Exception message reads as follows: " + ex.Message);
        }
    }

    [Test]
    public void MorseCodeDecoderComplexTest_3()
    {
        try
        {
            string input = "      ...---... -.-.--   - .... .   --.- ..- .. -.-. -.-   -... .-. --- .-- -.   ..-. --- -..-   .--- ..- -- .--. ...   --- ...- . .-.   - .... .   .-.. .- --.. -.--   -.. --- --. .-.-.-  ";
            string expected = "SOS! THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG.";

            string actual = MorseCodeDecoder.Decode(input);

            Assert.AreEqual(expected, actual);
        }
        catch (Exception ex)
        {
            Assert.Fail("There seems to be an error somewhere in your code. Exception message reads as follows: " + ex.Message);
        }
    }
}