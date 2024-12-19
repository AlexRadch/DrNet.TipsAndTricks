// ---Day 19: Linen Layout ---
//  https://adventofcode.com/2024/day/19

{
    using var reader = File.OpenText("input1.txt");
    var towels = reader.ReadLines().First().Split(',', StringSplitOptions.TrimEntries);
    _ = reader.ReadLines().Take(1).Count();

    var designs = ReadDesigns(reader).ToArray();

    var memo = new Dictionary<string, bool>();
    var solves = designs.Select(design => Solve(towels, design, memo));

    var result = solves.Where(solve => solve).Count();
    Console.WriteLine(result);
}

{
    using var reader = File.OpenText("input2.txt");
    var towels = reader.ReadLines().First().Split(',', StringSplitOptions.TrimEntries)
        .Select(s => s.Trim().ToLower()).ToArray();
    _ = reader.ReadLines().Take(1).Count();

    var designs = ReadDesigns(reader).ToArray();

    var memo = new Dictionary<string, bool>();
    var solves = designs.Select(design => (design, Solve(towels, design, memo))).ToArray();

    var result = solves.Where(solve => solve.Item2).Count();
    Console.WriteLine(result);
}

{
    using var reader = File.OpenText("input2.txt");
    var towels = reader.ReadLines().First().Split(',', StringSplitOptions.TrimEntries)
        .Select(s => s.Trim().ToLower()).ToArray();
    _ = reader.ReadLines().Take(1).Count();

    var designs = ReadDesigns(reader).ToArray();

    var memo = new Dictionary<string, bool>();
    var solves = designs.Select(design => (design, Solve2(towels, design, memo))).ToArray();

    var result = solves.Where(solve => solve.Item2).Count();
    Console.WriteLine(result);
}


static IEnumerable<string> ReadDesigns(TextReader reader) =>
    reader.ReadLines().Select(s => s.Trim().ToLower());

static bool Solve<TTowels>(TTowels towels, string design, Dictionary<string, bool> memo) where TTowels : IEnumerable<string>
{
    var maxLen = towels.Select(s => s.Length).Max();
    var sets = new HashSet<string>[maxLen];
    for (var i = 0; i < sets.Length; i++)
        sets[i] = [];

    foreach (var towel in towels)
        sets[towel.Length - 1].Add(towel);

    return SolveFast(design);

    bool SolveFast(string design)
    {
        if (string.IsNullOrEmpty(design))
            return true;
        if (memo.TryGetValue(design, out bool value))
            return value;

        for (var i = Math.Min(design.Length, sets.Length); i > 0; i--)
            if (sets[i - 1].Contains(design[..i]) && SolveFast(design[i..]))
                return memo[design] = true;

        return memo[design] = false;
    }
}

static bool Solve2<TTowels>(TTowels towels, string design, Dictionary<string, bool> memo) where TTowels : IEnumerable<string>
{
    var maxLen = towels.Select(s => s.Length).Max();
    var set = new HashSet<string>(towels);

    return SolveFast(design);

    bool SolveFast(string design)
    {
        if (string.IsNullOrEmpty(design))
            return true;
        if (memo.TryGetValue(design, out bool value))
            return value;

        for (var i = Math.Min(design.Length, maxLen); i > 0; i--)
            if (set.Contains(design[..i]) && SolveFast(design[i..]))
                return memo[design] = true;

        return memo[design] = false;
    }
}

static class Extensions
{
    public static IEnumerable<string> ReadLines(this TextReader reader)
    {
        while (reader.ReadLine() is string line)
            yield return line;
    }
}
