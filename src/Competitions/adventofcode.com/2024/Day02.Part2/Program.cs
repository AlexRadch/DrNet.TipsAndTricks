//  --- Day 2: Red-Nosed Reports ---
//  https://adventofcode.com/2024/day/2

{
    TextReader input = File.OpenText("input1.txt");

    var result = Solve(ReadReports(input));

    Console.WriteLine($"{result}");
}

{
    TextReader input = File.OpenText("input2.txt");

    var result = Solve(ReadReports(input));

    Console.WriteLine($"{result}");
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
{
    var prev = report.ElementAt(1);
    var sign = Math.Sign(prev - report.ElementAt(0));
    var safe = IsSafePair(report.ElementAt(0), prev, sign);
    if (safe)
    {
        foreach (var next in report.Skip(2))
        {
            if (!IsSafePair(prev, next, sign))
            {
                if (!safe)
                    goto Try2;
                safe = false;
            }
            else
                prev = next;
        }
        return true;
    }

Try2:
    prev = report.ElementAt(2);
    sign = Math.Sign(prev - report.ElementAt(0));
        safe = IsSafePair(report.ElementAt(0), prev, sign);
    if (safe)
    {
        foreach (var next in report.Skip(3))
        {
            if (!IsSafePair(prev, next, sign))
                goto Try3;
            else
                prev = next;
        }
        return true;
    }

Try3:
    prev = report.ElementAt(2);
    sign = Math.Sign(prev - report.ElementAt(1));
    safe = IsSafePair(report.ElementAt(1), prev, sign);
    if (!safe)
        return false;

    foreach (var next in report.Skip(3))
    {
        if (!IsSafePair(prev, next, sign))
            return false;
        else
            prev = next;
    }
    return true;
}

static bool IsSafePair(int first, int second, int sign)
    => second - first is { } distance && Math.Sign(distance) == sign && Math.Abs(distance) <= 3;


