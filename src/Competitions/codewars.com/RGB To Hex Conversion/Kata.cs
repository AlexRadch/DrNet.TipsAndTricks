using System;

// RGB To Hex Conversion
// https://www.codewars.com/kata/513e08acc600c94f01000001
public class Kata
{
    public static string Rgb(int r, int g, int b) =>
      (((Clamp(r) << 8) + Clamp(g) << 8) + Clamp(b)).ToString("X6");

    static int Clamp(int @byte) => Math.Clamp(@byte, byte.MinValue, byte.MaxValue);
}