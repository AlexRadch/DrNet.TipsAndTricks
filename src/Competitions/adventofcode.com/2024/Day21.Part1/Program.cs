// --- Day 21: Keypad Conundrum ---
//  https://adventofcode.com/2024/day/21

using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Point = (int X, int Y);

Console.WriteLine(126384);
ProcessFile("input1.txt");
Console.WriteLine();

Console.WriteLine(163920);
ProcessFile("input2.txt");

static void ProcessFile(string filePath)
{
    using var reader = File.OpenText(filePath);
    var codes = ReadCodes(reader).ToArray();

    var solves = codes.Select(Solve).ToArray();

    var result = codes.Zip(solves, (code, solve) => int.Parse(code[..^1]) * solve.Length).Sum();
    Console.WriteLine(result);
}

static IEnumerable<string> ReadCodes(TextReader reader) =>
    reader.ReadLines();

static string Solve(string code)
{
    var inputs1 = DirectionalInputs(NumericKeypad, code)
        .Select(input => (Complexity: Complexity(DirectionalKeypad, input), Value: input))
        .ToArray();
    var min1 = inputs1.Min(input => input.Complexity);
    inputs1 = [.. inputs1.Where(input => input.Complexity == min1)];

    var inputs2 = inputs1
        .SelectMany(input1 => DirectionalInputs(DirectionalKeypad, input1.Value)
            .Select(input2 => (Complexity: Complexity(DirectionalKeypad, input2), Value: input2)))
        .ToArray();
    var min2 = inputs2.Min(input => input.Complexity);
    inputs2 = [.. inputs2.Where(input => input.Complexity == min2)];


    var inputs3 = inputs2
        .SelectMany(input2 => DirectionalInputs(DirectionalKeypad, input2.Value)
            .Select(input3 => (Complexity: Complexity(DirectionalKeypad, input3), Value: input3)))
        .ToArray();
    var min3 = inputs3.Min(input => input.Value.Length);
    var input3 = inputs3.First(input => input.Value.Length == min3);

    return input3.Value;
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

static int Complexity<TKeypad>(TKeypad keypad, string input) where TKeypad : IReadOnlyList<string>
{
    var codeToPoint = keypad.FlatMap().ToDictionary(mapPoint => mapPoint.Value, mapPoint => mapPoint.Point);

    var result = 0;
    var start = codeToPoint['A'];
    foreach (var next in input.Select(c => codeToPoint[c]))
    {
        result += Math.Abs(next.X - start.X) + Math.Abs(next.Y - start.Y) + 1;
        start = next;
    }
    return result;
}


partial class Program
{
    static readonly ReadOnlyCollection<string> NumericKeypad  = new string[] {
        "789",
        "456",
        "123",
        " 0A"
        }.AsReadOnly();

    static readonly ReadOnlyCollection<string> DirectionalKeypad = new string[] {
        " ^A",
        "<v>",
        }.AsReadOnly();

    // ">>vA" 2 1 2 3
    // ">v>A" 2 2 2 2

    // <<^A 4 1
    // ^<<A

    // "<<^AvvA>>vA"
    // "<<^AvvA>v>A"
    // "^<<AvvA>>vA"
    // "^<<AvvA>v>A"
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
