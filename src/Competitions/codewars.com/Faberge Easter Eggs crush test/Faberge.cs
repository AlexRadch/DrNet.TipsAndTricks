using System.Numerics;
public class Faberge
{
    public static BigInteger Height(int n, int m)
    {
        return Faberge_CnkSum.Height(n, m);
        //return Faberge_SlowRecursion.Height(n, m);
    }
}