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

    var circuit = new Circuit();
    foreach (var element in ReadWares(reader, circuit))
        circuit[element.Output] = element;

    foreach (var element in ReadGates(reader, circuit))
        circuit[element.Output] = element;

    var result = Solve(circuit);
    Console.WriteLine(result);
}


static IEnumerable<Ware> ReadWares(TextReader reader, Circuit circuit) =>
    reader.ReadLines().TakeWhile(line => line.Length > 0)
        .Select(line => line.Split(':', StringSplitOptions.TrimEntries) is { } parts
            ? new Ware(circuit, parts[0]) { Value = parts[1] == "1" }
            : throw new InvalidDataException($"ReadWares {line}")
        );

static IEnumerable<Gate> ReadGates(TextReader reader, Circuit circuit) =>
    reader.ReadLines()
        .Select(line => line.Split("->", StringSplitOptions.TrimEntries) is { } parts1
            ? parts1[0].Split(' ') is { } parts2
                ? new Gate(circuit, parts2[0], MapOperation(parts2[1]), parts2[2], parts1[1])
                : throw new InvalidDataException($"ReadGates2 {line} {parts1[0]}")
            : throw new InvalidDataException($"ReadGates1 {line}")
        );

static long Solve(Circuit circuit)
{
    var values = circuit.Keys.Where(key => key[0] == 'z').OrderDescending()
        .Select(key => circuit[key].Evaluate()!.Value ? 1 : 0).ToList();

    var result = values.Aggregate(0L, (acc, value) => (acc << 1) | (long)value);
    return result;
}

static Operation MapOperation(string operation) =>
    operation switch
    {
        "AND" => Operation.And,
        "XOR" => Operation.Xor,
        "OR" => Operation.Or,
        _ => throw new InvalidDataException($"MapOperation {operation}")
    };

class Circuit : Dictionary<string, Element> { }

abstract record Element(Circuit Circuit, string Output)
{
    public abstract bool? Evaluate();
}


record Ware(Circuit Circuit, string Output) : Element(Circuit, Output)
{
    public bool Value { get; set; }

    public override bool? Evaluate() => Value;
}

record Gate(Circuit Circuit, string In1, Operation Operation, string In2, string Output) : Element(Circuit, Output)
{
    private bool? _value;
    private bool _evaluation;
    public override bool? Evaluate()
    {
        if (_value.HasValue)
            return _value.Value;
        if (_evaluation) // Circuit recursion
            return default;

        _evaluation = true;
        try
        {
            var value1 = Circuit[In1].Evaluate();
            if (!value1.HasValue)
                return default;


            var value2 = Circuit[In2].Evaluate();
            if (!value2.HasValue)
                return default;

            return _value ??= Evaluate(value1.Value, Operation, value2.Value);
        }
        finally
        {
            _evaluation = false;
        }
    }

    public void Reset() => _value = null;

    public static bool Evaluate(bool in1, Operation operation, bool in2) => operation switch
    {
        Operation.And => in1 && in2,
        Operation.Xor => in1 ^ in2,
        Operation.Or => in1 || in2,
        _ => throw new InvalidDataException($"Gate.Evaluate invalid operation {operation}")
    };
}

enum Operation
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
