using System;
using System.Linq;

// Decode the Morse code, advanced
// https://www.codewars.com/kata/54b72c16cd7f5154e9000457
public class MorseCodeDecoder
{
    public static string DecodeBits(string bits)
    {
        bits = bits.Trim('0');
        var rate = bits.Split('1', StringSplitOptions.RemoveEmptyEntries).Concat(
            bits.Split('0', StringSplitOptions.RemoveEmptyEntries)).Min(str => str.Length);

        var result = bits.Replace(new string('1', rate * dashRates), "-");
        result = result.Replace(new string('1', rate * dotRates), ".");
        result = result.Replace(new string('0', rate * pauseBetweenWords), "   ");
        result = result.Replace(new string('0', rate * pauseBetweenChars), " ");
        result = result.Replace(new string('0', rate * pauseBetweenDotsAndDashes), "");
        return result;
    }

    public static string DecodeMorse(string morseCode)
    {
        var chars = morseCode.Trim().Replace("   ", " _ ").Split(' ')
            .Select(code => code == "_" ? " " : MorseCode.Get(code));
        return string.Join("", chars);
    }

    const int dotRates = 1;
    const int dashRates = 3;
    const int pauseBetweenDotsAndDashes = 1;
    const int pauseBetweenChars = 3;
    const int pauseBetweenWords = 7;
}