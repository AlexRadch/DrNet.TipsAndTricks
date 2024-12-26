// --- Day 24: Crossed Wires ---
//  https://adventofcode.com/2024/day/24

using System.Collections.ObjectModel;

ProcessFile("input1.txt");
Console.WriteLine();

//ProcessFile("input2.txt");
//Console.WriteLine();

static void ProcessFile(string filePath)
{
    using var reader = File.OpenText(filePath);
    var height = ReadLocksAndKeys(reader, out var locks, out var keys);

    var result = Solve(height, locks, keys);
    Console.WriteLine(result);
}


static int ReadLocksAndKeys(TextReader reader,
    out ReadOnlyCollection<ReadOnlyCollection<int>> locks,
    out ReadOnlyCollection<ReadOnlyCollection<int>> keys)
{
    int height = 0;
    var locksList = new List<ReadOnlyCollection<int>>();
    var keysList = new List<ReadOnlyCollection<int>>();

    foreach (var pattern in ReadPatterns(reader))
    {
        height = pattern.Count() - 2;
        var firstLine = pattern.First();
        if (firstLine[0] == '#')
        {
            var aLock = new int[firstLine.Length];
            foreach (var line in pattern.Skip(1).SkipLast(1))
            {
                for (var i = 0; i < aLock.Length; i++)
                    if (line[i] == '#')
                        aLock[i]++;
            }
            locksList.Add(aLock.AsReadOnly());
        }
        else
        {
            var key = new int[firstLine.Length];
            foreach (var line in pattern.Skip(1).SkipLast(1))
            {
                for (var i = 0; i < key.Length; i++)
                    if (line[i] == '#')
                        key[i]++;
            }
            keysList.Add(key.AsReadOnly());
        }
    }

    locks = locksList.AsReadOnly();
    keys = keysList.AsReadOnly();
    return height;
}

static IEnumerable<IEnumerable<string>> ReadPatterns(TextReader reader)
{
    while (reader.ReadLines().TakeWhile(line => !string.IsNullOrEmpty(line)).ToList() is { } pattern &&
        pattern.Count != 0)
    {
        yield return pattern;
    }
}

static int Solve<TLocks, TKeys>(int height, TLocks locks, TKeys keys)
    where TLocks : IEnumerable<IEnumerable<int>>
    where TKeys : IEnumerable<IEnumerable<int>>
    => keys.Select(key => locks.Count(aLock => IsFit(height, aLock, key))).Sum();

static bool IsFit<TLock, TKey>(int height, TLock aLock, TKey key)
    where TLock : IEnumerable<int>
    where TKey : IEnumerable<int>
    => aLock.Zip(key, (lockPin, keyPin) => lockPin + keyPin).All(len => len <= height);

static class Extensions
{
    public static IEnumerable<string> ReadLines(this TextReader reader)
    {
        while (reader.ReadLine() is string line)
            yield return line;
    }
}
