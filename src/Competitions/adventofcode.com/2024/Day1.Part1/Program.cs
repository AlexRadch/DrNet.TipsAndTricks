
{
    TextReader input = File.OpenText("input2.txt");

    List<int> list1 = [];
    List<int> list2 = [];
    ReadInput(input, list1, list2);

    var result = Solve(list1, list2);

    Console.WriteLine($"{result}");
}

static void ReadInput<TCollection>(TextReader input, TCollection list1, TCollection list2) where TCollection: ICollection<int>
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

static int Solve<TSource>(TSource list1, TSource list2) where TSource : IEnumerable<int> =>
    list1
        .OrderBy(x => x)
        .Zip(list2.OrderBy(x => x), (x, y) => Math.Abs(x - y))
        //.Zip(list2.OrderBy(x => x))
        //.Select(z => z.First - z.Second)
        //.Select(Math.Abs)
        .Sum();
