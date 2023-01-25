using System.Numerics;
using System.Text;

// Thue-Morse Sequence
// https://www.codewars.com/kata/591aa1752afcb02fa300002a
public class Kata
{
    // https://en.wikipedia.org/wiki/Thue%E2%80%93Morse_sequence
    public static string ThueMorse(int n)
    {
        var result = new StringBuilder(n);

        var bit = 0;
        for (uint i = 0; i < n; i++)
        {
            var x = BitOperations.Log2(i ^ (i - 1));
            if ((x & 1) == 0)
                bit = 1 - bit;
            result.Append(bit);
        }

        return result.ToString();
    }
}