// --- Day 20: Race Condition ---
//  https://adventofcode.com/2024/day/20

using Point = (int X, int Y);
using Cheat = ((int X, int Y) P1, (int X, int Y) P2, int Save);

ProcessFile("input1.txt", 20, cheat => cheat.Save >= 50);
ProcessFile("input2.txt", 20, cheat => cheat.Save >= 100);

static void ProcessFile(string filePath, int maxDuration, Func<Cheat, bool> filter)
{
    using var reader = File.OpenText(filePath);
    var map = ReadMap(reader).ToArray();

    var height = map.Length;
    var width = map[0].Length;

    var cheats = Solve(map, maxDuration)
        .Where(filter)
        //.OrderBy(cheat => cheat.Save)
        ;

    var result = cheats.Count();
    Console.WriteLine(result);

    //var group = cheats.GroupBy(cheat => cheat.Save, (key, cheats) => (key, cheats.Count()));
    //Console.WriteLine(group.Count());
}

static IEnumerable<string> ReadMap(TextReader reader) =>
    reader.ReadLines();

static IEnumerable<Cheat> Solve<TMap>(TMap map, int maxDuration) where TMap : IEnumerable<string>
{
    var height = map.Count();
    var width = map.First().Length;

    var start = map.FlatMap().First(point => point.Value == 'S').Position;
    var end = map.FlatMap().First(point => point.Value == 'E').Position;

    var costMap = map.Select(row => row.Select(value => value == '#' ? -1 : int.MaxValue / 2).ToArray()).ToArray();
    PaveTrack(costMap, start, end);

    return PaveCheats(costMap, maxDuration)
        .GroupBy(cheat => (cheat.P1, cheat.P2), (key, cheats) => (key.P1, key.P2, cheats.Max(cheat => cheat.Save)));
}

static IEnumerable<Cheat> PaveCheats<TMap>(TMap map, int maxDuration) where TMap : IList<IList<int>> =>
    map.SelectMany((row, y) => row.SelectMany((value, x) => value >= 0 ? PointCheats(map, (x, y), maxDuration) : []));


static IEnumerable<Cheat> PointCheats<TMap>(TMap map, Point p, int maxDuration) where TMap : IList<IList<int>>
{
    var height = map.Count;
    var width = map.First().Count;

    var cost = map[p.Y][p.X];
    if (cost < 0)
        yield break;

    for (var dy = -maxDuration; dy <= maxDuration; dy++)
        for (var dx = -maxDuration; dx <= maxDuration; dx++)
        {
            var nx = p.X + dx;
            if (nx < 0 || nx >= width)
                continue;

            var ny = p.Y + dy;
            if (ny < 0 || ny >= height)
                continue;

            var duration = Math.Abs(dx) + Math.Abs(dy);
            if (duration > maxDuration)
                continue;

            if (map[ny][nx] < 0)
                continue;

            var save = map[ny][nx] - cost - duration;
            if (save > 0)
                yield return ((p.X, p.Y), (nx, ny), save);
        }

    //Point[] points = [(p.X, p.Y - 1), (p.X + 1, p.Y), (p.X, p.Y + 1), (p.X - 1, p.Y)];

    //foreach (var (sx, sy) in points)
    //{
    //    if (map[sy][sx] >= 0)
    //        continue;

    //    for (var dy = -(maxDuration - 1); dy <= maxDuration - 1; dy++)
    //        for (var dx = -(maxDuration - 1); dx <= maxDuration - 1; dx++)
    //        {
    //            var duration = Math.Abs(dx) + Math.Abs(dy);
    //            if (duration > maxDuration - 1)
    //                continue;

    //            var (nx, ny) = (sx + dx, sy + dy);
    //            if (nx < 0 || ny < 0 || nx >= width || ny >= height || map[ny][nx] < 0)
    //                continue;

    //            var save = map[ny][nx] - cost - duration - 1;
    //            if (save > 0)
    //                yield return ((sx, sy), (nx, ny), save);
    //        }
    //}
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
