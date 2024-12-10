//  --- Day 1: Historian Hysteria ---
//  https://adventofcode.com/2024/day/1

using System.Runtime.InteropServices;

{
    TextReader input = File.OpenText("input1.txt");

    List<int> list1 = [];
    List<int> list2 = [];
    ReadInput(input, list1, list2);

    var result = Solve(list1, list2);

    Console.WriteLine($"{result}");
}

{
    TextReader input = File.OpenText("input2.txt");

    List<int> list1 = [];
    List<int> list2 = [];
    ReadInput(input, list1, list2);

    var result = Solve(list1, list2);

    Console.WriteLine($"{result}");
}

static void ReadInput<TCollection>(TextReader input, TCollection list1, TCollection list2) where TCollection : ICollection<int>
{
    while (input.ReadLine() is string line)
    {
        var items = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (items.Length >= 2)
        {
            list1.Add(int.Parse(items[0]));
            list2.Add(int.Parse(items[1]));
        }
    }
}

static int Solve<TSource>(TSource list1, TSource list2) where TSource : IEnumerable<int>
{
    var dict = BuildCountDictionary(list2);

    return list1
        .Select(x => dict.TryGetValue(x, out var count) ? x * count : 0)
        .Sum();
}

static Dictionary<int, int> BuildCountDictionary<TSource>(TSource source)
    where TSource : IEnumerable<int> 
{
    Dictionary<int, int> countsBy = [];

    foreach (var x in source)
    {
        ref int currentCount = ref CollectionsMarshal.GetValueRefOrAddDefault(countsBy, x, out _);
        currentCount++;
    }
    return countsBy;
}