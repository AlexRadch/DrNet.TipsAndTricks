using System.Collections.Generic;
using System.Linq;

// Roman Numerals Decoder
// https://www.codewars.com/kata/51b6249c4612257ac0000005
public class RomanDecode
{
    public static int Solution(string roman)
    {
        var prev = 0;
        return roman.Select(c => {
            var v = RomanDictionary[c];
            if (prev < v)
                v -= prev + prev;
            prev = v;
            return v;
        }).Sum();
    }

    private static Dictionary<char, int> RomanDictionary = new()
  {
    { 'I', 1 },
    { 'V', 5 },
    { 'X', 10 },
    { 'L', 50 },
    { 'C', 100 },
    { 'D', 500 },
    { 'M', 1000 },
  };
}