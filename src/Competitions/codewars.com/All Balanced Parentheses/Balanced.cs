using System.Collections.Generic;
using System.Text;

// All Balanced Parentheses
// https://www.codewars.com/kata/5426d7a2c2c7784365000783
public class Balanced
{
    public static List<string> BalancedParens(int n)
    {
        var result = new List<string>();
        Balance(0, 0, n, new StringBuilder(), result);
        return result;
    }

    private static void Balance(int open, int close, int n, StringBuilder balance, List<string> balancies)
    {
        if (open < n)
        {
            balance.Append('(');
            Balance(open + 1, close, n, balance, balancies);
            balance.Remove(balance.Length - 1, 1);
        }
        if (close < open)
        {
            balance.Append(')');
            Balance(open, close + 1, n, balance, balancies);
            balance.Remove(balance.Length - 1, 1);
        }
        if (close == n)
            balancies.Add(balance.ToString());
    }

}