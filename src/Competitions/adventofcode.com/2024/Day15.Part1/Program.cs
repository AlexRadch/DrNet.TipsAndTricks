//  --- Day 15: Warehouse Woes ---
//  https://adventofcode.com/2024/day/15


using System.Text;

{
    var reader = File.OpenText("input1.txt");

    var map = reader.ReadLines().TakeWhile(line => line.Length > 0).Select(line => line.ToArray()).ToArray();
    var moves = reader.ReadLines().Aggregate(new StringBuilder(), (sb, line) => sb.Append(line), sb => sb.ToString());

    MakeMoves(map, moves);

    var coordinates = BoxCoordinates(map);
    var result = coordinates.Sum();

    Console.WriteLine(result);
}

{
    var reader = File.OpenText("input2.txt");

    var map = reader.ReadLines().TakeWhile(line => line.Length > 0).Select(line => line.ToArray()).ToArray();
    var moves = reader.ReadLines().Aggregate(new StringBuilder(), (sb, line) => sb.Append(line), sb => sb.ToString());

    MakeMoves(map, moves);

    var coordinates = BoxCoordinates(map);
    var result = coordinates.Sum();

    Console.WriteLine(result);
}

{
    var reader = File.OpenText("input3.txt");

    var map = reader.ReadLines().TakeWhile(line => line.Length > 0).Select(line => line.ToArray()).ToArray();
    var moves = reader.ReadLines().Aggregate(new StringBuilder(), (sb, line) => sb.Append(line), sb => sb.ToString());

    MakeMoves(map, moves);

    var coordinates = BoxCoordinates(map);
    var result = coordinates.Sum();

    Console.WriteLine(result);
}

static void MakeMoves(char[][] map, string moves)
{
    (var x, var y, _) = map.Items().Where(item => item.C == '@').First();
    foreach (var move in moves)
    {
        (var dx, var dy) = move switch
        {
            '^' => (0, -1),
            'v' => (0, 1),
            '<' => (-1, 0),
            '>' => (1, 0),
            _ => throw new NotSupportedException()
        };

        var nx = x; var ny = y;
        while (true)
        {
            nx += dx; ny += dy;
            var c = map[ny][nx];
            if (c == '#')
                break;
            if (c == 'O')
                continue;

            map[ny][nx] = 'O';

            map[y][x] = '.';
            x += dx; y += dy;
            map[y][x] = '@';
            break;
        }
    }
    return;
}

static IEnumerable<int> BoxCoordinates<TMap>(TMap map) where TMap : IEnumerable<IEnumerable<char>> =>
    map.Items().Where(item => item.C == 'O').Select(item => item.Y * 100 + item.X);

static class Extensions
{
    public static IEnumerable<(int X, int Y, char C)> Items<TMap>(this TMap map) where TMap : IEnumerable<IEnumerable<char>> =>
        map.SelectMany((line, y) => line.Select((c, x) => (x, y, c)));


    public static IEnumerable<string> ReadLines(this TextReader reader)
    {
        while (reader.ReadLine() is string line)
            yield return line;
    }
}