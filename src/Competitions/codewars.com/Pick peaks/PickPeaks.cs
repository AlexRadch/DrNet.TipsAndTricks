using System.Collections.Generic;

// Pick peaks
// https://www.codewars.com/kata/5279f6fe5ab7f447890006a7
public class PickPeaks
{
    public static Dictionary<string, List<int>> GetPeaks(int[] arr)
    {
        var pos = new List<int>();
        var peaks = new List<int>();

        var idx = 0;
        for (var i = 1; i < arr.Length - 1; i++)
        {
            if (arr[i - 1] < arr[i])
                idx = i;
            if (idx > 0 && arr[i] > arr[i + 1])
            {
                pos.Add(idx);
                peaks.Add(arr[idx]);
                idx = 0;
            }
        }

        return new Dictionary<string, List<int>>() {
            ["pos"] = pos,
            ["peaks"] = peaks};
    }
}