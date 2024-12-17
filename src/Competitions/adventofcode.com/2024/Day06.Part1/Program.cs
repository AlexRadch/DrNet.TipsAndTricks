//  --- Day 6: Guard Gallivant ---
//  https://adventofcode.com/2024/day/6

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

static int Solve<TMap>(TMap map) where TMap : IList<char[]>
{
    var height = map.Count;
    var width = map[0].Length;

    (int y, int x) = map.Select((row, y) => (Y: y, X: row.AsSpan().IndexOf('^'))).First(pos => pos.X >= 0);
    int dy = -1; int dx = 0;

    var result = 0;
    while (true)
    {
        if (map[y][x] != 'X')
            result++;
        map[y][x] = 'X';

    Turn:
        var ny = y + dy;
        var nx = x + dx;
        if (ny < 0 || ny >= height || nx < 0 || nx >= width)
            break;
        if (map[ny][nx] == '#')
        {
            (dy, dx) = (dx, -dy);
            goto Turn;
        }

        y = ny;
        x = nx;
    }

    return result;
}