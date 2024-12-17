//  --- Day 8: Resonant Collinearity ---
//  https://adventofcode.com/2024/day/8

using Point = (int Y, int X);
using MapLetter = (char Letter, (int Y, int X) Point);

{
    var reader = File.OpenText("input1.txt");
    var map = ReadMap(reader).ToArray();

    var result = Solve(map);
    Console.WriteLine(result);
}

{
    var reader = File.OpenText("input2.txt");
    var map = ReadMap(reader).ToArray();

    var result = Solve(map);
    Console.WriteLine(result);
}

static IEnumerable<char[]> ReadMap(TextReader input)
{
    while (input.ReadLine() is string row && row.Length > 0)
        yield return row.ToCharArray();
}

static int Solve<TMap>(TMap map) where TMap : IEnumerable<IEnumerable<char>>
{
    var height = map.Count();
    var width = map.First().Count();

    var positions = SelectMap(map).Where(point => point.Letter != '.')
        .SelectMany(antenna1 => SelectMap(map).Where(antenna2 =>
                antenna2.Letter == antenna1.Letter &&
                (antenna2.Point.Y > antenna1.Point.Y ||
                    (antenna2.Point.Y == antenna1.Point.Y &&
                        antenna2.Point.X > antenna1.Point.X))
            )
            .SelectMany(antenna2 => SolvePair(antenna1.Point, antenna2.Point, height, width))
        )
        .Distinct();

    return positions.Count();
}

static IEnumerable<MapLetter> SelectMap<TMap>(TMap map) where TMap : IEnumerable<IEnumerable<char>>
    => map.SelectMany((row, rIndex) => row.Select((c, cIndex) => new MapLetter(c, new Point(rIndex, cIndex))));

static IEnumerable<Point> SolvePair(Point p1, Point p2, int height, int width)
{
    var dy = p2.Y - p1.Y;
    var dx = p2.X - p1.X;

    var y = p1.Y;
    var x = p1.X;
    do
    {
        yield return new Point(y, x);
        y -= dy;
        x -= dx;
    }
    while (y >= 0 && y < height && x >= 0 && x < width);

    y = p2.Y;
    x = p2.X;
    do
    {
        yield return new Point(y, x);
        y += dy;
        x += dx;
    }
    while (y >= 0 && y < height && x >= 0 && x < width);
}
