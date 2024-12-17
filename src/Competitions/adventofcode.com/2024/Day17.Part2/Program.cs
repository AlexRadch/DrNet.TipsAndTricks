// --- Day 17: Chronospatial Computer ---
//  https://adventofcode.com/2024/day/17

using System.Numerics;
using System.Text.RegularExpressions;
using Registers = (System.Numerics.BigInteger A, System.Numerics.BigInteger B, System.Numerics.BigInteger C);

for (int i = 3; i <= 3; i++)
{
    using var reader = File.OpenText($"input{i}.txt");

    var registers = new Registers(ReadRegister(reader), ReadRegister(reader), ReadRegister(reader));
    reader.ReadLine();
    var program = ReadProgram(reader);

    var result = Solve(registers, program);

    Console.WriteLine(result);
    Console.WriteLine();
}

static BigInteger ReadRegister(TextReader reader) =>
    BigInteger.Parse(IntRegex().Match(reader.ReadLine()!).Value);

static IReadOnlyList<int> ReadProgram(TextReader reader) =>
    IntRegex().Matches(reader.ReadLine()!).Select(match => int.Parse(match.Value)).ToArray();

static BigInteger Solve(Registers registers, IReadOnlyList<int> program)
{
    BigInteger result = 0;
    while (true)
    {
        for (int i = 0; i <= 8; i++)
        {
            if (i == 8)
                return -1;

            var output = Run((result, registers.B, registers.C), program);

            var count = output.Count();
            if (count > program.Count)
                return -1;

            if (program[^count] == output.First())
            {
                if (count == program.Count)
                    return result;
                break;
            }
            result++;
        }
        result *= 8;
    }
}

static IEnumerable<int> Run(Registers registers, IReadOnlyList<int> program)
{
    var result = new List<int>();
    var ip = 0;
    while (ip >= 0 && ip < program.Count - 1)
    {
        switch (program[ip])
        {
            case 0: // adv
                registers.A /= BigInteger.Pow(2, (int)ReadCombo(program[ip + 1]));
                break;
            case 1: // bxl
                registers.B ^= program[ip + 1];
                break;
            case 2: // bst
                registers.B = ReadCombo(program[ip + 1]) & 7;
                break;
            case 3: // jnz
                if (registers.A != 0)
                {
                    ip = program[ip + 1];
                    continue;
                }
                break;
            case 4: // bxc
                registers.B ^= registers.C;
                break;
            case 5: // out
                result.Add((int)(ReadCombo(program[ip + 1]) & 7));
                break;
            case 6: // bdv
                registers.B = registers.A / BigInteger.Pow(2, (int)ReadCombo(program[ip + 1]));
                break;
            case 7: // cdv
                registers.C = registers.A / BigInteger.Pow(2, (int)ReadCombo(program[ip + 1]));
                break;
            default:
                ip = -1;
                continue;
        }
        ip += 2;
    }

    return result;

    BigInteger ReadCombo(int operand) => operand switch
    {
        >= 0 and <= 3 => operand,
        4 => registers.A,
        5 => registers.B,
        6 => registers.C,
        _ => throw new InvalidDataException(),
    };
}

static class Extensions
{
    public static IEnumerable<string> ReadLines(this TextReader reader)
    {
        while (reader.ReadLine() is string line)
            yield return line;
    }
}

partial class Program
{
    [GeneratedRegex(@"\d+")]
    private static partial Regex IntRegex();
}