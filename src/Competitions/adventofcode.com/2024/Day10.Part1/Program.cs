//  --- Day 10: Hoof It ---
//  https://adventofcode.com/2024/day/10

{
    var map = File.ReadLines("input1.txt").Where(line => line?.Length > 0).ToArray();

    int result = Solve(map);
    Console.WriteLine($"{result}");
}

{
    var map = File.ReadLines("input1.txt").Where(line => line?.Length > 0).ToArray();

    int result = Solve(map);
    Console.WriteLine($"{result}");
}

static int Solve<TMap>(TMap map) where TMap : IReadOnlyList<string>
{
    return 0;
}
