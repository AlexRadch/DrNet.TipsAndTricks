using System.Text;

public class RangeExtraction
{
    public static string Extract(int[] args)
    {
        var builder = new StringBuilder();
        for (var i = 0; i < args.Length; i++)
        {
            var j = i;
            while (i + 1 < args.Length && args[i] + 1 == args[i + 1])
                ++i;
            j = i - j;
            builder.Append(j switch
            {
                0 => args[i].ToString(),
                1 => $"{args[i] - j},{args[i]}",
                _ => $"{args[i] - j}-{args[i]}",
            });
            builder.Append(',');
        }
        if (builder.Length > 0)
            builder.Remove(builder.Length - 1, 1);

        return builder.ToString();
    }
}