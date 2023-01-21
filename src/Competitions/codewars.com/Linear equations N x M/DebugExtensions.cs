using System;

public static class DebugExtensions
{
    public static string VisualizeMatrix(this Array matrix, int pad = 5)
    {
        var result = "";
        for (int i = matrix.GetLowerBound(0); i <= matrix.GetUpperBound(0); i++)
        {
            for (int j = matrix.GetLowerBound(1); j <= matrix.GetUpperBound(1); j++)
                result += matrix.GetValue(i, j)!.ToString()!.PadLeft(pad);
            result += "\n";
        }
        return result;
    }

    public static string VisualizeDynMatrix(this Array matrix, int pad = 5)
    {
        var result = "";
        for (int i = matrix.GetLowerBound(0); i <= matrix.GetUpperBound(0); i++)
        {
            Array row = (Array)matrix.GetValue(i)!;
            for (int j = row.GetLowerBound(0); j <= row.GetUpperBound(0); j++)
                result += row.GetValue(j)!.ToString()!.PadLeft(pad);
            result += "\n";
        }
        return result;
    }
}