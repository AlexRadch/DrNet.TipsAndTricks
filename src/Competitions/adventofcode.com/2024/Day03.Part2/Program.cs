//  --- Day 3: Mull It Over ---
//  https://adventofcode.com/2024/day/3

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
    MulRegex().Matches(DontRegex().Replace(input, "_"))
        .Select(match => long.Parse(match.Groups[1].Value) * long.Parse(match.Groups[2].Value))
        .Sum();

partial class Program
{
    [GeneratedRegex(@"don't\(\).*?((do\(\))|$)", RegexOptions.Singleline)]
    private static partial Regex DontRegex();

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex MulRegex();
}