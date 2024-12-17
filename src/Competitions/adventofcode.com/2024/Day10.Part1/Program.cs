//  --- Day 10: Hoof It ---
//  https://adventofcode.com/2024/day/10

using System.Collections;

using Point = (int X, int Y);
using PointHeight = ((int X, int Y) Point, int H);

{
    var map = File.ReadLines("input1.txt").Where(line => line?.Length > 0)
        .Select(line => line.Select(c => c - '0').ToArray())
        .ToArray();

    int result = Solve(map);
    Console.WriteLine(result);
}

{
    var map = File.ReadLines("input2.txt").Where(line => line?.Length > 0)
        .Select(line => line.Select(c => c - '0').ToArray())
        .ToArray();

    int result = Solve(map);
    Console.WriteLine(result);
}

static int Solve<TMap>(TMap map) where TMap : IReadOnlyList<IReadOnlyList<int>>
{
    var starts = map.SelectMany((row, y) => row.Select((h, x) => new PointHeight((x, y), h)))
        .Where(ph => ph.H == 0);

    var result = 0;
    var hiked = new BitArray(map.Count * map[0].Count);
    foreach (((var x, var y), _) in starts)
    {
        result += GoodHiking(map, hiked, x, y - 1, 1) +
            GoodHiking(map, hiked, x + 1, y, 1) +
            GoodHiking(map, hiked, x, y + 1, 1) +
            GoodHiking(map, hiked, x - 1, y, 1);
        hiked.SetAll(false);
    }

    return result;
}

static int GoodHiking<TMap>(TMap map, BitArray hiked, int x, int y, int height)
    where TMap : IReadOnlyList<IReadOnlyList<int>>
{
    var mh = map.Count;
    var mw = map[0].Count;

    if (y < 0 || y >= map.Count || x < 0 || x >= mw ||
        map[y][x] != height || hiked[y * mw + x])
        return 0;

    hiked[y * mw + x] = true;

    if (height == 9)
        return 1;

    return GoodHiking(map, hiked, x, y - 1, height + 1) +
        GoodHiking(map, hiked, x + 1, y, height + 1) +
        GoodHiking(map, hiked, x, y + 1, height + 1) +
        GoodHiking(map, hiked, x - 1, y, height + 1);
}
