using System.Collections.Frozen;
using System.Collections.Immutable;

using BenchmarkDotNet.Attributes;
using Bogus;

namespace Frozen_Collections;

[MemoryDiagnoser(false)]
public class Sets_Construction
{
    [Params(10, 100, 1_000, 10_000, 100_000, 1_000_000)]
    public int Size { get; set; }
    public int MaxValue => checked(Size * 2); // exclusive

    public int Seed = 0x3a9d48f0;

    private IEnumerable<int> items = default!;

    public volatile HashSet<int> hashSet = default!;
    public volatile ImmutableHashSet<int> immutableHashSet = default!;
    public volatile FrozenSet<int> frozenSet = default!;

    public volatile SortedSet<int> sortedSet = default!;
    public volatile ImmutableSortedSet<int> immutableSortedSet = default!;
    //public volatile FrozenSortedSet<int> frozenSortedSet = default!; // Added proposal https://github.com/dotnet/runtime/issues/79781

    public volatile int[] sorted_Array = default!;
    public ImmutableArray<int> sorted_ImmutableArray = default!;
    //public volatile FrozenArray<int> Sorted_FrozenArray = default!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        Randomizer.Seed = new Random(Seed);
        Faker faker = new ();

        HashSet<int> list = new(Size);
        while (list.Count < Size)
            list.Add(faker.Random.Number(MaxValue));

        items = list.ToArray();
    }

    [Benchmark(Baseline = true)]
    public void HashSet_Construction()
    {
        hashSet = new (items);
    }

    [Benchmark()]
    public void ImmutableHashSet_Construction()
    {
        immutableHashSet = items.ToImmutableHashSet();
    }

    [Benchmark()]
    public void FrozenSet_Construction()
    {
        frozenSet = items.ToFrozenSet();
    }

    [Benchmark()]
    public void SortedSet_Construction()
    {
        sortedSet = new (items);
    }

    [Benchmark()]
    public void ImmutableSortedSet_Construction()
    {
        immutableSortedSet = items.ToImmutableSortedSet();
    }

    //[Benchmark()]
    //public void FrozenSortedSet_Construction()
    //{
    //    frozenSortedSet = list.ToFrozenSortedSet(); // Added proposal https://github.com/dotnet/runtime/issues/79781
    //}

    [Benchmark()]
    public void Sorted_Array_FastConstruction()
    {
        sorted_Array = items.ToArray();
        Array.Sort(sorted_Array);
    }

    [Benchmark()]
    public void Sorted_ImmutableArray_Construction()
    {
        var builder = ImmutableArray.CreateBuilder<int>(items.Count());
        builder.AddRange(items);
        builder.Sort();

        sorted_ImmutableArray = builder.MoveToImmutable();
    }
}
