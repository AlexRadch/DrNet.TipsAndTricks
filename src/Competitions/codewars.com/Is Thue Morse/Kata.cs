using System.Numerics;

namespace myjinxin
{
    // Simple Fun #106: Is Thue Morse?
    // https://www.codewars.com/kata/589a9792ea93aae1bf00001c
    public class Kata
    {
        // https://en.wikipedia.org/wiki/Thue%E2%80%93Morse_sequence
        public bool IsThueMorse(int[] seq)
        {
            var bit = 0;
            for (uint i = 0; i < seq.Length; i++)
            {
                var x = BitOperations.Log2(i ^ (i - 1));
                if ((x & 1) == 0)
                    bit = 1 - bit;
                if (bit != seq[i])
                    return false;
            }
            return true;
        }
    }
}