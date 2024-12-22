// --- Day 21: Keypad Conundrum ---
//  https://adventofcode.com/2024/day/21

using System.Collections.ObjectModel;

using Point = (int X, int Y);

Console.WriteLine(126384);
ProcessFile("input1.txt", 1 + 1);
Console.WriteLine();

Console.WriteLine(163920);
ProcessFile("input2.txt", 1 + 1);

static void ProcessFile(string filePath, int robots)
{
    using var reader = File.OpenText(filePath);
    var codes = ReadCodes(reader).ToArray();

    var result = Solve(codes, robots);
    Console.WriteLine(result);
}

static IEnumerable<string> ReadCodes(TextReader reader) =>
    reader.ReadLines();

static long Solve<TCodes>(TCodes codes, int robots) where TCodes : IEnumerable<string>
{
    var memos = new Dictionary<string, long>[robots];
    for (var i = 0; i < robots; i++)
        memos[i] = [];

    var solves = codes.Select(code => (Code: code, Length: SolveCode(code, robots, memos)));
    var result = solves.Select(solve => int.Parse(solve.Code[..^1]) * solve.Length).Sum();
    return result;
}

static long SolveCode<TMemos>(string code, int robots, TMemos memos)
    where TMemos : IReadOnlyList<IDictionary<string, long>>
{
    var inputs = DirectionalInputs(NumericKeypad, code).ToArray();

    var costs = inputs.Select(input => Cost(input, robots, memos)).ToArray();
    var min = costs.Min();

    return min;
}

static long Cost<TMemos>(string code, int robot, TMemos memos)
    where TMemos : IReadOnlyList<IDictionary<string, long>>
{
    return 0;
}

static IEnumerable<string> DirectionalInputs<TKeypad>(TKeypad keypad, string code) where TKeypad : IReadOnlyList<string>
{
    var codeToPoint = keypad.FlatMap().ToDictionary(mapPoint => mapPoint.Value, mapPoint => mapPoint.Point);

    var start = codeToPoint['A'];
    var disabled = codeToPoint[' '];

    IEnumerable<string>? result = default;
    foreach (var next in code.Select(chr => codeToPoint[chr]))
    {
        var currentStart = start; // For correct closure
        if (result is null)
            result = InputsToNext(currentStart, next, disabled);
        else
            result = result.SelectMany(input =>
                    InputsToNext(currentStart, next, disabled).Select(nextInput => input + nextInput)
                );
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

    static readonly ReadOnlyCollection<string> DirectionalKeypad = new string[] {
        " ^A",
        "<v>",
        }.AsReadOnly();
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

    public static IEnumerable<int[]> PermutationsOfMultiset<TCounts>(this TCounts counts) where TCounts : IEnumerable<int>
    {
        var countList = counts.ToArray();
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

            for (int i = 0; i < countList.Length; i++)
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
