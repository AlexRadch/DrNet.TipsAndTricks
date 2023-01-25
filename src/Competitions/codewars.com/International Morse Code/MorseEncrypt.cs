using System.Linq;

// International Morse Code Encryption
// https://www.codewars.com/kata/55b8c0276a7930249e00003c
//
// Dictionary CHAR_TO_MORSE in the class Preloaded is already defined to convert characters into their Morse Code equivilant.
public class MorseEncrypt
{
    public static string ToMorse(string englishStr)
    {
        if (!Preloaded.CHAR_TO_MORSE.ContainsKey(' '))
            Preloaded.CHAR_TO_MORSE.Add(' ', " ");
        return string.Join(" ", englishStr.Select(chr => Preloaded.CHAR_TO_MORSE[chr]));
    }
}