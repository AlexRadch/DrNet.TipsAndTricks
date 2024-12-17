//  --- Day 5: Print Queue ---
//  https://adventofcode.com/2024/day/5

using Rule = (int PageBefore, int PageAfter);

{
    var reader = File.OpenText("input1.txt");
    var rules = ReadRules(reader).ToArray();
    var updates = ReadUpdates(reader).ToArray();

    var result = Solve(rules, updates);
    Console.WriteLine(result);
}

{
    var reader = File.OpenText("input2.txt");
    var rules = ReadRules(reader).ToArray();
    var updates = ReadUpdates(reader).ToArray();

    var result = Solve(rules, updates);
    Console.WriteLine(result);
}

static IEnumerable<Rule> ReadRules(TextReader input)
{
    while (input.ReadLine() is string line && line.Split('|') is { } rule && rule.Length >= 2)
        yield return new Rule(int.Parse(rule[0]), int.Parse(rule[1]));
}

static IEnumerable<IEnumerable<int>> ReadUpdates(TextReader input)
{
    while (input.ReadLine() is string line && line.Split(',') is { } update && update.Length > 0)
        yield return update.Select(int.Parse);
}

static int Solve<TRules, TUpdates>(TRules rules, TUpdates updates)
    where TRules : IEnumerable<Rule>
    where TUpdates : IEnumerable<IEnumerable<int>>
{
    var rulesSet = rules.ToHashSet();

    var results = updates.Select(update => SolveUpdate(rulesSet, update));
    var result = results.Sum();
    return result;
}

static int SolveUpdate<TRules, TUpdate>(TRules rules, TUpdate update)
    where TRules : ISet<Rule>
    where TUpdate : IEnumerable<int>
{
    if (update.Zip(update.Skip(1)).All(rule => !rules.Contains((rule.Second, rule.First))))
        return 0;

    var ordered = update.Order(Comparer<int>.Create(Compare));
    return ordered.ElementAt(ordered.Count() / 2);

    int Compare(int a, int b) =>
        rules.Contains((a, b)) ? -1
        : rules.Contains((b, a)) ? 1
        : 0;
}