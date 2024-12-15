//  --- Day 14: Restroom Redoubt ---
//  https://adventofcode.com/2024/day/14

using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

using Robot = (int X, int Y, int DX, int DY);
using Point = (int X, int Y);
using System.Text;

{
    var robots = File.ReadLines("input1.txt")
        .Select(ReadRobot)
        .ToArray().AsReadOnly();

    var result = Solve(robots, 11, 7);

    Console.WriteLine($"{result}");
}

{
    var robots = File.ReadLines("input2.txt")
        .Select(ReadRobot)
        .ToArray().AsReadOnly();

    var result = Solve(robots, 101, 103);

    Console.WriteLine($"{result}");
}

static Robot ReadRobot(string line)
{
    var matches = SignIntRegex().Matches(line);

    return new Robot(
        int.Parse(matches[0].Value), int.Parse(matches[1].Value),
        int.Parse(matches[2].Value), int.Parse(matches[3].Value));
}

static int Solve<TRobots>(TRobots robots, int wide, int tall) where TRobots : IEnumerable<Robot>
{
    var totalCount = robots.Count();
    var wideHalf = wide / 2;

    IEnumerable<Point> max = [];
    int result = -1;
    for (var seconds = 0; seconds < 10000; seconds++)
    {
       
        var points = robots.Select(robot => Evaluate(robot, wide, tall, seconds));
        var picture = MaxPicture(points);

        if (picture.Count() > max.Count())
        {
            max = picture;
            result = seconds;

            var map = MapString(picture);
            Console.WriteLine(map);

            if (picture.Count() * 2 > totalCount)
                return result;
        }
    }
    return result;
}

static Point Evaluate(Robot robot, int wide, int tall, int seconds) =>
    checked(
        (((robot.X + robot.DX * seconds) % wide + wide) % wide,
        ((robot.Y + robot.DY * seconds) % tall + tall) % tall)
    );

static IEnumerable<Point> MaxPicture(IEnumerable<Point> points)
{
    var set = points.ToHashSet();
    var visited = new HashSet<Point>();

    IEnumerable<Point> result = [];
    foreach (var point in set)
    {
        if (visited.Contains(point))
            continue;

        var picture = GetPicture(set, point);
        visited.UnionWith(picture);

        if (picture.Count() > result.Count())
            result = picture;
    }

    //{
    //    var map = MapString(set);
    //    Console.WriteLine(map);
    //}
    //{
    //    var map = MapString(result);
    //    Console.WriteLine(map);
    //}

    return result;
}

static IEnumerable<Point> GetPicture(ISet<Point> points, Point start)
{
    var result = new HashSet<Point>();

    var queue = new Queue<Point>();
    queue.Enqueue(start);

    while (queue.Count > 0)
    {
        var point = queue.Dequeue();
        if (!result.Add(point))
            continue;

        foreach(var (X, Y) in Directions)
        {
            var next = new Point(point.X + X, point.Y + Y);
            if (result.Contains(next) || !points.Contains(next))
                continue;
            queue.Enqueue(next);
        }
    }

    return result;
}

static string MapString(IEnumerable<Point> points)
{
    var width = points.Select(point => point.X).Max() + 1;
    var height = points.Select(point => point.Y).Max() + 1;
    var result = new StringBuilder(width * height);

    for (var y = 0; y < height; y++)
    {
        for (var x = 0; x < width; x++)
        {
            if (points.Contains((x, y)))
                result.Append('#');
            else
                result.Append('.');
        }
        result.AppendLine();
    }

    return result.ToString();
}

partial class Program
{
    [GeneratedRegex(@"-?\d+")]
    private static partial Regex SignIntRegex();

    static readonly ReadOnlyCollection<Point> Directions = new Point[]
        {
            (-1, -1), (0, -1), (1, -1),
            (1, 0), (1, 1),
            (0, 1), (-1, 1),
            (-1, 0),
        }.AsReadOnly();
}