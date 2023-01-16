using System.Collections.Frozen;
using System.Collections.Immutable;

using BenchmarkDotNet.Attributes;
using Bogus;

namespace Frozen_Collections;

public class Sets_Access
{
    [Params(10, 100, 1_000, 10_000, 100_000, 1_000_000)]
    public int Size { get; set; }
    public int MaxValue => checked(Size * 2); // exclusive

    [Params(10)]
    public int Items { get; set; }

    [Params(100)]
    public int Times { get; set; }

    public int Seed = 0x367af5f9;

    private HashSet<int> hashSet = default!;
    private ImmutableHashSet<int> immutableHashSet = default!;
    private FrozenSet<int> frozenSet = default!;

    private SortedSet<int> sortedSet = default!;
    private ImmutableSortedSet<int> immutableSortedSet = default!;
    //private FrozenSortedSet<int> frozenSortedSet = default!; // Added proposal https://github.com/dotnet/runtime/issues/79781

    private int[] sorted_Array = default!;
    private ImmutableArray<int> sorted_ImmutableArray = default!;
    //public FrozenArray<int> sorted_FrozenArray = default!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        Randomizer.Seed = new Random(Seed);
        Faker faker = new ();

        HashSet<int> list = new(Size);
        while (list.Count < Size) 
            list.Add(faker.Random.Number(MaxValue));


        hashSet = list.ToHashSet();
        immutableHashSet = list.ToImmutableHashSet();
        frozenSet = list.ToFrozenSet();

        //sortedSet = list.ToSortedSet(); // Added proposal https://github.com/dotnet/runtime/issues/79780
        sortedSet = new SortedSet<int>(list);
        immutableSortedSet = list.ToImmutableSortedSet();
        //frozenSortedSet = list.ToFrozenSortedSet(); // Added proposal https://github.com/dotnet/runtime/issues/79781

        sorted_Array = list.ToArray();
        Array.Sort(sorted_Array);

        {
            var builder = ImmutableArray.CreateBuilder<int>(list.Count);
            builder.AddRange(list);
            builder.Sort();

            sorted_ImmutableArray = builder.MoveToImmutable();
        }
    }

    public volatile bool vFound;
    public volatile int vValue;

    [Benchmark(Baseline = true)]
    public void HashSet_Contains()
    {
        var maxValue = MaxValue;
        for (var i = 1; i <= Items; i++)
        {
            var value = maxValue * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                vFound = hashSet.Contains(value);
        }
    }

    [Benchmark()]
    public void ImmutableHashSet_Contains()
    {
        var maxValue = MaxValue;
        for (var i = 1; i <= Items; i++)
        {
            var value = maxValue * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                vFound = immutableHashSet.Contains(value);
        }
    }

    [Benchmark()]
    public void FrozenSet_Contains()
    {
        var maxValue = MaxValue;
        for (var i = 1; i <= Items; i++)
        {
            var value = maxValue * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                vFound = frozenSet.Contains(value);
        }
    }

    [Benchmark()]
    public void SortedSet_Contains()
    {
        var maxValue = MaxValue;
        for (var i = 1; i <= Items; i++)
        {
            var value = maxValue * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                vFound = sortedSet.Contains(value);
        }
    }

    //[Benchmark()]
    //public void SortedSet_ByIndex()
    //{
    //    for (var i = 1; i <= Items; i++)
    //    {
    //        var index = Items * i / (Items + 1);

    //        for (var times = 0; times < Times; times++)
    //            vValue = sortedSet[index]; // NO!!!
    //    }
    //}

    [Benchmark()]
    public void ImmutableSortedSet_Contains()
    {
        var maxValue = MaxValue;
        for (var i = 1; i <= Items; i++)
        {
            var value = maxValue * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                vFound = immutableSortedSet.Contains(value);
        }
    }

    [Benchmark()]
    public void ImmutableSortedSet_ByIndex()
    {
        for (var i = 1; i <= Items; i++)
        {
            var index = Items * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                vValue = immutableSortedSet[index];
        }
    }

    //[Benchmark()]
    //public void FrozenSortedSet_Contains()
    //{
    //    for (var i = 1; i <= Items; i++)
    //    {
    //        var value = maxValue * i / (Items + 1);

    //        for (var times = 0; times < Times; times++)
    //            vFound = frozenSortedSet.Contains(item);
    //    }
    //}

    //[Benchmark()]
    //public void FrozenSortedSet_ByIndex()
    //{
    //    for (var i = 1; i <= Items; i++)
    //    {
    //        var index = Items * i / (Items + 1);

    //        for (var times = 0; times < Times; times++)
    //            vValue = frozenSortedSet[index];
    //    }
    //}

    [Benchmark()]
    public void Sorted_Array_Contains()
    {
        var maxValue = MaxValue;
        for (var i = 1; i <= Items; i++)
        {
            var value = maxValue * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                vFound = Array.BinarySearch(sorted_Array, value) >= 0;
        }
    }

    [Benchmark()]
    public void Sorted_Array_ByIndex()
    {
        for (var i = 1; i <= Items; i++)
        {
            var index = Items * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                vValue = sorted_Array[index];
        }
    }

    [Benchmark()]
    public void Sorted_ImmutableArray_Contains()
    {
        var maxValue = MaxValue;
        for (var i = 1; i <= Items; i++)
        {
            var value = maxValue * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                vFound = sorted_ImmutableArray.BinarySearch(value) >= 0;
        }
    }

    [Benchmark()]
    public void Sorted_ImmutableArray_ByIndex()
    {
        for (var i = 1; i <= Items; i++)
        {
            var index = Items * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                vValue = sorted_ImmutableArray[index];
        }
    }
}
