using System;
using System.Linq;

// Vowel Count
// https://www.codewars.com/kata/54ff3102c1bad923760001f3
public static class Kata
{
    static char[] vowels = new char[] { 'a', 'e', 'i', 'o', 'u' };

    public static int GetVowelCount(string str)
    {
        int start = 0;
        int vowelCount = 0;
        while ((start = str.IndexOfAny(vowels, start) + 1) > 0)
            ++vowelCount;

        return vowelCount;
    }
}