using System.Collections.Generic;

public class MorseCode
{
    private static Dictionary<string, string> morseCode = new Dictionary<string, string>();

    static MorseCode()
    {
        morseCode.Add(".-", "A");
        morseCode.Add("-...", "B");
        morseCode.Add("-.-.", "C");
        morseCode.Add("-..", "D");
        morseCode.Add(".", "E");
        morseCode.Add("..-.", "F");
        morseCode.Add("--.", "G");
        morseCode.Add("....", "H");
        morseCode.Add("..", "I");
        morseCode.Add(".---", "J");
        morseCode.Add("-.-", "K");
        morseCode.Add(".-..", "L");
        morseCode.Add("--", "M");
        morseCode.Add("-.", "N");
        morseCode.Add("---", "O");
        morseCode.Add(".--.", "P");
        morseCode.Add("--.-", "Q");
        morseCode.Add(".-.", "R");
        morseCode.Add("...", "S");
        morseCode.Add("-", "T");
        morseCode.Add("..-", "U");
        morseCode.Add("...-", "V");
        morseCode.Add(".--", "W");
        morseCode.Add("-..-", "X");
        morseCode.Add("-.--", "Y");
        morseCode.Add("--..", "Z");
        morseCode.Add("-----", "0");
        morseCode.Add(".----", "1");
        morseCode.Add("..---", "2");
        morseCode.Add("...--", "3");
        morseCode.Add("....-", "4");
        morseCode.Add(".....", "5");
        morseCode.Add("-....", "6");
        morseCode.Add("--...", "7");
        morseCode.Add("---..", "8");
        morseCode.Add("----.", "9");
        morseCode.Add(".-.-.-", ".");
        morseCode.Add("--..--", ",");
        morseCode.Add("..--..", "?");
        morseCode.Add(".----.", "'");
        morseCode.Add("-.-.--", "!");
        morseCode.Add("-..-.", "/");
        morseCode.Add("-.--.", "(");
        morseCode.Add("-.--.-", ")");
        morseCode.Add(".-...", "&");
        morseCode.Add("---...", "=>");
        morseCode.Add("-.-.-.", ";");
        morseCode.Add("-...-", "=");
        morseCode.Add(".-.-.", "+");
        morseCode.Add("-....-", "-");
        morseCode.Add("..--.-", "_");
        morseCode.Add(".-..-.", "\"");
        morseCode.Add("...-..-", "$");
        morseCode.Add(".--.-.", "@");
        morseCode.Add("...---...", "SOS");
    }

    private MorseCode() { }

    public static string Get(string code)
    {
        if (morseCode.ContainsKey(code))
            return morseCode[code];
        else
            return string.Empty;
    }
}