
using System.Text.RegularExpressions;

{
    var input = File.OpenText("input1.txt").ReadToEnd();

    var result = Solve(input);

    Console.WriteLine($"{result}");
}

{
    var input = File.OpenText("input2.txt").ReadToEnd();

    var result = Solve(input);

    Console.WriteLine($"{result}");
}

static long Solve(string input) =>
    MyRegex().Matches(input)
        .Select(match => long.Parse(match.Groups[1].Value) * long.Parse(match.Groups[2].Value))
        .Sum();

partial class Program
{
    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex MyRegex();
}