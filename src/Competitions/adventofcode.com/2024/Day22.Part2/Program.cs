// --- Day 22: Monkey Market ---
//  https://adventofcode.com/2024/day/22

using System.Collections;

ProcessFile("input1.txt");
Console.WriteLine();

ProcessFile("input2.txt");
Console.WriteLine();

static void ProcessFile(string filePath)
{
    using var reader = File.OpenText(filePath);
    var secrets = ReadSecrets(reader).ToList();

    var result = Solve(secrets, 2000);
    Console.WriteLine(result);
}

static IEnumerable<int> ReadSecrets(TextReader reader) =>
    reader.ReadLines().Select(int.Parse);

static int Solve<TSecrets>(TSecrets secrets, int times) where TSecrets: IEnumerable<int>
{

    var predictBananas = new int[MaxChanges];


    foreach (var secret in secrets)
        PredictBananas(secret, times, predictBananas);

    var maxPair = predictBananas.Index().MaxBy(pair => pair.Item);
    return maxPair.Item;
}

static void PredictBananas<TPredict>(int secret, int times, TPredict predictBananas) where TPredict: IList<int>
{
    var changesExist = new BitArray(MaxChanges);

    var changesIndex = 0;
    for (var i = 0; i < times; i++)
    {
        var nextSecret = TransformSecret(secret);

        var bananas = nextSecret % 10;
        changesIndex = (bananas - (secret % 10) - ChangesMin) * (MaxChanges / ChangesRadix) +
            changesIndex / ChangesRadix;

        secret = nextSecret;

        if (i < 3)
            continue;

        if (changesExist[changesIndex])
            continue;

        predictBananas[changesIndex] += bananas;
        changesExist[changesIndex] = true;
    }
}

static int TransformSecret(int secret)
{
    secret ^= secret << 6;
    secret &= 16777216 - 1;

    secret ^= secret >> 5;
    secret &= 16777216 - 1;

    secret ^= secret << 11;
    secret &= 16777216 - 1;

    return secret;
}

partial class Program
{
    const int ChangesMin = -9;
    //const int ChangesMax = 9;
    const int ChangesRadix = 20;    // ChangesMax - ChangesMin + 1;
    const int MaxChanges = ChangesRadix * ChangesRadix * ChangesRadix * ChangesRadix;

    internal static int[] ChangesFromIndex(int changesIndex) => [
        (changesIndex % ChangesRadix) + ChangesMin,
        (changesIndex / ChangesRadix % ChangesRadix) + ChangesMin,
        (changesIndex / (ChangesRadix * ChangesRadix) % ChangesRadix) + ChangesMin,
        (changesIndex / (ChangesRadix * ChangesRadix * ChangesRadix) % ChangesRadix) + ChangesMin];
}

static class Extensions
{
    public static IEnumerable<string> ReadLines(this TextReader reader)
    {
        while (reader.ReadLine() is string line)
            yield return line;
    }
}
