//  --- Day 11: Plutonian Pebbles ---
//  https://adventofcode.com/2024/day/11

using System.Numerics;

{
    var stones = new LinkedList<BigInteger>(
        File.OpenText("input1.txt").ReadLine()!.Split()
            .Select(BigInteger.Parse));

    for (int i = 0; i < 25; i++)
        stones = Solve(stones);

    var result = stones.Count;

    Console.WriteLine(result);
}

{
    var stones = new LinkedList<BigInteger>(
        File.OpenText("input2.txt").ReadLine()!.Split()
            .Select(BigInteger.Parse));

    for (int i = 0; i < 25; i++)
        stones = Solve(stones);

    var result = stones.Count;

    Console.WriteLine(result);
}

static TStones Solve<TStones>(TStones stones) where TStones : LinkedList<BigInteger>
{
    var node = stones.Last;
    while (node is not null)
    {
        if (node.Value == 0)
            node.Value = 1;
        else if (Len(node.Value) is int len && len % 2 == 0)
        {
            var power = BigInteger.Pow(10, len / 2);
            stones.AddAfter(node, node.Value % power);
            node.Value /= power;
        }
        else
            node.Value *= 2024;

        node = node.Previous;
    }
    return stones;
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
