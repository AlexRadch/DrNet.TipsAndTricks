using System;
using System.Linq;

// Tribonacci Sequence
// https://www.codewars.com/kata/556deca17c58da83c00002db
public class Xbonacci
{
    public double[] Tribonacci(double[] signature, int n)
    {
        var len = signature.Length;
        var result = new double[n];
        Array.Copy(signature, result, Math.Min(len, n));

        double sum = signature.Sum();
        for (var i = len; i < n; i++)
            (result[i], sum) = (sum, sum + sum - result[i - len]);

        return result;
    }
}