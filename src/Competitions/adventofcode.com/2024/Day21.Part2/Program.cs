// --- Day 21: Keypad Conundrum ---
//  https://adventofcode.com/2024/day/21

using System.Collections.ObjectModel;

using Point = (int X, int Y);

Console.WriteLine(126384);
ProcessFile("input1.txt", 2 + 1);
Console.WriteLine();

Console.WriteLine(163920);
ProcessFile("input2.txt", 2 + 1);
Console.WriteLine();

ProcessFile("input2.txt", 25 + 1);

static void ProcessFile(string filePath, int robots)
{
    using var reader = File.OpenText(filePath);
    var codes = ReadCodes(reader).ToList();

    var result = Solve(codes, robots);
    Console.WriteLine(result);
}

static IEnumerable<string> ReadCodes(TextReader reader) =>
    reader.ReadLines();

static long Solve<TCodes>(TCodes codes, int robots) where TCodes : IEnumerable<string>
{
    var memos = new Dictionary<string, long>[robots - 1];
    for (var i = 0; i < robots - 1; i++)
        memos[i] = [];

    var solves = codes.Select(code => (Code: code, Length: SolveCode(code, robots, memos)));
    var result = solves.Select(solve => int.Parse(solve.Code[..^1]) * solve.Length).Sum();
    return result;
}

static long SolveCode<TMemos>(string code, int robots, TMemos memos)
    where TMemos : IReadOnlyList<IDictionary<string, long>>
{
    var seqInputs = DirectionalInputs(NumericKeypadMap, code);

    var costs = seqInputs.Select(inputs =>
            inputs.Select(input => Cost(input, robots - 1, memos))
                .Sum()
        )
        ;

    var min = costs.Min();
    return min;
}

static long Cost<TMemos>(string code, int keypadIndex, TMemos memos)
    where TMemos : IReadOnlyList<IDictionary<string, long>>
{
    if (keypadIndex == 0)
        return code.Length;

    if (memos[keypadIndex - 1].TryGetValue(code, out var result))
        return result;

    var minCost = long.MaxValue;

    var seqInputs = DirectionalInputs(DirectionalKeypadMap, code);
    foreach (var inputs in seqInputs)
    {
        var costs = inputs.Select(input => Cost(input, keypadIndex - 1, memos));
        var cost = costs.Sum();
        if (cost < minCost)
            minCost = cost;
    }

    memos[keypadIndex - 1][code] = minCost;
    return minCost;
}

static IEnumerable<IEnumerable<string>> DirectionalInputs<TKeypadMap>(TKeypadMap keypadMap, string code)
    where TKeypadMap : IReadOnlyDictionary<char, Point>
{
    var start = keypadMap['A'];
    var disabled = keypadMap[' '];

    IEnumerable<IEnumerable<string>> result = [[]];
    foreach (var next in code.Select(chr => keypadMap[chr]))
    {
        var currentStart = start; // For correct closure
        IEnumerable<string> nextInputs = [.. InputsToNext(currentStart, next, disabled)];

        result = [.. result.SelectMany(inputs => nextInputs.Select(nextInput => inputs.Append(nextInput)))];
        start = next;
    }

    return result ?? [];
}

static IEnumerable<string> InputsToNext(Point start, Point end, Point disabled)
{
    var dx = end.X - start.X;
    var dy = end.Y - start.Y;

    var counts = new int[] { Math.Abs(dx), Math.Abs(dy) };

    var permutations = counts.PermutationsOfMultiset();


    var items = new char[] { dx > 0 ? '>' : '<', dy > 0 ? 'v' : '^' };

    foreach (var permutation in permutations)
    {
        var next = start;
        foreach (var dir in permutation.Select(i => items[i]))
        {
            switch (dir)
            {
                case '^': next.Y--; break;
                case '>': next.X++; break;
                case '<': next.X--; break;
                case 'v': next.Y++; break;
            }
            if (next == disabled)
                goto Skip;
        }

        yield return string.Create(Math.Abs(dx) + Math.Abs(dy) + 1, permutation, (span, permutation) =>
        {
            for (var i = 0; i < permutation.Length; i++)
                span[i] = items[permutation[i]];
            span[permutation.Length] = 'A';
        });
    Skip:
        { }
    }
}

partial class Program
{
    static readonly ReadOnlyCollection<string> NumericKeypad = new string[] {
        "789",
        "456",
        "123",
        " 0A"
        }.AsReadOnly();

    static readonly ReadOnlyDictionary<char, Point> NumericKeypadMap = NumericKeypad
        .FlatMap()
        .ToDictionary(mapPoint => mapPoint.Value, mapPoint => mapPoint.Point)
        .AsReadOnly();

    static readonly ReadOnlyCollection<string> DirectionalKeypad = new string[] {
        " ^A",
        "<v>",
        }.AsReadOnly();

    static readonly ReadOnlyDictionary<char, Point> DirectionalKeypadMap = DirectionalKeypad
        .FlatMap()
        .ToDictionary(mapPoint => mapPoint.Value, mapPoint => mapPoint.Point)
        .AsReadOnly();
}

static class Extensions
{
    public static IEnumerable<(Point Point, T Value)> FlatMap<T>(this IEnumerable<IEnumerable<T>> map) =>
        map.SelectMany((row, y) => row.Select((value, x) => ((x, y), value)));

    public static IEnumerable<string> ReadLines(this TextReader reader)
    {
        while (reader.ReadLine() is string line)
            yield return line;
    }

    public static IEnumerable<int[]> PermutationsOfMultiset<TCounts>(this TCounts counts)
        where TCounts : IEnumerable<int>
    {
        var countList = counts.ToList();
        var total = countList.Sum();
        var result = new int[total];

        return GeneratePermutations(0);

        IEnumerable<int[]> GeneratePermutations(int depth)
        {
            if (depth == result.Length)
            {
                yield return (int[])result.Clone();
                yield break;
            }

            for (int i = 0; i < countList.Count; i++)
            {
                if (countList[i] > 0)
                {
                    result[depth] = i;
                    countList[i]--;
                    foreach (var permutation in GeneratePermutations(depth + 1))
                        yield return permutation;
                    countList[i]++;
                }
            }
        }
    }
}
