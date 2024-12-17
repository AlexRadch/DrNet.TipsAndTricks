//  --- Day 9: Disk Fragmenter ---
//  https://adventofcode.com/2024/day/9

{
    var reader = File.OpenText("input1.txt");
    var map = reader.ReadLine()!.Select(c => c - '0').ToArray();

    var result = Solve(map);
    Console.WriteLine(result);
}

{
    var reader = File.OpenText("input2.txt");
    var map = reader.ReadLine()!.Select(c => c - '0').ToArray();

    var result = Solve(map);
    Console.WriteLine(result);
}

static long Solve<TMap>(TMap map) where TMap : IReadOnlyList<int>
{
    var start = 0;
    var end = (map.Count - 1) / 2;
    var endLength = map[end * 2];
    int offset = 0;

    long result = 0;
    while (start <= end)
        result += SolveStartEnd(map, ref start, ref end, ref endLength, ref offset);

    return result;
}

static long SolveStartEnd<TMap>(TMap map, ref int start, ref int end, ref int endLength, ref int offset)
    where TMap : IReadOnlyList<int>
{
    if (start == end)
    {
        long result = SumArithmeticProgression(offset, endLength) * end;
        offset += endLength;
        end--;
        endLength = 0;
        return result;
    }

    long sum = SumArithmeticProgression(offset, map[start * 2]) * start;
    offset += map[start * 2];

    var len = map[start * 2 + 1];
    start++;
    while (len > 0 && start <= end)
    {
        var minLen = Math.Min(len, endLength);
        sum += SumArithmeticProgression(offset, minLen) * end;
        offset += minLen;
        len -= minLen;
        endLength -= minLen;

        if (endLength <= 0)
        {
            end--;
            endLength = map[end * 2];
        }
    }

    return sum;
}

static long SumArithmeticProgression(int start, int count)
    => ((long)(start + start + count - 1)) * count / 2;
