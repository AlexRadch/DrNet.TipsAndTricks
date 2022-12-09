using System;
using System.Linq;

// First non-repeating character
// https://www.codewars.com/kata/52bc74d4ac05d0945d00054e
public class Kata
{
    public static string FirstNonRepeatingLetter(string s) =>
        s.Where((c, i) =>
            s.IndexOf(c.ToString(), i + 1, StringComparison.OrdinalIgnoreCase) < 0 &&
            s.IndexOf(c.ToString(), StringComparison.OrdinalIgnoreCase) == i)
        .FirstOrDefault()
        .ToString()
        .Trim(default(char));
}