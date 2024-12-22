// --- Day 22: Monkey Market ---
//  https://adventofcode.com/2024/day/22

ProcessFile("input1.txt");
Console.WriteLine();

ProcessFile("input2.txt");
Console.WriteLine();

static void ProcessFile(string filePath)
{
    using var reader = File.OpenText(filePath);
    var secrets = ReadSecrets(reader).ToArray();

    var secretsEnd = secrets.Select(secret => Solve(secret, 2000));

    var result = secretsEnd.Select(secret => (long)secret).Sum();
    Console.WriteLine(result);
}

static IEnumerable<int> ReadSecrets(TextReader reader) =>
    reader.ReadLines().Select(int.Parse);

static int Solve(int secret, int times)
{
    for (int i = 0; i < times; i++)
        secret = TransformSecret(secret);

    return secret;
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

static class Extensions
{
    public static IEnumerable<string> ReadLines(this TextReader reader)
    {
        while (reader.ReadLine() is string line)
            yield return line;
    }
}
