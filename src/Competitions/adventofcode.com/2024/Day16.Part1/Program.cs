﻿// ---Day 16: Reindeer Maze ---
//  https://adventofcode.com/2024/day/16

using Point = (int X, int Y);

{
    var map = File.ReadLines("input1.txt").ToArray();
    var result = Solve(map);
    Console.WriteLine($"{result}");
}

{
    var map = File.ReadLines("input2.txt").ToArray();
    var result = Solve(map);
    Console.WriteLine($"{result}");
}

{
    var map = File.ReadLines("input3.txt").ToArray();
    var result = Solve(map);
    Console.WriteLine($"{result}");
}

static int Solve<TMap>(TMap map) where TMap : IList<string>
{
    (var sx, var sy, _) = map.Items().Where(item => item.C == 'S').First();
    (var ex, var ey, _) = map.Items().Where(item => item.C == 'E').First();
    var endCost = Costs.MaxValue;

    var points = new Dictionary<Point, Costs>()
    {
        { (sx, sy), new Costs { Right = 0 } }
    };

    var toProcess = new HashSet<Point>()
    {
        (sx, sy)
    };

    while(toProcess.Count > 0)
    {
        (var x, var y) = toProcess.First();
        toProcess.Remove((x, y));
        var costs = points[(x, y)];

        if (map[y - 1][x] != '#')
            UpdateUpCost((x, y - 1), costs.ToUp + 1);
        if (map[y][x + 1] != '#')
            UpdateRightCost((x + 1, y), costs.ToRight + 1);
        if (map[y + 1][x] != '#')
            UpdateDownCost((x, y + 1), costs.ToDown + 1);
        if (map[y][x - 1] != '#')
            UpdateLeftCost((x - 1, y), costs.ToLeft + 1);
    }

    if (points.TryGetValue((ex, ey), out var result))
        return result.Min;

    return -1;

    void UpdateUpCost(Point point, int value)
    {
        if (value >= endCost)
            return;

        if (!points.TryGetValue(point, out var costs))
        {
            costs = new Costs();
            points[point] = costs;
        }
        if (value < costs.Up)
        {
            costs.Up = value;
            toProcess.Add(point);

            if (point.X == ex && point.Y == ey)
                endCost = costs.Min;
        }
    }

    void UpdateRightCost(Point point, int value)
    {
        if (value >= endCost)
            return;

        if (!points.TryGetValue(point, out var costs))
        {
            costs = new Costs();
            points[point] = costs;
        }
        if (value < costs.Right)
        {
            costs.Right = value;
            toProcess.Add(point);

            if (point.X == ex && point.Y == ey)
                endCost = costs.Min;
        }
    }

    void UpdateDownCost(Point point, int value)
    {
        if (value >= endCost)
            return;

        if (!points.TryGetValue(point, out var costs))
        {
            costs = new Costs();
            points[point] = costs;
        }
        if (value < costs.Down)
        {
            costs.Down = value;
            toProcess.Add(point);

            if (point.X == ex && point.Y == ey)
                endCost = costs.Min;
        }
    }

    void UpdateLeftCost(Point point, int value)
    {
        if (value >= endCost)
            return;

        if (!points.TryGetValue(point, out var costs))
        {
            costs = new Costs();
            points[point] = costs;
        }
        if (value < costs.Left)
        {
            costs.Left = value;
            toProcess.Add(point);

            if (point.X == ex && point.Y == ey)
                endCost = costs.Min;
        }
    }
}


record class Costs
{
    public const int MaxValue = int.MaxValue / 2;
    const int TurnConst = 1000;

    public int Up { get; set; } = MaxValue;
    public int Right { get; set; } = MaxValue;
    public int Down { get; set; } = MaxValue;
    public int Left { get; set; } = MaxValue;

    public int ToUp
    {
        get => Math.Min(Math.Min(Math.Min(Up, Right + TurnConst), Left + TurnConst), Down + TurnConst + TurnConst);
    }
    public int ToRight
    {
        get => Math.Min(Math.Min(Math.Min(Right, Down + TurnConst), Up + TurnConst), Left + TurnConst + TurnConst);
    }
    public int ToDown
    {
        get => Math.Min(Math.Min(Math.Min(Down, Left + TurnConst), Right + TurnConst), Up + TurnConst + TurnConst);
    }
    public int ToLeft
    {
        get => Math.Min(Math.Min(Math.Min(Left, Up + TurnConst), Down + TurnConst), Right + TurnConst + TurnConst);
    }
    public int Min
    {
        get => Math.Min(Math.Min(Math.Min(Up, Right), Left), Down);
    }
}

static class Extensions
{
    public static IEnumerable<(int X, int Y, char C)> Items<TMap>(this TMap map) where TMap : IEnumerable<IEnumerable<char>> =>
        map.SelectMany((line, y) => line.Select((c, x) => (x, y, c)));
}