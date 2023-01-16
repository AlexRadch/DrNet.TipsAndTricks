using System.Collections.Frozen;
using System.Collections.Immutable;

using BenchmarkDotNet.Attributes;
using Bogus;

namespace Frozen_Collections;

[MemoryDiagnoser(false)]
public class Dictionaries_Construction
{
    [Params(10, 100, 1_000, 10_000, 100_000, 1_000_000)]
    public int Size { get; set; }
    public int MaxKey => checked(Size * 2); // exclusive
    public int MaxValue => int.MaxValue; // exclusive

    public int Seed = 0x2626ad8a;

    private IDictionary<int, int> items = default!;

    public volatile Dictionary<int, int> dictionary = default!;
    public volatile ImmutableDictionary<int, int> immutableDictionary = default!;
    public volatile FrozenDictionary<int, int> frozenDictionary = default!;

    public volatile SortedDictionary<int, int> sortedDictionary = default!;
    public volatile ImmutableSortedDictionary<int, int> immutableSortedDictionary = default!;
    //public volatile FrozenSortedDictionary<int, int> frozenSortedDictionary = default!; // Added proposal https://github.com/dotnet/runtime/issues/79781

    public volatile SortedList<int, int> sortedList = default!;
    public volatile KeyValuePair<int, int>[] sorted_Array = default!;
    public ImmutableArray<KeyValuePair<int, int>> sorted_ImmutableArray = default!;
    //private SortedDictionary<int, int>.KeyValuePairComparer comparer = new (null); // Added issue https://github.com/dotnet/runtime/issues/79797
    private KeyValuePairComparer<int, int> arrayComparer = new();

    [GlobalSetup]
    public void GlobalSetup()
    {
        Randomizer.Seed = new Random(Seed);
        Faker faker = new();

        Dictionary<int, int> list = new(Size);
        while (list.Count < Size)
            _ = list.TryAdd(faker.Random.Number(MaxKey), faker.Random.Number(MaxValue));

        items = list;
    }

    [Benchmark(Baseline = true)]
    public void Dictionary_Construction()
    {
        dictionary = new (items);
    }

    [Benchmark()]
    public void ImmutableDictionary_Construction()
    {
        immutableDictionary = items.ToImmutableDictionary();
    }

    [Benchmark()]
    public void FrozenDictionary_Construction()
    {
        frozenDictionary = items.ToFrozenDictionary();
    }

    [Benchmark()]
    public void SortedDictionary_Construction()
    {
        //sortedDictionary = items.ToSortedDictionary(); // Added proposal https://github.com/dotnet/runtime/issues/79780
        sortedDictionary = new (items);
    }

    [Benchmark()]
    public void ImmutableSortedDictionary_Construction()
    {
        immutableSortedDictionary = items.ToImmutableSortedDictionary();
    }

    //[Benchmark()]
    //public void FrozenSortedDictionary_Construction()
    //{
    //    frozenSortedDictionary = items.ToFrozenSortedDictionary(); // Added proposal https://github.com/dotnet/runtime/issues/79781
    //}

    [Benchmark()]
    public void SortedList_Construction()
    {
        sortedList = new(items);
    }

    [Benchmark()]
    public void Sorted_Array_Construction()
    {
        sorted_Array = items.ToArray();
        Array.Sort(sorted_Array, arrayComparer);
    }

    [Benchmark()]
    public void Sorted_ImmutableArray_Construction()
    {
        var builder = ImmutableArray.CreateBuilder<KeyValuePair<int, int>>(items.Count);
        builder.AddRange(items);
        builder.Sort(arrayComparer);

        sorted_ImmutableArray = builder.MoveToImmutable();
    }
}
