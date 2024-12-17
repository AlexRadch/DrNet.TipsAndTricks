//  --- Day 2: Red-Nosed Reports ---
//  https://adventofcode.com/2024/day/2

{
    TextReader input = File.OpenText("input1.txt");

    var result = Solve(ReadReports(input));

    Console.WriteLine(result);
}

{
    TextReader input = File.OpenText("input2.txt");

    var result = Solve(ReadReports(input));

    Console.WriteLine(result);
}

static IEnumerable<IEnumerable<int>> ReadReports(TextReader input)
{
    while (input.ReadLine() is string line)
    {
        var items = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (items.Length >= 2)
            yield return items.Select(int.Parse);
    }
}

static int Solve(IEnumerable<IEnumerable<int>> reports)
    => reports
        .Select(report => IsSafe(report))
        .Where(safe => safe)
        .Count();

static bool IsSafe<TReport>(TReport report)
    where TReport : IEnumerable<int>
    => report.Zip(report.Skip(1))
        .Select(pair => pair.Second - pair.First) is { } distances
    && Math.Sign(distances.First()) is { } sign
    && distances
        .Select(distance => Math.Sign(distance) == sign && Math.Abs(distance) <= 3)
        .All(safe => safe);
//{
//    var distances = report.Zip(report.Skip(1))
//        .Select(pair => pair.Second - pair.First);

//    var sign = Math.Sign(distances.First());

//    return distances
//        .Select(distance => Math.Sign(distance) == sign && Math.Abs(distance) <= 3)
//        .All(safe => safe);
//}
