using System.Numerics;

var ints = Generate(new Random(), random => random.Next(100)).Take(10).ToArray();

var sum1 = Sum1(ints);
var sum2 = Sum(ints);
var avr = Average(ints);
var dev = StandardDeviation(ints);

Console.WriteLine($"""
    Sum1 = {sum1}
    Sum2 = {sum2}
    Avr = {avr}
    dev = {dev}

    """);

var longs = Generate(new Random(), random => random.NextInt64(100)).Take(10).ToArray();

//sum1 = Sum1(longs);
sum2 = Sum(longs);
avr = Average(longs);
dev = StandardDeviation(longs);

Console.WriteLine($"""
    {""//Sum1 = {sum1}
   }Sum2 = {sum2}
    Avr = {avr}
    dev = {dev}

    """);

static double Sum1(IEnumerable<int> values)
{
    long result = 0;

    foreach (var value in values)
    {
        result += value;
    }

    return result;
}

static double Sum<T>(IEnumerable<T> values)
    where T : INumberBase<T>
{
    double result = 0d;

    foreach (var value in values)
    {
        checked { result += double.CreateChecked(value); }
    }

    return result;
}

static double Average<T>(IEnumerable<T> values)
    where T : INumberBase<T>
{
    double sum = Sum(values);
    return sum / double.CreateChecked(values.Count());
}

static double StandardDeviation<T>(IEnumerable<T> values)
    where T : INumberBase<T>
{
    double standardDeviation = 0d;

    if (values.Any())
    {
        double average = Average(values);
        double sum = Sum(values.Select((value) => {
            var deviation = double.CreateChecked(value) - average;
            return deviation * deviation;
        }));
        standardDeviation = double.Sqrt(sum / double.CreateSaturating(values.Count() - 1));
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