// --- Day 18: RAM Run ---
//  https://adventofcode.com/2024/day/18

using System.Data;

using Point = (int X, int Y);

{
    var bytes = ReadBytes("input1.txt").ToArray();

    var (x, y) = Solve(bytes, 7, 7, 13);
    Console.WriteLine($"{x},{y}");
}

{
    var bytes = ReadBytes("input2.txt").ToArray();

    var (x, y) = Solve(bytes, 71, 71, 1025);
    Console.WriteLine($"{x},{y}");
}

static IEnumerable<Point> ReadBytes(string fileName) =>
    File.ReadLines(fileName).Select(line =>
    {
        var parts = line.Split(',');
        return new Point(int.Parse(parts[0]), int.Parse(parts[1]));
    });


static Point Solve<TBytes>(TBytes bytes, int width, int height, int startFrom) where TBytes : IEnumerable<Point>
{
    var l = startFrom;
    var h = bytes.Count() - 1;
    while (l <= h)
    {
        var m = (l + h) / 2;
        if (SolveMap(bytes.Take(m + 1), width, height) >= 0)
            l = m + 1;
        else
            h = m - 1;
    }
    return bytes.ElementAt(l);
}

static int SolveMap<TBytes>(TBytes bytes, int width, int height) where TBytes : IEnumerable<Point>
{
    var distances = new int[height * width];
    var toProcess = new Queue<Point>();

    foreach (var (x, y) in bytes)
        distances[y * width + x] = -1;

    var start = new Point(0, 0);
    var end = new Point(width - 1, height - 1);
    distances[start.Y * width + start.X] = 1;
    toProcess.Enqueue(start);

    while (toProcess.Count > 0)
    {
        var p = toProcess.Dequeue();
        var d = distances[p.Y * width + p.X];

        if (p == end)
            return d - 1;

        Enqueue(p.X, p.Y - 1, d + 1);
        Enqueue(p.X + 1, p.Y, d + 1);
        Enqueue(p.X, p.Y + 1, d + 1);
        Enqueue(p.X - 1, p.Y, d + 1);
    }

    return -1;

    void Enqueue(int x, int y, int d)
    {
        if (!(x >= 0 && x < width && y >= 0 && y < height))
            return;

        ref var distance = ref distances[y * width + x];
        if (distance == 0)
        {
            distance = d;
            toProcess.Enqueue(new Point(x, y));
        }
    }
}
