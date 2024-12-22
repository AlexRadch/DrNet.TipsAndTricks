// --- Day 22: Monkey Market ---
//  https://adventofcode.com/2024/day/22

using System.Net.Sockets;

ProcessFile("input1.txt");
Console.WriteLine();


static void ProcessFile(string filePath)
{
    using var reader = File.OpenText(filePath);
    var secrets = ReadSecrets(reader).ToArray();

    Solve(123, 10);
    var secretsEnd = secrets.Select(secret => Solve(secret, 2000));

    var result = secretsEnd.Sum();
    Console.WriteLine(result);
}

static IEnumerable<int> ReadSecrets(TextReader reader) =>
    reader.ReadLines().Select(int.Parse);

static long Solve(long secret, int times)
{
    for (int i = 0; i < times; i++)
    {
        checked
        {
            secret ^= secret * 64;
            secret %= 16777216;
            secret ^= secret / 32 + (secret / 16 & 1);
            secret %= 16777216;
            secret ^= secret * 2024;
            secret %= 16777216;
        }
    }

    return secret;
}


static class Extensions
{
    public static IEnumerable<string> ReadLines(this TextReader reader)
    {
        while (reader.ReadLine() is string line)
            yield return line;
    }
}
