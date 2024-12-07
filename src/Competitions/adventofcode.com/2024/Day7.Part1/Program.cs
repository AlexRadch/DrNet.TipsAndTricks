//  --- Day 7: Bridge Repair ---
//  https://adventofcode.com/2024/day/7

using Equation = (long Value, long[] Numbers);

{
    var reader = File.OpenText("input1.txt");
    var equations = ReadEquations(reader).ToArray();

    var result = Solve(equations);
    Console.WriteLine($"{result}");
}

{
    var reader = File.OpenText("input2.txt");
    var equations = ReadEquations(reader).ToArray();

    var result = Solve(equations);
    Console.WriteLine($"{result}");
}

static IEnumerable<Equation> ReadEquations(TextReader input)
{
    while (input.ReadLine() is string line && line.Split(':') is { } equParts && equParts.Length >= 2)
        yield return new Equation(long.Parse(equParts[0]), 
            equParts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray());
}

static long Solve<TEquations>(TEquations equations) where TEquations : IEnumerable<Equation>
    => equations.Select(SolveEquation).Sum();

static long SolveEquation(Equation equation)
{
    foreach (var operations in MultiCombinations(Operations, equation.Numbers.Length - 1))
    {
        long result = equation.Numbers[0];
        foreach ((var operation, var value) in operations.Zip(equation.Numbers.Skip(1)))
        {
            if (operation == '+')
                result += value;
            else
                result *= value;
        }
        if (result == equation.Value)
            return equation.Value;
    }
    return 0;
}

static IEnumerable<IEnumerable<T>> MultiCombinations<T>(IEnumerable<T> source, int length)
{
    if (length == 0)
    {
        yield return [];
        yield break;
    }

    foreach (var item in source)
        foreach (var combination in MultiCombinations(source, length - 1))
            yield return combination.Prepend(item) ;
}

partial class Program
{
    const string Operations = "+*";
}