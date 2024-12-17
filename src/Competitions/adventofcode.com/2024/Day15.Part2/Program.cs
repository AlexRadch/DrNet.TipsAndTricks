//  --- Day 15: Warehouse Woes ---
//  https://adventofcode.com/2024/day/15


using System.Text;

{
    var reader = File.OpenText("input1.txt");

    var map = reader.ReadLines().TakeWhile(line => line.Length > 0).Select(line => line.SelectMany(c => Scale(c)).ToArray()).ToArray();
    var moves = reader.ReadLines().Aggregate(new StringBuilder(), (sb, line) => sb.Append(line), sb => sb.ToString());

    MakeMoves(map, moves);

    var coordinates = BoxCoordinates(map);
    var result = coordinates.Sum();

    Console.WriteLine(result);
}

{
    var reader = File.OpenText("input2.txt");

    var map = reader.ReadLines().TakeWhile(line => line.Length > 0).Select(line => line.SelectMany(c => Scale(c)).ToArray()).ToArray();
    var moves = reader.ReadLines().Aggregate(new StringBuilder(), (sb, line) => sb.Append(line), sb => sb.ToString());

    MakeMoves(map, moves);

    var coordinates = BoxCoordinates(map);
    var result = coordinates.Sum();

    Console.WriteLine(result);
}

{
    var reader = File.OpenText("input3.txt");

    var map = reader.ReadLines().TakeWhile(line => line.Length > 0).Select(line => line.SelectMany(c => Scale(c)).ToArray()).ToArray();
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
        switch (move)
        {
            case '^':
                if (CanMoveUpDown(map, x, y - 1, -1))
                {
                    MoveUpDown(map, x, y, -1);
                    y--;
                }
                break;
            case 'v':
                if (CanMoveUpDown(map, x, y + 1, 1))
                {
                    MoveUpDown(map, x, y, 1);
                    y++;
                }
                break;
            case '<':
                if (MoveLeftRight(map, x, y, -1))
                    x--;
                break;
            case '>':
                if (MoveLeftRight(map, x, y, + 1))
                    x++;
                break;
        }
    }
    return;
}

static bool CanMoveUpDown(char[][] map, int x, int y, int dy) => map[y][x] switch
{
    '#' => false,
    '[' => CanMoveUpDown(map, x, y + dy, dy) && CanMoveUpDown(map, x + 1, y + dy, dy),
    ']' => CanMoveUpDown(map, x - 1, y + dy, dy) && CanMoveUpDown(map, x, y + dy, dy),
    _ => true,
};

static void MoveUpDown(char[][] map, int x, int y, int dy)
{
    var c = map[y][x];
    switch (c)
    {
        case '.':
            return;
        case '[':
            MoveUpDown(map, x + 1, y + dy, dy);
            map[y + dy][x + 1] = map[y][x + 1];
            map[y][x + 1] = '.';
            break;
        case ']':
            MoveUpDown(map, x - 1, y + dy, dy);
            map[y + dy][x - 1] = map[y][x - 1];
            map[y][x - 1] = '.';
            break;
    }
    MoveUpDown(map, x, y + dy, dy);
    map[y + dy][x] = c;
    map[y][x] = '.';
}

static bool MoveLeftRight(char[][] map, int x, int y, int dx)
{
    var c = map[y][x];
    switch (c)
    {
        case '#':
            return false;
        case '@':
            if (!MoveLeftRight(map, x + dx, y, dx))
                return false;
            break;
        case '[':
        case ']':
            if (!MoveLeftRight(map, x + dx + dx, y, dx))
                return false;
            map[y][x + dx + dx] = map[y][x + dx];
            break;
        default: 
            return true;
    }

    map[y][x + dx] = c;
    map[y][x] = '.';
    return true;
}

static string Scale(char c) => c switch
{
    'O' => "[]",
    '@' => "@.",
    _ => new string(c, 2),
};

static IEnumerable<int> BoxCoordinates<TMap>(TMap map) where TMap : IEnumerable<IEnumerable<char>> =>
    map.Items().Where(item => item.C == '[').Select(item => item.Y * 100 + item.X);

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