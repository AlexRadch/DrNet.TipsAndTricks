// --- Day 23: LAN Party ---
//  https://adventofcode.com/2024/day/23

ProcessFile("input1.txt");
Console.WriteLine();

ProcessFile("input2.txt");
Console.WriteLine();

static void ProcessFile(string filePath)
{
    using var reader = File.OpenText(filePath);
    var pairs = ReadPairs(reader).ToList();

    var party = Solve(pairs).Order();
    var result = string.Join(',', party);

    Console.WriteLine(result);
}

static IEnumerable<Pair> ReadPairs(TextReader reader) =>
    reader.ReadLines().Select(line => new Pair(line[..2], line[3..5]));

static IEnumerable<string> Solve<TPairs>(TPairs pairs) where TPairs : IEnumerable<Pair>
{
    var pairSet = new HashSet<Pair>(pairs);

    var itemsDict = new Dictionary<string, Dictionary<Pair, List<string>>>();
    foreach (var pair in pairSet)
    {
        if (!itemsDict.TryGetValue(pair.Item1, out var partiesDict))
            itemsDict[pair.Item1] = partiesDict = [];

        if (!partiesDict.TryGetValue(pair, out var pairParty))
        {
            pairParty = [ pair.Item1, pair.Item2 ];
            partiesDict[pair] = pairParty;
        }

        foreach (var party in partiesDict.Values)
        {
            if (party == pairParty)
                continue;

            if (party.All(item3 => pairSet.Contains(new Pair(pair.Item2, item3))))
                party.Add(pair.Item2);
        }
    }

    var allParties = itemsDict.Values.SelectMany(dict => dict.Values);
    var maxParty = allParties.MaxBy(party => party.Count) ?? [];

    return maxParty;
}

readonly record struct Pair
{
    public string Item1 { get; init; }
    public string Item2 { get; init; }

    public Pair(string item1, string item2)
    {
        if (string.Compare(item1, item2) <= 0)
        {
            Item1 = item1;
            Item2 = item2;
        }
        else
        {
            Item1 = item2;
            Item2 = item1;
        }
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
