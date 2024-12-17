//  --- Day 6: Guard Gallivant ---
//  https://adventofcode.com/2024/day/6

using System.Collections.ObjectModel;

using Position = (int Row, int Column);

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

    var pos = map.Select((row, y) => new Position(y, row.AsSpan().IndexOf('^'))).First(pos => pos.Column >= 0);
    var direction = Direction.Up;
    map[pos.Row][pos.Column] = '.';

    var mapFlags = new Dictionary<Position, Flags>();

    var result = 0;
    while (true)
    {
        if (!SetFlag(mapFlags, pos, direction))
            break;

        var y = pos.Row + DirectionDeltaY[(int)direction];
        var x = pos.Column + DirectionDeltaX[(int)direction];
        if (y < 0 || y >= height || x < 0 || x >= width)
            break;

        if (map[y][x] == '#')
        {
            direction = (Direction)(((int)direction + 1) % 4);
            continue;
        }

        if (CanAddObstruction(map, mapFlags, pos, direction))
            result++;
        pos = (y, x);
    }

    return result;
}

static bool CanAddObstruction<TMap, TMapFlags>(TMap map, TMapFlags mapFlags, Position pos, Direction direction)
    where TMap : IList<char[]>
    where TMapFlags : IReadOnlyDictionary<Position, Flags>
{
    var height = map.Count;
    var width = map[0].Length;

    var oY = pos.Row + DirectionDeltaY[(int)direction];
    var oX = pos.Column + DirectionDeltaX[(int)direction];
    if (mapFlags.ContainsKey((oY, oX)))
        return false;

    var flags = new Dictionary<Position, Flags>(mapFlags);
    map[oY][oX] = '#';
    try
    {
        direction = (Direction)(((int)direction + 1) % 4);
        while (true)
        {
            if (!SetFlag(flags, pos, direction))
                return true;

            var y = pos.Row + DirectionDeltaY[(int)direction];
            var x = pos.Column + DirectionDeltaX[(int)direction];
            if (y < 0 || y >= height || x < 0 || x >= width)
                return false;

            if (map[y][x] == '#')
            {
                direction = (Direction)(((int)direction + 1) % 4);
                continue;
            }

            pos = (y, x);
        }
    }
    finally
    {
        map[oY][oX] = '.';
    }
}

static bool SetFlag<TMapFlags>(TMapFlags mapFlags, Position position, Direction direction) 
    where TMapFlags: IDictionary<Position, Flags>
{
    var flags = mapFlags.ContainsKey(position) ? mapFlags[position] : Flags.None;
    var flag = DirectionFlag[(int)direction];

    if ((flags & flag) == 0)
    {
        mapFlags[position] = flags | flag;
        return true;
    }
    return false;
}

enum Direction
{
    Up,
    Right,
    Down,
    Left,
}

[Flags]
enum Flags
{
    None = 0,
    Up = 1,
    Right = 2,
    Down = 4,
    Left = 8,
}

partial class Program
{
    static readonly ReadOnlyCollection<int> DirectionDeltaY = Array.AsReadOnly([-1, 0, 1, 0]);
    static readonly ReadOnlyCollection<int> DirectionDeltaX = Array.AsReadOnly([0, 1, 0, -1]);
    static readonly ReadOnlyCollection<Flags> DirectionFlag = Array.AsReadOnly([Flags.Up, Flags.Right, Flags.Down, Flags.Left]);
}