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

    var result = Solve1(pairs);
    Console.WriteLine(result);

    result = Solve2(pairs);
    Console.WriteLine(result);
}

static IEnumerable<Pair> ReadPairs(TextReader reader) =>
    reader.ReadLines().Select(line => new Pair(line[..2], line[3..5]));

static int Solve1<TPairs>(TPairs pairs) where TPairs : IEnumerable<Pair>
{
    var pairsDict = new Dictionary<string, HashSet<Pair>>();
    foreach (var pair in pairs)
    {
        if (!pairsDict.TryGetValue(pair.Item1, out var set1))
            pairsDict.Add(pair.Item1, set1 = []);
        set1.Add(pair);

        if (!pairsDict.TryGetValue(pair.Item2, out var set2))
            pairsDict.Add(pair.Item2, set2 = []);

        set2.Add(pair);
    }

    var triplets = new HashSet<Triplet>();
    foreach (var first in pairsDict.Keys
        .Where(key => key[0] == 't')
        .SelectMany(key => pairsDict[key])
    )
    {
        var set = pairsDict[first.Item1];

        foreach (var second in set)
        {
            if (second.Item1 != first.Item1 && second.Item1 != first.Item2)
            {
                var third = new Pair(second.Item1, second.Item2 == first.Item1 ? first.Item2 : first.Item1);
                if (pairsDict[second.Item1].Contains(third))
                    triplets.Add(new Triplet(second.Item1, first.Item1, first.Item2));
            }
            if (second.Item2 != first.Item1 && second.Item2 != first.Item2)
            {
                var third = new Pair(second.Item1 == first.Item1 ? first.Item2 : first.Item1, second.Item2);
                if (pairsDict[second.Item2].Contains(third))
                    triplets.Add(new Triplet(first.Item1, first.Item2, second.Item2));
            }
        }
    }

    //var result = triplets.Where(triplet => triplet.Item1[0] == 't' || triplet.Item2[0] == 't' || triplet.Item3[0] == 't');
    //return result.Count();

    return triplets.Count;
}

static int Solve2<TPairs>(TPairs pairs) where TPairs : IEnumerable<Pair>
{
    var pairsDict = new Dictionary<string, HashSet<string>>();
    foreach (var pair in pairs)
    {
        if (!pairsDict.TryGetValue(pair.Item1, out var set1))
            pairsDict.Add(pair.Item1, set1 = []);
        set1.Add(pair.Item2);

        //if (!pairsDict.TryGetValue(pair.Item2, out var set2))
        //    pairsDict.Add(pair.Item2, set2 = []);

        //set2.Add(pair.Item1);
    }

    var empty = new HashSet<string>();
    var triplets = pairsDict
        .SelectMany(item1 => item1.Value
            .SelectMany(item2 => item1.Value
                .Intersect(pairsDict.GetValueOrDefault(item2, empty))
                //.Where(item3 => item1.Key[0] == 't' || item2[0] == 't' || item3[0] == 't')
                //.Select(item3 => new Triplet(item1.Key, item2, item3))
                .Select(item3 => (Item1: item1.Key, Item2: item2, Item3: item3))
            )
        )
        .Where(triplet => triplet.Item1[0] == 't' || triplet.Item2[0] == 't' || triplet.Item3[0] == 't')
        //.Distinct()
        ;

    return triplets.Count();
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

readonly record struct Triplet
{
    public string Item1 { get; init; }
    public string Item2 { get; init; }
    public string Item3 { get; init; }

    public Triplet(string item1, string item2, string item3)
    {
        var items = new[] { item1, item2, item3 };
        Array.Sort(items, StringComparer.Ordinal);
        Item1 = items[0];
        Item2 = items[1];
        Item3 = items[2];
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
