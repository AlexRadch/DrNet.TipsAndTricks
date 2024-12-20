// --- Day 20: Race Condition ---
//  https://adventofcode.com/2024/day/20

using Point = (int X, int Y);
using Cheat = ((int X, int Y) P1, (int X, int Y) P2, int Save);

{
    using var reader = File.OpenText("input1.txt");
    var map = ReadMap(reader).ToArray();

    var cheats = Solve(map);

    var result = cheats.Where(cheat => cheat.Save > 0).Count();
    Console.WriteLine(result);
}

{
    using var reader = File.OpenText("input2.txt");
    var map = ReadMap(reader).ToArray();

    var cheats = Solve(map);

    var result = cheats.Where(cheat => cheat.Save >= 100).Count();
    Console.WriteLine(result);
}

static IEnumerable<string> ReadMap(TextReader reader) =>
    reader.ReadLines();

static IEnumerable<Cheat> Solve<TMap>(TMap map) where TMap : IEnumerable<string>
{
    var height = map.Count();
    var width = map.First().Length;

    var start = map.FlatMap().Where(point => point.Value == 'S').First().Position;
    var end = map.FlatMap().Where(point => point.Value == 'E').First().Position;

    var costMap = map.Select(row => row.Select(value => value == '#' ? -1 : int.MaxValue / 2).ToArray()).ToArray();
    PaveTrack(costMap, start, end);

    var cheats = PaveCheats(costMap);

    return cheats;
}

static IEnumerable<Cheat> PaveCheats<TMap>(TMap map) where TMap : IList<IList<int>>
{
    var cheats = map.SelectMany((row, y) => row.SelectMany((value, x) => value >= 0 ? PointCheats(map, (x, y)) : []));
    return cheats;
}

static IEnumerable<Cheat> PointCheats<TMap>(TMap map, Point p) where TMap : IList<IList<int>>
{
    var height = map.Count;
    var width = map.First().Count;

    var cost = map[p.Y][p.X];
    if (cost < 0)
        yield break;

    {
        if (p.Y - 2 >= 0 && map[p.Y - 1][p.X] < 0 && map[p.Y - 2][p.X] - cost - 2 is int save && save > 0)
            yield return new Cheat((p.X, p.Y - 1), (p.X, p.Y - 2), save);
    }

    {
        if (p.X + 2 < width && map[p.Y][p.X + 1] < 0 && map[p.Y][p.X + 2] - cost - 2 is int save && save > 0)
            yield return new Cheat((p.X + 1, p.Y), (p.X + 2, p.Y), save);
    }

    {
        if (p.Y + 2 < height && map[p.Y + 1][p.X] < 0 && map[p.Y + 2][p.X] - cost - 2 is int save && save > 0)
            yield return new Cheat((p.X, p.Y + 1), (p.X, p.Y + 2), save);
    }

    {
        if (p.X - 2 >= 0 && map[p.Y][p.X - 1] < 0 && map[p.Y][p.X - 2] - cost - 2 is int save && save > 0)
            yield return new Cheat((p.X - 1, p.Y), (p.X - 2, p.Y), save);
    }
}


static void PaveTrack<TMap>(TMap map, Point start, Point end) where TMap : IList<IList<int>>
{
    var cost = 0;
    map[start.Y][start.X] = 0;
    while (start != end)
    {
        cost++;
        
        var count = 0;
        Point next;
        Point next2 = new();

        next = new Point(start.X, start.Y - 1);
        if (map[next.Y][next.X] > cost)
        {
            map[next.Y][next.X] = cost;
            next2 = next;
            count++;
        }

        next = new Point(start.X + 1, start.Y);
        if (map[next.Y][next.X] > cost)
        {
            map[next.Y][next.X] = cost;
            next2 = next;
            count++;
        }

        next = new Point(start.X, start.Y + 1);
        if (map[next.Y][next.X] > cost)
        {
            map[next.Y][next.X] = cost;
            next2 = next;
            count++;
        }

        next = new Point(start.X - 1, start.Y);
        if (map[next.Y][next.X] > cost)
        {
            map[next.Y][next.X] = cost;
            next2 = next;
            count++;
        }

        if (count < 1)
            throw new NotSupportedException("No track");
        if (count > 1)
            throw new NotSupportedException("Multitrack");

        start = next2;
    }

    foreach (var (position, value) in map.FlatMap().Where(pv => pv.Value > cost))
        map[position.Y][position.X] = -1;
}

static class Extensions
{
    public static IEnumerable<(Point Position, T Value)> FlatMap<T>(this IEnumerable<IEnumerable<T>> map)
        => map.SelectMany((row, y) => row.Select((value, x) => ((x, y), value)));

    public static IEnumerable<string> ReadLines(this TextReader reader)
    {
        while (reader.ReadLine() is string line)
            yield return line;
    }
}
