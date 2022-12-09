using System.Collections.Generic;
using System.Linq;

// Roman Numerals Decoder
// https://www.codewars.com/kata/51b6249c4612257ac0000005
public class RomanDecode
{
    public static int Solution(string roman)
    {
        var result = 0;

        var prev = 0;
        foreach(var c in roman)
        {
            var current = RomanCharValue(c);
            result += current;
            if (prev < current)
                result -= prev + prev;
            prev = current;
        }

        return result;
    }

    public static int RomanCharValue(char c) => c switch
    {
        'I' => 1,
        'V' => 5,
        'X' => 10,
        'L' => 50,
        'C' => 100,
        'D' => 500,
        'M' => 1000,
        _ => throw new System.NotImplementedException()
    };
}