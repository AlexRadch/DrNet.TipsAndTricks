﻿// --- Day 20: Race Condition ---
//  https://adventofcode.com/2024/day/20

using Point = (int X, int Y);
using Cheat = ((int X, int Y) P1, (int X, int Y) P2, int Save);

ProcessFile("input1.txt", 1);
ProcessFile("input2.txt", 100);

static void ProcessFile(string filePath, int minSave)
{
    using var reader = File.OpenText(filePath);
    var map = ReadMap(reader).ToArray();

    var cheats = Solve(map).Where(cheat => cheat.Save >= minSave);

    var result = cheats.Count();
    Console.WriteLine(result);
}

static IEnumerable<string> ReadMap(TextReader reader) =>
    reader.ReadLines();

static IEnumerable<Cheat> Solve<TMap>(TMap map) where TMap : IEnumerable<string>
{
    var height = map.Count();
    var width = map.First().Length;

    var start = map.FlatMap().First(point => point.Value == 'S').Position;
    var end = map.FlatMap().First(point => point.Value == 'E').Position;

    var costMap = map.Select(row => row.Select(value => value == '#' ? -1 : int.MaxValue / 2).ToArray()).ToArray();
    PaveTrack(costMap, start, end);

    return PaveCheats(costMap);
}

static IEnumerable<Cheat> PaveCheats<TMap>(TMap map) where TMap : IList<IList<int>> =>
    map.SelectMany((row, y) => row.SelectMany((value, x) => value >= 0 ? PointCheats(map, (x, y)) : []));


static IEnumerable<Cheat> PointCheats<TMap>(TMap map, Point p) where TMap : IList<IList<int>>
{
    const int maxDuration = 2;
    var height = map.Count;
    var width = map.First().Count;

    var cost = map[p.Y][p.X];
    if (cost < 0)
        yield break;

    for (var ny = Math.Max(0, p.Y - maxDuration); ny <= Math.Min(height - 1, p.Y + maxDuration); ny++)
    {
        int ady = Math.Abs(ny - p.Y);
        for (var nx = Math.Max(0, p.X - maxDuration + ady); nx <= Math.Min(width - 1, p.X + maxDuration - ady); nx++)
        {
            var save = map[ny][nx];
            if (save < 0)
                continue;

            save -= cost + ady + Math.Abs(nx - p.X);
            if (save > 0)
                yield return ((p.X, p.Y), (nx, ny), save);
        }
    }
}

static void PaveTrack<TMap>(TMap map, Point start, Point end) where TMap : IList<IList<int>>
{
    var cost = 0;
    var points = new Point[4];
    map[start.Y][start.X] = 0;

    while (start != end)
    {
        cost++;

        points[0] = new Point(start.X, start.Y - 1);
        points[1] = new Point(start.X + 1, start.Y);
        points[2] = new Point(start.X, start.Y + 1);
        points[3] = new Point(start.X - 1, start.Y);

        var nextPoints = points.Where(next => map[next.Y][next.X] > cost).ToList();

        if (nextPoints.Count == 0)
            throw new NotSupportedException($"No track found from start ({start.X}, {start.Y}) to end ({end.X}, {end.Y}).");
        if (nextPoints.Count > 1)
            throw new NotSupportedException($"Multiple tracks found from start ({start.X}, {start.Y}) to end ({end.X}, {end.Y}).");

        var nextPoint = nextPoints[0];
        map[nextPoint.Y][nextPoint.X] = cost;
        start = nextPoint;
    }

    foreach (var (position, value) in map.FlatMap().Where(pv => pv.Value > cost))
        map[position.Y][position.X] = -1;
}

static class Extensions
{
    public static IEnumerable<(Point Position, T Value)> FlatMap<T>(this IEnumerable<IEnumerable<T>> map) =>
        map.SelectMany((row, y) => row.Select((value, x) => ((x, y), value)));

    public static IEnumerable<string> ReadLines(this TextReader reader)
    {
        while (reader.ReadLine() is string line)
            yield return line;
    }
}
