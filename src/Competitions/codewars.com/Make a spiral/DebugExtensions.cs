using System;

public static class DebugExtensions
{
    public static string Visualize2d(this Array source, int pad = 5)
    {
        var result = "";
        for (int i = source.GetLowerBound(0); i <= source.GetUpperBound(0); i++)
        {
            for (int j = source.GetLowerBound(1); j <= source.GetUpperBound(1); j++)
                result += source.GetValue(i, j)!.ToString()!.PadLeft(pad);
            result += "\n";
        }
        return result;
    }
}