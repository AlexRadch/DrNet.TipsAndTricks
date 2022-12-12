using System;
using System.Collections.Generic;
using System.Text;

// Human readable duration format
// https://www.codewars.com/kata/52742f58faf5485cae000b9a
public class HumanTimeFormat
{
    public static string formatDuration(int seconds)
    {
        var result = new StringBuilder();

        var ticks = TimeSpan.FromSeconds(seconds).Ticks;
        for (int i = 0; i < Durations.Count; ++i)
        {
            var duration = Durations[i];
            (var quotient, var remainder) = Math.DivRem(ticks, duration.TimeSpan.Ticks);
            if (quotient <= 0)
                continue;

            if (result.Length > 0)
                result.Append(remainder > 0 ? ", " : " and ");
            result.Append(quotient).Append(' ').Append(quotient > 1 ? duration.PluralName : duration.SingularName);

            ticks = remainder;
        }

        return result.Length > 0 ? result.ToString() : "now";
    }

    public static readonly IReadOnlyList<(TimeSpan TimeSpan, string SingularName, string PluralName)> Durations = new (TimeSpan, string, string)[] {
        (TimeSpan.FromDays(365), "year", "years"),
        (TimeSpan.FromDays(1), "day", "days"),
        (TimeSpan.FromHours(1), "hour", "hours"),
        (TimeSpan.FromMinutes(1), "minute", "minutes"),
        (TimeSpan.FromSeconds(1), "second", "seconds"), };
}