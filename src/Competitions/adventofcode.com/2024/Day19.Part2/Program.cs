// ---Day 19: Linen Layout ---
//  https://adventofcode.com/2024/day/19

using System;

{
    using var reader = File.OpenText("input1.txt");
    var towels = reader.ReadLines().First().Split(',', StringSplitOptions.TrimEntries);
    _ = reader.ReadLines().Take(1).Count();

    var designs = ReadDesigns(reader).ToArray();

    var memo = new Dictionary<string, long>();
    var solves = designs.Select(design => Solve(towels, design, memo));

    var result = solves.Sum();
    Console.WriteLine(result);
}

{
    using var reader = File.OpenText("input2.txt");
    var towels = reader.ReadLines().First().Split(',', StringSplitOptions.TrimEntries);
    _ = reader.ReadLines().Take(1).Count();

    var designs = ReadDesigns(reader).ToArray();

    {
        var memo = new Dictionary<string, long>();
        var result = designs.Select(design => Solve(towels, design, memo)).Sum();
        Console.WriteLine(result);
    }
    {
        var memo = new Dictionary<string, long>();
        var result = designs.Select(design => Solve2(towels, design, memo)).Sum();
        Console.WriteLine(result);
    }
    {
        var memo = new Dictionary<ReadOnlyMemory<char>, long>();
        var result = designs.Select(design => Solve3(towels, design, memo)).Sum();
        Console.WriteLine(result);
    }
    {
        var memo = new Dictionary<ReadOnlyMemory<char>, long>();
        var result = designs.Select(design => Solve4(towels, design, memo)).Sum();
        Console.WriteLine(result);
    }
}

static IEnumerable<string> ReadDesigns(TextReader reader) =>
    reader.ReadLines().Select(s => s.Trim().ToLower());

static long Solve<TTowels>(TTowels towels, string design, Dictionary<string, long> memo) where TTowels : IEnumerable<string>
{
    var maxLen = towels.Select(s => s.Length).Max();
    var sets = new HashSet<string>[maxLen];
    for (var i = 0; i < sets.Length; i++)
        sets[i] = [];

    foreach (var towel in towels)
        sets[towel.Length - 1].Add(towel);

    return SolveFast(design);

    long SolveFast(string design)
    {
        if (string.IsNullOrEmpty(design))
            return 1;
        if (memo.TryGetValue(design, out long value))
            return value;

        long result = 0;
        for (var i = Math.Min(design.Length, sets.Length); i > 0; i--)
            if (sets[i - 1].Contains(design[..i]))
                result += SolveFast(design[i..]);

        return memo[design] = result;
    }
}

static long Solve2<TTowels>(TTowels towels, string design, Dictionary<string, long> memo) where TTowels : IEnumerable<string>
{
    var maxLen = towels.Select(s => s.Length).Max();
    var set = new HashSet<string>(towels);

    return SolveFast(design);

    long SolveFast(string design)
    {
        if (string.IsNullOrEmpty(design))
            return 1;
        if (memo.TryGetValue(design, out long value))
            return value;

        long result = 0;
        for (var i = Math.Min(design.Length, maxLen); i > 0; i--)
            if (set.Contains(design[..i]))
                result += SolveFast(design[i..]);

        return memo[design] = result;
    }
}

static long Solve3<TTowels>(TTowels towels, string design, Dictionary<ReadOnlyMemory<char>, long> memo) where TTowels : IEnumerable<string>
{
    var maxLen = towels.Select(s => s.Length).Max();
    var set = new HashSet<string>(towels);

    return SolveFast(design.AsMemory());

    long SolveFast(ReadOnlyMemory<char> design)
    {
        if (design.IsEmpty)
            return 1;
        if (memo.TryGetValue(design, out long value))
            return value;

        long result = 0;
        for (var i = Math.Min(design.Length, maxLen); i > 0; i--)
            if (set.GetAlternateLookup<ReadOnlySpan<char>>().Contains(design[..i].Span))
                result += SolveFast(design[i..]);

        return memo[design] = result;
    }
}

static long Solve4<TTowels>(TTowels towels, string design, Dictionary<ReadOnlyMemory<char>, long> memo) where TTowels : IEnumerable<string>
{
    var maxLen = towels.Select(s => s.Length).Max();
    var set = new HashSet<string>(towels);

    return SolveFast(design.AsMemory());

    long SolveFast(ReadOnlyMemory<char> design) =>
        design.IsEmpty ? 1
        : memo.TryGetValue(design, out long value) ? value
        : memo[design] = Enumerable.Range(1, Math.Min(design.Length, maxLen))
            .Where(i => set.GetAlternateLookup<ReadOnlySpan<char>>().Contains(design[..i].Span))
           .Select(i => SolveFast(design[i..])).Sum();
}

static class Extensions
{
    public static IEnumerable<string> ReadLines(this TextReader reader)
    {
        while (reader.ReadLine() is string line)
            yield return line;
    }
}
