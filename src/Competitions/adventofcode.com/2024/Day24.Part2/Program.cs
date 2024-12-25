// --- Day 24: Crossed Wires ---
//  https://adventofcode.com/2024/day/24

ProcessFile("input1.txt");
Console.WriteLine();

ProcessFile("input2.txt");
Console.WriteLine();

ProcessFile("input3.txt");
Console.WriteLine();

static void ProcessFile(string filePath)
{
    using var reader = File.OpenText(filePath);
    var wares = ReadWares(reader).ToList();
    var gates = ReadGates(reader).ToList();

    var result = Solve(wares, gates);
    Console.WriteLine(result);
}


static IEnumerable<Ware> ReadWares(TextReader reader) =>
    reader.ReadLines().TakeWhile(line => line.Length > 0)
        .Select(line => line.Split(':', StringSplitOptions.TrimEntries) is { } parts
            ? new Ware(parts[0], parts[1] == "1")
            : throw new InvalidDataException($"ReadWares {line}")
        );

static IEnumerable<Gate2> ReadGates(TextReader reader) =>
    reader.ReadLines()
        .Select(line => line.Split("->", StringSplitOptions.TrimEntries) is { } parts1
            ? parts1[0].Split(' ') is { } parts2
                ? new Gate2(parts2[0], MapOperation2(parts2[1]), parts2[2], parts1[1])
                : throw new InvalidDataException($"ReadGates2 {line} {parts1[0]}")
            : throw new InvalidDataException($"ReadGates1 {line}")
        );

static long Solve<TWares, TGates>(TWares wares, TGates gates)
    where TWares : IEnumerable<Ware>
    where TGates : IEnumerable<Gate2>
{
    var circuit = new Dictionary<string, CircuitElement>();

    foreach (var ware in wares)
        circuit[ware.Output] = new CircuitElement(circuit, ware);

    foreach (var gate in gates)
        circuit[gate.Output] = new CircuitElement(circuit, gate);

    var values = circuit.Keys.Where(key => key[0] == 'z').OrderDescending()
        .Select(key => circuit[key].Value ? 1 : 0).ToList();

    var result = values.Aggregate(0L, (acc, value) => (acc << 1) | (long)value);
    return result;
}

static Operation2 MapOperation2(string operation) =>
    operation switch
    {
        "AND" => Operation2.And,
        "XOR" => Operation2.Xor,
        "OR" => Operation2.Or,
        _ => throw new InvalidDataException($"MapOperation {operation}")
    };

record CircuitElement(Dictionary<string, CircuitElement> Circuit, Element Element)
{
    private bool? _value;
    public bool Value
    {
        get
        {
            _value ??= Element switch
            {
                Ware ware => ware.Value,
                Gate2 gate => gate.Operation switch
                {
                    Operation2.And => Circuit[gate.In1].Value && Circuit[gate.In2].Value,
                    Operation2.Xor => Circuit[gate.In1].Value ^ Circuit[gate.In2].Value,
                    Operation2.Or => Circuit[gate.In1].Value || Circuit[gate.In2].Value,
                    _ => throw new InvalidDataException($"CircuitElement {gate.Operation}")
                },
                _ => throw new InvalidDataException($"CircuitElement {Element}")
            };
            return _value.Value;
        }
    }
}

abstract record Element(string Output);

record Ware : Element
{
    public bool Value { get; init; }

    public Ware(string output, bool value) : base(output)
    {
        Value = value;
    }
}

record Gate2 : Element
{
    public string In1 { get; init; }
    public Operation2 Operation { get; init; }
    public string In2 { get; init; }

    public Gate2(string in1, Operation2 operation, string in2, string output) : base(output)
    {
        In1 = in1;
        Operation = operation;
        In2 = in2;
    }
}

enum Operation2
{
    And,
    Xor,
    Or,
}

static class Extensions
{
    public static IEnumerable<string> ReadLines(this TextReader reader)
    {
        while (reader.ReadLine() is string line)
            yield return line;
    }
}
