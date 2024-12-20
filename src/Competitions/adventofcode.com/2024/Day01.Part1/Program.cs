//  --- Day 1: Historian Hysteria ---
//  https://adventofcode.com/2024/day/1

ProcessInputFile("input1.txt");
ProcessInputFile("input2.txt");

static void ProcessInputFile(string filePath)
{
    List<int> list1 = [];
    List<int> list2 = [];

    {
        using TextReader input = File.OpenText(filePath);
        ReadInput(input, list1, list2);
    }

    var result = Solve(list1, list2);

    Console.WriteLine(result);
}


static void ReadInput<TCollection>(TextReader input, TCollection list1, TCollection list2) where TCollection: ICollection<int>
{

    foreach (var line in input.ReadLines())
    {
        var items = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (items.Length >= 2)
        {
            list1.Add(int.Parse(items[0]));
            list2.Add(int.Parse(items[1]));
        }
    }
}

static int Solve<TSource>(TSource list1, TSource list2) where TSource : IEnumerable<int> =>
    list1
        .OrderBy(x => x)
        .Zip(list2.OrderBy(x => x), (x, y) => Math.Abs(x - y))
        .Sum();

static class Extensions
{
    public static IEnumerable<string> ReadLines(this TextReader reader)
    {
        while (reader.ReadLine() is { } line)
            yield return line;
    }
}
