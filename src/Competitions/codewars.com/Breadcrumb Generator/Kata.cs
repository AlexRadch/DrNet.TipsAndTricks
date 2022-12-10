using System;
using System.Collections.Generic;
using System.Text;

// Breadcrumb Generator
// https://www.codewars.com/kata/563fbac924106b8bf7000046
public static class Kata
{

    public static string GenerateBC(string url, string separator)
    {
        ReadOnlySpan<char> urlSpan = url;
        urlSpan = urlSpan.TrimFromStartStart(@"://").TrimFromEndEnd(@"?").TrimFromEndEnd(@"#").TrimIndex();

        var result = new StringBuilder();

        urlSpan = TrimHome(urlSpan);
        result.AppendBC(@"", @"HOME", separator, urlSpan.Length <= 0);

        var indexOf = 0;
        while (indexOf >= 0 && indexOf < urlSpan.Length)
        {
            var nextIndex = urlSpan.IndexOf("/", indexOf);
            if (nextIndex < 0)
            {
                result.AppendBC(@"", urlSpan[indexOf..].FormatName(), separator, true);
                indexOf = nextIndex;
            }
            else
            {
                result.AppendBC(urlSpan[..(nextIndex + 1)], urlSpan[indexOf..nextIndex].FormatName(), separator, false);
                indexOf = nextIndex + 1;
            }
        }

        return result.ToString();
    }

    public static StringBuilder AppendBC(this StringBuilder builder, ReadOnlySpan<char> url, ReadOnlySpan<char> name, ReadOnlySpan<char> separator, bool isLast)
        => isLast ? builder.Append(@"<span class=""active"">").Append(name).Append(@"</span>") :
            builder.Append(@"<a href=""/").Append(url).Append("\">").Append(name).Append("</a>").Append(separator);

    public static string FormatName(this ReadOnlySpan<char> name)
    {
        Span<char> span = stackalloc char[name.Length];
        name.CopyTo(span);

        if (name.Length > 30)
            span = span.AcronymizeName();
        else
            span.Replace('-', ' ');

        Span<char> result = stackalloc char[span.Length];
        MemoryExtensions.ToUpperInvariant(span, result);

        return new (result);
    }

    private static Span<char> AcronymizeName(this Span<char> name)
    {
        var endIndex = name.Length;
        while (true)
        {
            var previndex = LastIndexOfSkipEnd(name, endIndex, '-');
            if (previndex < 0)
            {
                if (IsIgnoryWord(name[..endIndex]))
                    name = name[endIndex..];
                else
                {
                    var count = endIndex - 1;
                    if (count > 0)
                    {
                        name[endIndex..].CopyTo(name[1..]);
                        name = name[..^count];
                    }
                }
                break;
            }

            {
                var count = endIndex - previndex;
                if (count > 1 && !IsIgnoryWord(name[(previndex + 1)..endIndex]))
                {
                    name[previndex] = name[previndex + 1];
                    --count;
                }

                name[endIndex..].CopyTo(name[(endIndex - count)..]);
                name = name[..^count];
            }
            endIndex = previndex;
        }

        return name;
    }

    private static readonly HashSet<string> IgnoryWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {"the", "of", "in", "from", "by", "with", "and", "or", "for", "to", "at", "a"};

    public static bool IsIgnoryWord(this ReadOnlySpan<char> word)
        => IgnoryWords.Contains(new string(word));

    public static void Replace<T>(this Span<T> span, T value, T newValue) where T : IEquatable<T>
    {
        var indexOf = -1;
        while ((indexOf = IndexOf(span, value, indexOf + 1)) >= 0)
            span[indexOf] = newValue;
    }

    public static int IndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value, int from) where T : IEquatable<T>
    {
        var indexOf = span[from..].IndexOf(value);
        return indexOf < 0 ? indexOf : indexOf + from;
    }

    public static int IndexOf<T>(this ReadOnlySpan<T> span, T value, int from) where T : IEquatable<T>
    {
        var indexOf = span[from..].IndexOf(value);
        return indexOf < 0 ? indexOf : indexOf + from;
    }

    public static int LastIndexOfSkipEnd<T>(this ReadOnlySpan<T> span, int from, T value) where T : IEquatable<T>
        => span[..from].LastIndexOf(value);

    public static int LastIndexOfSkipStart<T>(this ReadOnlySpan<T> span, int to, T value) where T : IEquatable<T>
        => span[to..].LastIndexOf(value) is var indexOf && 
            indexOf < 0 ? indexOf : indexOf + to;
        


    public static ReadOnlySpan<char> TrimFromStartStart(this ReadOnlySpan<char> span, ReadOnlySpan<char> value)
    {
        var indexOf = span.IndexOf(value);
        return indexOf >= 0 ? span[(indexOf + value.Length)..] : span;
    }

    public static ReadOnlySpan<char> TrimFromStartEnd(this ReadOnlySpan<char> span, ReadOnlySpan<char> value)
    {
        var indexOf = span.IndexOf(value);
        return indexOf >= 0 ? span[..indexOf] : span;
    }

    public static ReadOnlySpan<char> TrimFromEndStart(this ReadOnlySpan<char> span, ReadOnlySpan<char> value)
    {
        var indexOf = span.LastIndexOf(value);
        return indexOf >= 0 ? span[(indexOf + value.Length)..] : span;
    }

    public static ReadOnlySpan<char> TrimFromEndEnd(this ReadOnlySpan<char> span, ReadOnlySpan<char> value)
    {
        var indexOf = span.LastIndexOf(value);
        return indexOf >= 0 ? span[..indexOf] : span;
    }

    public static ReadOnlySpan<char> TrimIndex(this ReadOnlySpan<char> span)
    {
        var indexFrom = span.LastIndexOf('/');
        if (indexFrom < 0)
            return span;
        {
            var indexTo = span.LastIndexOfSkipStart(indexFrom + 1, '.');
            if (indexTo >= 0)
                span = span[..indexTo];
        }

        var indexSpan = span[(indexFrom + 1)..];
        if (indexSpan.Length == 0)
            return span[..indexFrom];

        if (indexSpan.CompareTo("index", StringComparison.OrdinalIgnoreCase) == 0)
            return span[..indexFrom];

        return span;
    }

    public static ReadOnlySpan<char> TrimHome(this ReadOnlySpan<char> span)
    {
        var indexOf = span.IndexOf('/');
        return indexOf >= 0 ? span[(indexOf + 1)..] : span[span.Length..];
    }
}