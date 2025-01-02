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

    var party = Solve1(pairs).Order();
    var result = string.Join(',', party);
    Console.WriteLine(result);

    party = Solve2(pairs).Order();
    result = string.Join(',', party);
    Console.WriteLine(result);

    party = Solve3(pairs).Order();
    result = string.Join(',', party);
    Console.WriteLine(result);
}

static IEnumerable<Pair> ReadPairs(TextReader reader) =>
    reader.ReadLines().Select(line => new Pair(line[..2], line[3..5]));

static IEnumerable<string> Solve1<TPairs>(TPairs pairs) where TPairs : IEnumerable<Pair>
{
    var pairsSet = new HashSet<Pair>(pairs);

    var itemsDict = new Dictionary<string, Dictionary<Pair, HashSet<string>>>();
    foreach (var pair in pairsSet)
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

            if (party.All(item3 => pairsSet.Contains(new Pair(pair.Item2, item3))))
                party.Add(pair.Item2);
        }
    }

    var allParties = itemsDict.Values.SelectMany(dict => dict.Values);
    var maxParty = allParties.MaxBy(party => party.Count) ?? [];

    return maxParty;
}

static IEnumerable<string> Solve2<TPairs>(TPairs pairs) where TPairs : IEnumerable<Pair>
{
    var parties = new Dictionary<SortedSet<string>, HashSet<string>>(SortedSet<string>.CreateSetComparer());
    foreach (var pair in pairs)
    {
        var key = new SortedSet<string> { pair.Item1 };
        if (!parties.TryGetValue(key, out var party))
            parties[key] = party = [];
        party.Add(pair.Item2);
    }

    var added = parties.Count > 0;
    while (added)
    {
        var nextParties = new Dictionary<SortedSet<string>, HashSet<string>>(SortedSet<string>.CreateSetComparer());
        foreach ((var key, var party) in parties)
        {
            foreach (var item in party)
            {
                var itemKey = new SortedSet<string>(key.SkipLast(1)) { item };
                if (parties.TryGetValue(itemKey, out var itemParty))
                {
                    var nextParty = new HashSet<string>(party.Intersect(itemParty));
                    if (nextParty.Count > 0)
                        nextParties[[.. key, item]] = nextParty;
                }
            }
        }

        added = nextParties.Count > 0;
        if (added)
            parties = nextParties;
    }

    var result = parties.First();
    return result.Key.Concat(result.Value);
}

static IEnumerable<string> Solve3<TPairs>(TPairs pairs) where TPairs : IEnumerable<Pair>
{
    var graph = new Dictionary<string, HashSet<string>>();
    foreach (var pair in pairs)
    {
        if (!graph.TryGetValue(pair.Item1, out var list))
            graph[pair.Item1] = list = [];
        list.Add(pair.Item2);

        if (!graph.TryGetValue(pair.Item2, out list))
            graph[pair.Item2] = list = [];
        list.Add(pair.Item1);
    }

    var result = SolveBronKerbosch(graph.AsReadOnly());
    return result;
}


static List<string> SolveBronKerbosch<TGraphItems>(IReadOnlyDictionary<string, TGraphItems> graph)
    where TGraphItems : IEnumerable<string>
{
    var result = new List<string>();
    BronKerbosch([], new HashSet<string>(graph.Keys), new HashSet<string>(), graph, result);
    return result;
}


static void BronKerbosch<TGraphItems>(IEnumerable<string> R, ISet<string> P, ISet<string> X,
    IReadOnlyDictionary<string, TGraphItems> graph, ICollection<string> largestClique)
    where TGraphItems: IEnumerable<string>
{
    string pivot;
    if (P.Count > 0)
        pivot = P.First();
    else if (X.Count > 0)
        pivot = X.First();
    else
    {
        // Found a clique
        if (R.Count() > largestClique.Count)
        {
            largestClique.Clear();
            foreach (var item in R)
                largestClique.Add(item);
        }
        return;
    }

    var notNeighbors = new HashSet<string>(P);
    notNeighbors.ExceptWith(graph[pivot]);
    foreach (var v in notNeighbors)
    {
        var newR = R.Append(v);
        var vNeighbors = graph[v];

        var newP = new HashSet<string>(P);
        newP.IntersectWith(vNeighbors);

        var newX = new HashSet<string>(X);
        newX.IntersectWith(vNeighbors);

        BronKerbosch(newR, newP, newX, graph, largestClique);
        P.Remove(v);
        X.Add(v);
    }
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
