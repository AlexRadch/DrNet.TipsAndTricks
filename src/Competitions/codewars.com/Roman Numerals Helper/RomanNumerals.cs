using System.Collections.Generic;
using System.Text;

// Roman Numerals Helper
// https://www.codewars.com/kata/51b66044bce5799a7f000003
public class RomanNumerals
{
    public static string ToRoman(int n)
    {
        var result = new StringBuilder();

        for (var i = RomanValues.Count - 1; i >= 0; i -= 2)
        {
            var rValue = RomanValues[i];
            var times = n / rValue.Value;
            if (times == 0)
                continue;
            n %= rValue.Value;

            if (times == 4)
            {
                result.Append(rValue.Char).Append(RomanValues[i + 1].Char);
                continue;
            }
            else if (times == 9)
            {
                result.Append(rValue.Char).Append(RomanValues[i + 2].Char);
                continue;
            }
            else if (times >= 5)
            {
                result.Append(RomanValues[i + 1].Char);
                times -= 5;
            }

            for (var j = 0; j < times; ++j)
                result.Append(rValue.Char);
        }

        return result.ToString();
    }

    public static int FromRoman(string romanNumeral)
    {
        var result = 0;

        var prev = 0;
        foreach (var c in romanNumeral)
        {
            var current = RomanCharValue(c);
            result += current;
            if (prev < current)
                result -= prev + prev;
            prev = current;
        }

        return result;
    }

    public static IReadOnlyList<(char Char, int Value)> RomanValues = new (char, int)[] {
        ('I', 1), ('V', 5), ('X', 10), ('L',  50), ('C', 100), ('D', 500), ('M', 1000) };

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