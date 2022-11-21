using System.Numerics;

var ints = Generate(new Random(), random => random.Next(100)).Take(5).ToArray();

var sum1 = Sum1(ints); // Compiler error. See https://github.com/dotnet/roslyn/issues/65534
var sum2 = Sum<int, double>(ints);
var avr = Average<int, double>(ints);
var dev = StandardDeviation<int, double>(ints);

Console.WriteLine($"""
    Sum1 = {sum1}
    Sum2 = {sum2}
    Avr = {avr}
    dev = {dev}

    """);

var longs = Generate(new Random(), random => random.NextInt64(100)).Take(5).ToArray();

//sum1 = Sum1(longs);
sum2 = Sum<long, double>(longs);
avr = Average<long, double>(longs);
dev = StandardDeviation<long, double>(longs);

Console.WriteLine($"""
    {""//Sum1 = {sum1}
   }Sum2 = {sum2}
    Avr = {avr}
    dev = {dev}{""
   } // the same line

    """);

static double Sum1(IEnumerable<int> values) // Compiler error. See https://github.com/dotnet/roslyn/issues/65534
{
    long result = 0;

    foreach (var value in values)
    {
        result += value;
    }

    return result;
}

static TResult Sum<T, TResult>(IEnumerable<T> values)
    where T : INumberBase<T>
    where TResult : INumberBase<TResult>
{
    TResult result = TResult.Zero;

    foreach (var value in values)
    {
        checked { result += TResult.CreateChecked(value); }
    }

    return result;
}

static TResult Average<T, TResult>(IEnumerable<T> values)
    where T : INumberBase<T>
    where TResult : INumberBase<TResult>
{
    TResult sum = Sum<T, TResult>(values);
    return sum / TResult.CreateChecked(values.Count());
}

static TResult StandardDeviation<T, TResult>(IEnumerable<T> values)
    where T : INumberBase<T>
    where TResult : IRootFunctions<TResult>
{
    TResult standardDeviation = TResult.Zero;

    if (values.Any())
    {
        TResult average = Average<T, TResult>(values);
        TResult sum = Sum<TResult, TResult>(values.Select((value) => {
            var deviation = TResult.CreateChecked(value) - average;
            return deviation * deviation;
        }));
        standardDeviation = TResult.Sqrt(sum / TResult.CreateSaturating(values.Count() - 1));
    }

    return standardDeviation;
}

static IEnumerable<TResult> Generate<TGenerator, TResult>(TGenerator generator, Func<TGenerator, TResult> next)
{
    while (true)
    {
        yield return next(generator);
    }
}