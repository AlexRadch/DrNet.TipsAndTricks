//  --- Day 13: Claw Contraption ---
//  https://adventofcode.com/2024/day/13

using System.Text.RegularExpressions;

using Machine = ((int DX, int DY) ButtonA, (int DX, int DY) ButtonB, (int X, int Y) Prize);

{
    var machines = File.ReadLines("input1.txt")
        .Index().GroupBy(item => item.Index / 4)
        .Select(group => ReadMachine(group.Select(item => item.Item)))
        .ToArray().AsReadOnly();

    var costs = machines.Select(Solve);

    var result = costs.Sum();

    Console.WriteLine($"{result}");
}

{
    var machines = File.ReadLines("input2.txt")
        .Index().GroupBy(item => item.Index / 4)
        .Select(group => ReadMachine(group.Select(item => item.Item)))
        .ToArray().AsReadOnly();

    var costs = machines.Select(Solve);

    var result = costs.Sum();

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
        (int.Parse(matches3[0].Value), int.Parse(matches3[1].Value)));
}

static int Solve(Machine machine)
{
    // machine.ButtonA.DX * a + machine.ButtonB.DX * b = machine.Prize.X
    // machine.ButtonA.DY * a + machine.ButtonB.DY * b = machine.Prize.Y
    //
    // (machine.ButtonA.DX * machine.ButtonA.DY - machine.ButtonA.DX * machine.ButtonA.DY) * a +
    // (machine.ButtonB.DX * machine.ButtonA.DY - machine.ButtonA.DX * machine.ButtonB.DY) * b =
    // machine.Prize.X * machine.ButtonA.DY - machine.ButtonA.DX * machine.Prize.Y
    //
    // b = (machine.Prize.X * machine.ButtonA.DY - machine.ButtonA.DX * machine.Prize.Y) / (machine.ButtonB.DX * machine.ButtonA.DY - machine.ButtonA.DX * machine.ButtonB.DY)

    var k1 = machine.ButtonB.DX * machine.ButtonA.DY - machine.ButtonA.DX * machine.ButtonB.DY;
    if (k1 == 0)
        return 0;
    var k2 = machine.Prize.X * machine.ButtonA.DY - machine.ButtonA.DX * machine.Prize.Y;

    if (k2 % k1 != 0)
        return 0;

    var b = k2 / k1;

    // machine.ButtonA.DX * a + machine.ButtonB.DX * b = machine.Prize.X
    // machine.ButtonA.DX * a = machine.Prize.X - machine.ButtonB.DX * b
    // a = (machine.Prize.X - machine.ButtonB.DX * b) / machine.ButtonA.DX

    var a = (machine.Prize.X - machine.ButtonB.DX * b) / machine.ButtonA.DX;

    return a * 3 + b;
}

partial class Program
{
    [GeneratedRegex(@"\d+")]
    private static partial Regex IntRegex();
}