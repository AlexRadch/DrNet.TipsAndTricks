using BenchmarkDotNet.Attributes;

namespace ForLooping;

[MemoryDiagnoser(false)]
public class Benchmarks
{
    [Params(100, 10_000, 1_000_000)]
    public int Size { get; set; }

    private int[] array = Array.Empty<int>();
    private List<int> list = new ();
    public volatile int vItem;

    [GlobalSetup]
    public void GlobalSetup()
    {
        array = Generate(1_000).Take(Size).ToArray();
        list = new List<int> (array);
    }

    [Benchmark]
    public void ArrayFor()
    {
        for (var i = 0; i < array.Length; i++)
            vItem = array[i];
    }

    [Benchmark]
    public void ListFor()
    {
        for (var i = 0; i < list.Count; i++)
            vItem = list[i];
    }

    [Benchmark]
    public void SpanFor()
    {
        var span = new Span<int>(array);
        for (var i = 0; i < span.Length; i++)
            vItem = span[i];
    }

    [Benchmark]
    public void ArrayForReverse()
    {
        for (var i = array.Length - 1; i >= 0; i--)
            vItem = array[i];
    }

    [Benchmark]
    public void ListForReverse()
    {
        for (var i = list.Count - 1; i >= 0; i--)
            vItem = list[i];
    }

    [Benchmark]
    public void SpanForReverse()
    {
        var span = new Span<int>(array);
        for (var i = span.Length - 1; i >= 0; i--)
            vItem = span[i];
    }

    [Benchmark]
    public void ArrayForLocalCount()
    {
        for (int i = 0, lc = array.Length; i < lc; i++)
            vItem = array[i];
    }

    [Benchmark]
    public void ListForLocalCount()
    {
        for (int i = 0, lc = list.Count; i < lc; i++)
            vItem = list[i];
    }

    [Benchmark]
    public void SpanForLocalCount()
    {
        var span = new Span<int>(array);
        for (int i = 0, lc = span.Length; i < lc; i++)
            vItem = span[i];
    }

    public static IEnumerable<TResult> Generate<TGenerator, TResult>(TGenerator generator, Func<TGenerator, TResult> next)
    {
        while (true)
        {
            yield return next(generator);
        }
    }

    public static IEnumerable<int> Generate(int max) => Generate(new Random(), random => random.Next(max));
}
