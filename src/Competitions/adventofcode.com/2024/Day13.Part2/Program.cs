//  --- Day 13: Claw Contraption ---
//  https://adventofcode.com/2024/day/13

using System.Text.RegularExpressions;

using Machine = ((int DX, int DY) ButtonA, (int DX, int DY) ButtonB, (long X, long Y) Prize);

{
    var machines = File.ReadLines("input1.txt")
        .Index().GroupBy(item => item.Index / 4)
        .Select(group => ReadMachine(group.Select(item => item.Item)))
        .ToArray().AsReadOnly();

    var costs = machines.Select(Solve);

    var result = costs.Sum();

    Console.WriteLine($"875318608908");
    Console.WriteLine($"{result}");
}

{
    var machines = File.ReadLines("input2.txt")
        .Index().GroupBy(item => item.Index / 4)
        .Select(group => ReadMachine(group.Select(item => item.Item)))
        .ToArray().AsReadOnly();

    var costs = machines.Select(Solve);

    var result = costs.Sum();

    Console.WriteLine($"107487112929999");
    Console.WriteLine($"{result}");
}

static Machine ReadMachine(IEnumerable<string> lines)
{
    var matches1 = IntRegex().Matches(lines.ElementAt(0));
    var matches2 = IntRegex().Matches(lines.ElementAt(1));
    var matches3 = IntRegex().Matches(lines.ElementAt(2));

    return new Machine(
        (int.Parse(matches1[0].Value), int.Parse(matches1[1].Value)),
        (int.Parse(matches2[0].Value), int.Parse(matches2[1].Value)),
        (int.Parse(matches3[0].Value) + 10_000_000_000_000, int.Parse(matches3[1].Value) + 10_000_000_000_000));
}

static long Solve(Machine machine)
{
    checked
    {
        // machine.ButtonA.DX * a + machine.ButtonB.DX * b = machine.Prize.X
        // machine.ButtonA.DY * a + machine.ButtonB.DY * b = machine.Prize.Y
        //
        // (machine.ButtonA.DX * machine.ButtonA.DY - machine.ButtonA.DX * machine.ButtonA.DY) * a +
        // (machine.ButtonB.DX * machine.ButtonA.DY - machine.ButtonA.DX * machine.ButtonB.DY) * b =
        // machine.Prize.X * machine.ButtonA.DY - machine.ButtonA.DX * machine.Prize.Y
        //
        // b = (machine.Prize.X * machine.ButtonA.DY - machine.ButtonA.DX * machine.Prize.Y) / (machine.ButtonB.DX * machine.ButtonA.DY - machine.ButtonA.DX * machine.ButtonB.DY)

        var k2 = machine.ButtonB.DX * machine.ButtonA.DY - machine.ButtonA.DX * machine.ButtonB.DY;
        if (k2 == 0)
            throw new NotSupportedException();

        var k1 = machine.Prize.X * machine.ButtonA.DY - machine.ButtonA.DX * machine.Prize.Y;

        var b = Math.DivRem(k1, k2, out var rem);
        if (rem != 0)
            return 0;
        if (b < 0)
            throw new NotSupportedException();

        // machine.ButtonA.DX * a + machine.ButtonB.DX * b = machine.Prize.X
        // machine.ButtonA.DX * a = machine.Prize.X - machine.ButtonB.DX * b
        // a = (machine.Prize.X - machine.ButtonB.DX * b) / machine.ButtonA.DX

        var a = Math.DivRem(machine.Prize.X - machine.ButtonB.DX * b, machine.ButtonA.DX, out rem);
        if (rem != 0)
            return 0;
        if (a < 0)
            throw new NotSupportedException();

        return a * 3 + b;
    }
}

partial class Program
{
    [GeneratedRegex(@"\d+")]
    private static partial Regex IntRegex();
}