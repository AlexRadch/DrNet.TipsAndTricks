//  --- Day 9: Disk Fragmenter ---
//  https://adventofcode.com/2024/day/9

using UsedSpace = (System.Collections.Generic.List<int> Files, int FreeLen);

{
    var reader = File.OpenText("input1.txt");
    var map = reader.ReadLine()!.Select(c => c - '0').ToArray();

    long result = Solve(map);
    Console.WriteLine($"{result}");
}

{
    var reader = File.OpenText("input2.txt");
    var map = reader.ReadLine()!.Select(c => c - '0').ToArray();

    long result = Solve(map);
    Console.WriteLine($"{result}");
}

static long Solve<TMap>(TMap map) where TMap : IReadOnlyList<int>
{
    var sizeToFreeSpaces = new SortedSet<int>[9];
    for (var i = 0; i < sizeToFreeSpaces.Length; i++)
        sizeToFreeSpaces[i] = [];

    var usedSpaces = map.Where((_, i) => i % 2 == 0)
        .Select((len, file) => new UsedSpace([file], 0))
        .ToArray();

    foreach ((var space, var len) in map.Where((_, i) => i % 2 != 0).Index())
    {
        if (len > 0)
        {
            sizeToFreeSpaces[len - 1].Add(space);
            usedSpaces[space].FreeLen = len;
        }
    }

    for (var file = usedSpaces.Length - 1; file > 0; file--)
    {
        var len = map[file * 2];

        ref var usedSpace = ref usedSpaces[file];
        if (usedSpace.FreeLen > 0 && usedSpace.FreeLen < 10)
            sizeToFreeSpaces[usedSpace.FreeLen - 1].Remove(file);

        var space = GetFreeSpace(sizeToFreeSpaces, len);
        if (space >= 0 && space < file)
        {
            usedSpace.Files.RemoveAt(0);

            ref var prevSpace = ref usedSpaces[file - 1];
            if (prevSpace.FreeLen > 0)
                sizeToFreeSpaces[prevSpace.FreeLen - 1].Remove(file - 1);
            prevSpace.FreeLen += len;

            usedSpace = ref usedSpaces[space];
            usedSpace.Files.Add(file);
            if (usedSpace.FreeLen < 10)
                sizeToFreeSpaces[usedSpace.FreeLen - 1].Remove(space);
            usedSpace.FreeLen -= len;
            if (usedSpace.FreeLen > 0)
                sizeToFreeSpaces[usedSpace.FreeLen - 1].Add(space);
        }
    }

    {
        long result = 0;
        var offset = 0;
        foreach (var (Files, FreeLen) in usedSpaces)
        {
            foreach (var file in Files)
            {
                var len = map[file * 2];
                result += SumArithmeticProgression(offset, len) * file;
                offset += len;
            }

            offset += FreeLen;
        }

        return result;
    }
}

static int GetFreeSpace(SortedSet<int>[] sizeToFreeSpaces, int len)
    => sizeToFreeSpaces.Skip(len - 1).Select(set => set.FirstOrDefault(int.MaxValue)).Min();

static long SumArithmeticProgression(int start, int count)
    => ((long)(start + start + count - 1)) * count / 2;
