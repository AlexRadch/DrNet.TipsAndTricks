using System.Collections.Frozen;
using System.Collections.Immutable;

using BenchmarkDotNet.Attributes;
using Bogus;

namespace Frozen_Collections;

public class Dictionaries_Access
{
    [Params(10, 100, 1_000, 10_000, 100_000, 1_000_000)]
    public int Size { get; set; }
    public int MaxKey => checked(Size * 2); // exclusive
    public int MaxValue => int.MaxValue; // exclusive

    [Params(10)]
    public int Items { get; set; }

    [Params(100)]
    public int Times { get; set; }

    public int Seed = 0x150991c4;

    private Dictionary<int, int> dictionary = default!;
    private ImmutableDictionary<int, int> immutableDictionary = default!;
    private FrozenDictionary<int, int> frozenDictionary = default!;

    private SortedDictionary<int, int> sortedDictionary = default!;
    private ImmutableSortedDictionary<int, int> immutableSortedDictionary = default!;
    //private FrozenSortedDictionary<int, int> frozenSortedDictionary = default!; // Added proposal https://github.com/dotnet/runtime/issues/79781

    private SortedList<int, int> sortedList = default!;
    private KeyValuePair<int, int>[] sorted_Array = default!;
    private ImmutableArray<KeyValuePair<int, int>> sorted_ImmutableArray = default!;
    //private SortedDictionary<int, int>.KeyValuePairComparer comparer = new (null); // Added issue https://github.com/dotnet/runtime/issues/79797 
    private KeyValuePairComparer<int, int> arrayComparer = new ();

    [GlobalSetup]
    public void GlobalSetup()
    {
        Randomizer.Seed = new Random(Seed);
        Faker faker = new ();

        Dictionary<int, int> dict = new(Size);
        while (dict.Count < Size)
            dict.TryAdd(faker.Random.Number(MaxKey), faker.Random.Number(MaxValue));


        dictionary = new (dict);
        immutableDictionary = dict.ToImmutableDictionary();
        frozenDictionary = dict.ToFrozenDictionary();

        //sortedDictionary = dict.ToSortedDictionary(); // Added proposal https://github.com/dotnet/runtime/issues/79780
        sortedDictionary = new (dict);

        immutableSortedDictionary = dict.ToImmutableSortedDictionary();
        //frozenSortedDictionary = dict.ToFrozenSortedDictionary(); // Added proposal https://github.com/dotnet/runtime/issues/79781

        //sortedList = dict.ToSortedList();
        sortedList = new(dict);

        sorted_Array = dict.ToArray();
        Array.Sort(sorted_Array, arrayComparer);

        {
            var builder = ImmutableArray.CreateBuilder<KeyValuePair<int, int>>(dict.Count);
            builder.InsertRange(0, dict);
            builder.Sort(arrayComparer);

            sorted_ImmutableArray = builder.MoveToImmutable();
        }
    }

    public volatile bool vFound;
    public volatile int vValue;

    [Benchmark(Baseline = true)]
    public void Dictionary_TryGetValue()
    {
        var maxKey = MaxKey;
        for (var i = 1; i <= Items; i++)
        {
            var key = maxKey * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                vFound = dictionary.TryGetValue(key, out _);
        }
    }

    [Benchmark()]
    public void ImmutableDictionary_TryGetValue()
    {
        var maxKey = MaxKey;
        for (var i = 1; i <= Items; i++)
        {
            var key = maxKey * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                vFound = immutableDictionary.TryGetValue(key, out _);
        }
    }

    [Benchmark()]
    public void FrozenDictionary_TryGetValue()
    {
        var maxKey = MaxKey;
        for (var i = 1; i <= Items; i++)
        {
            var key = maxKey * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                vFound = frozenDictionary.TryGetValue(key, out _);
        }
    }

    [Benchmark()]
    public void FrozenDictionary_ByIndex()
    {
        for (var i = 1; i <= Items; i++)
        {
            var index = Items * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                vValue = frozenDictionary[frozenDictionary.Keys[index]];
        }
    }

    [Benchmark()]
    public void SortedDictionary_TryGetValue()
    {
        var maxKey = MaxKey;
        for (var i = 1; i <= Items; i++)
        {
            var key = maxKey * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                vFound = sortedDictionary.TryGetValue(key, out _);
        }
    }

    [Benchmark()]
    public void ImmutableSortedDictionary_TryGetValue()
    {
        var maxKey = MaxKey;
        for (var i = 1; i <= Items; i++)
        {
            var key = maxKey * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                vFound = immutableSortedDictionary.TryGetValue(key, out _);
        }
    }

    //[Benchmark()]
    //public void FrozenSortedDictionary_TryGetValue()
    //{
    //    var maxKey = MaxKey;
    //    for (var i = 1; i <= Items; i++)
    //    {
    //        var key = maxKey * i / (Items + 1);

    //        for (var times = 0; times < Times; times++)
    //            vFound = frozenSortedDictionary.TryGetValue(key, out _);
    //    }
    //}

    //[Benchmark()]
    //public void FrozenSortedDictionary_ByIndex()
    //{
    //    for (var i = 1; i <= Items; i++)
    //    {
    //        var index = Items * i / (Items + 1);

    //        for (var times = 0; times < Times; times++)
    //            vValue = frozenSortedDictionary[frozenSortedDictionary.Keys[index]];
    //    }
    //}

    [Benchmark()]
    public void SortedList_TryGetValue()
    {
        var maxKey = MaxKey;
        for (var i = 1; i <= Items; i++)
        {
            var key = maxKey * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                sortedList.TryGetValue(key, out _);
        }
    }

    [Benchmark()]
    public void SortedList_ByIndex()
    {
        for (var i = 1; i <= Items; i++)
        {
            var index = Items * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                vValue = sortedList[sortedList.Keys[index]];
        }
    }

    [Benchmark()]
    public void Sorted_Array_TryGetValue()
    {
        var maxKey = MaxKey;
        for (var i = 1; i <= Items; i++)
        {
            var key = maxKey * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                if (Array.BinarySearch(sorted_Array, new KeyValuePair<int, int>(key, default), arrayComparer) is var index && (vFound = index >= 0))
                    _ = sorted_Array[key];
                else
                    _ = default(int);
        }
    }

    [Benchmark()]
    public void Sorted_Array_ByIndex()
    {
        for (var i = 1; i <= Items; i++)
        {
            var index = Items * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                vValue = sorted_Array[index].Value;
        }
    }

    [Benchmark()]
    public void Sorted_ImmutableArray_TryGetValue()
    {
        var maxKey = MaxKey;
        for (var i = 1; i <= Items; i++)
        {
            var key = maxKey * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                if (sorted_ImmutableArray.BinarySearch(new KeyValuePair<int, int>(key, default), arrayComparer) is var index && (vFound = index >= 0))
                    _ = sorted_ImmutableArray[key];
                else
                    _ = default(int);
        }
    }

    [Benchmark()]
    public void Sorted_ImmutableArray_ByIndex()
    {
        for (var i = 1; i <= Items; i++)
        {
            var index = Items * i / (Items + 1);

            for (var times = 0; times < Times; times++)
                vValue = sorted_ImmutableArray[index].Value;
        }
    }

}