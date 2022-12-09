using System.Linq;

// Replace With Alphabet Position
// https://www.codewars.com/kata/546f922b54af40e1e90001da
public static class Kata
{
    public static string AlphabetPosition(string text) =>
      string.Join(" ", text.ToLower().Where(char.IsLetter).Select(x => x - 'a' + 1));
}