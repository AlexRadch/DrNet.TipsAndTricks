//  --- Day 11: Plutonian Pebbles ---
//  https://adventofcode.com/2024/day/11

using System.Numerics;

{
    IDictionary<BigInteger, long> stones = new Dictionary<BigInteger, long>(
        File.OpenText("input1.txt").ReadLine()!.Split()
            .Select(BigInteger.Parse)
            .GroupBy(stone => stone)
            .Select(stoneGroup => new KeyValuePair<BigInteger, long>(stoneGroup.Key, stoneGroup.Count()))
            );

    for (int i = 0; i < 75; i++)
        stones = Solve(stones);

    var result = stones.Select(pair => pair.Value).Sum();

    Console.WriteLine($"{result}");
}

{
    IDictionary<BigInteger, long> stones = new Dictionary<BigInteger, long>(
        File.OpenText("input2.txt").ReadLine()!.Split()
            .Select(BigInteger.Parse)
            .GroupBy(stone => stone)
            .Select(stoneGroup => new KeyValuePair<BigInteger, long>(stoneGroup.Key, stoneGroup.Count()))
            );

    for (int i = 0; i < 75; i++)
        stones = Solve(stones);

    var result = stones.Select(pair => pair.Value).Sum();

    Console.WriteLine($"{result}");
}

static IDictionary<BigInteger, long> Solve<TStones>(TStones stones) where TStones : IDictionary<BigInteger, long>
{
    var result = new Dictionary<BigInteger, long>();
    foreach ((var stone, var count) in stones)
    {
        if (stone == 0)
            Add(result, 1, count);
        else if (Len(stone) is int len && len % 2 == 0)
        {
            var power = BigInteger.Pow(10, len / 2);
            Add(result, stone / power, count);
            Add(result, stone % power, count);
        }
        else
            Add(result, stone * 2024, count);
    }
    return result;
}

static void Add<TStones>(TStones stones, BigInteger stone, long count) where TStones : IDictionary<BigInteger, long>
{
    if (stones.ContainsKey(stone))
        stones[stone] += count;
    else
        stones.Add(stone, count);
}

static int Len(BigInteger n)
{
    if (n < 10)
        return 1;

    int result = (int)(n.GetBitLength() * Math.Log10(2));
    var reference = BigInteger.Pow(10, result);

    if (n >= reference)
        result++;

    return result;
}
