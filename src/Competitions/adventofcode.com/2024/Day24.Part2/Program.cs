// --- Day 24: Crossed Wires ---
//  https://adventofcode.com/2024/day/24

//ProcessFile("input1.txt");
//Console.WriteLine();

//ProcessFile("input2.txt");
//Console.WriteLine();

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


    {
        //SwapWares(circuit, "z05", "dkr");
        //SwapWares(circuit, "z15", "htp");
        //SwapWares(circuit, "z20", "hhh");
        //SwapWares(circuit, "rhv", "ggk");
        IEnumerable<string> list = ["z05", "dkr", "z15", "htp", "z20", "hhh", "rhv", "ggk"];
        Console.WriteLine(string.Join(',', list.Order()));
    }

    {
        using var writer = new StreamWriter(Path.ChangeExtension(filePath, ".md"));
        WriteGraph(writer, circuit);
    }


    var result = Solve(circuit);
    var ordered = result.Order();
    Console.WriteLine(string.Join(',', ordered));
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

static void WriteGraph(TextWriter writer, Circuit circuit)
{
    writer.WriteLine("graph LR");

    foreach (var element in circuit.Values)
    {
        if (element is Ware ware)
        {
            writer.WriteLine($"    {ware.Output}");
        }
        else if (element is Gate gate)
        {
            writer.Write($"{gate.Output}{{{{{gate.Operation.ToString().ToUpper()}");
            if (gate.Output[0] != 'z')
                writer.Write($" {gate.Output}");
            writer.WriteLine($"}}}}");

            writer.WriteLine($"    {gate.In1} --> {gate.Output}");
            writer.WriteLine($"    {gate.In2} --> {gate.Output}");
            if (gate.Output[0] == 'z')
            {
                writer.WriteLine($"    {gate.Output}_out[\"{gate.Output}\"]");
                writer.WriteLine($"    {gate.Output} --> {gate.Output}_out");
            }
        }
    }
}

static IEnumerable<string> Solve(Circuit circuit)
{
    var stack = new Stack<(int Index, IEnumerator<(string Out1, string Out2)> Swaps)>();

    var i = 0;
    while (i <= 45)
    {
        if (TestBit(circuit, i))
        {
            i++;
            continue;
        }

        var fixEnum = FixBit(circuit, i).GetEnumerator();
    Loop:
        if (fixEnum.MoveNext())
        {
            stack.Push((i, fixEnum));
            SwapWares(circuit, fixEnum.Current.Out1, fixEnum.Current.Out2);

            i++;
            continue;
        }

        if (stack.Count > 0)
        {
            var item = stack.Pop();
            var (Out1, Out2) = item.Swaps.Current;
            SwapWares(circuit, Out1, Out2);

            fixEnum = item.Swaps;
            i = item.Index;
            goto Loop;
        }

        break;
    }

    return stack.Reverse()
        .SelectMany(item => new string[] { item.Swaps.Current.Out1, item.Swaps.Current.Out2 });
}

static void SwapWares(Circuit circuit, string out1, string out2)
{
    var ware1 = circuit[out1];
    ware1.Output = out2;

    var ware2 = circuit[out2];
    ware2.Output = out1;

    circuit[out1] = ware2;
    circuit[out2] = ware1;
}

static bool TestBit(Circuit circuit, int i)
{
    ReadOnlySpan<bool> falseTrue = [false, true];

    foreach (var carry in falseTrue)
    {
        foreach (var b in falseTrue)
        {
            foreach (var a in falseTrue)
            {
                Reset(circuit);

                bool c = false;
                bool y = false;
                bool x = false;

                if (i > 0)
                    ((Ware)circuit[$"x{i - 1:00}"]).Value = ((Ware)circuit[$"y{i - 1:00}"]).Value = c = carry;

                if (i < 45)
                {
                    ((Ware)circuit[$"x{i:00}"]).Value = x = a;
                    ((Ware)circuit[$"y{i:00}"]).Value = y = b;
                }

                var z = ((Gate)circuit[$"z{i:00}"]).Evaluate();
                if (z is bool && z == (x ^ y ^ c))
                    continue;
                return false;
            }
        }
    }
    return true;
}

static IEnumerable<(string Out1, string Out2)> FixBit(Circuit circuit, int i)
{
    var correctGates = new HashSet<string>(GatesForBits(circuit, 0, i - 1).Select(gate => gate.Output));

    var swapGates = GatesForBits(circuit, 0, 45)
        .Where(gate => !correctGates.Contains(gate.Output))
        .Select(gate => gate.Output)
        .Reverse()
        .ToList();

    var badGates = GatesForBits(circuit, i, i)
        .Where(gate => !correctGates.Contains(gate.Output))
        .Select(gate => gate.Output)
        .ToList();

    foreach (var badGate in badGates)
    {
        foreach (var swapGate in swapGates)
        {
            var found = false;
            SwapWares(circuit, badGate, swapGate);
            try
            {
                found = TestBit(circuit, i);
            }
            finally
            {
                SwapWares(circuit, badGate, swapGate);
            }
            if (found)
                yield return (badGate, swapGate);
        }
    }
}

static IEnumerable<Gate> GatesForBits(Circuit circuit, int from, int to)
{
    var set = new HashSet<string>();
    var queue = new Queue<string>();
    for (int i = to; i >= from; i--)
    {
        var gate = (Gate)circuit[$"z{i:00}"];
        if (!set.Add(gate.Output))
            continue;

        yield return gate;

        queue.Enqueue(gate.Output);
        while (queue.Count > 0)
        {
            gate = (Gate)circuit[queue.Dequeue()];

            var in1 = circuit[gate.In1];
            if (in1 is Gate gate1)
            {
                if (set.Add(gate.In1))
                {
                    queue.Enqueue(gate.In1);
                    yield return gate1;
                }
            }

            var in2 = circuit[gate.In2];
            if (in2 is Gate gate2)
            {
                if (set.Add(gate.In2))
                {
                    queue.Enqueue(gate.In2);
                    yield return gate2;
                }
            }
        }
    }
}


static void Reset(Circuit circuit)
{
    foreach (var element in circuit.Values)
        element.Reset();
}

static Operation MapOperation(string operation) =>
    operation switch
    {
        "AND" => Operation.And,
        "XOR" => Operation.Xor,
        "OR" => Operation.Or,
        _ => throw new InvalidDataException($"MapOperation {operation}")
    };

class Circuit: Dictionary<string, Element> { }

abstract record Element(Circuit Circuit, string Output)
{
    public string Output { get; set; } = Output;
    public abstract void Reset();
    public abstract bool? Evaluate();
}
 

record Ware(Circuit Circuit, string Output) : Element(Circuit, Output)
{
    public bool Value { get; set; }
    public override void Reset() => Value = false;
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

    public override void Reset() => _value = null;

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
