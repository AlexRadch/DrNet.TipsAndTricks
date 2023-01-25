using System.Linq;

// Decode the Morse code
// https://www.codewars.com/kata/54b724efac3d5402db00065e
class MorseCodeDecoder
{
    public static string Decode(string morseCode)
    {
        var chars = morseCode.Trim().Replace("   ", " _ ").Split(' ')
            .Select(code => code == "_" ? " " : MorseCode.Get(code));
        return string.Join("", chars);
        
        //var result = new StringBuilder();

        //var words = morseCode.Trim().Split("  ");
        //foreach (var word in words)
        //{
        //    var codes = word.Split();
        //    foreach (var code in codes)
        //        result.Append(MorseCode.Get(code));
        //    result.Append(' ');
        //}

        //if (result.Length > 0)
        //    result.Length--;
        //return result.ToString();
    }
}