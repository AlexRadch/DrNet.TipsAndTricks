using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace FastListLooping
{
    [MemoryDiagnoser(false)]
    public class Benchmarks
    {
        [Params(100, 10_000, 1_000_000)]
        public int Size { get; set; }

        private List<int> items = new ();
        public volatile int vItem;

        [GlobalSetup]
        public void GlobalSetup()
        {
            items = Generate(1_000).Take(Size).ToList();
        }

        [Benchmark]
        public void ForEarch()
        {
            foreach (var item in items)
                vItem = item;
        }

        [Benchmark]
        public void For()
        {
            for (var i = 0; i < items.Count; i++)
                vItem = items[i];
        }

        [Benchmark]
        public void ForEarchSpan()
        {
            ReadOnlySpan<int> span = CollectionsMarshal.AsSpan(items);
            foreach (var item in span)
                vItem = item;
        }

        [Benchmark]
        public void ForSpan()
        {
            ReadOnlySpan<int> span = CollectionsMarshal.AsSpan(items); ;
            for (var i = 0; i < span.Length; i++)
                vItem = span[i];
        }

        [Benchmark]
        public void UnsafeForRef()
        {
            ReadOnlySpan<int> span = CollectionsMarshal.AsSpan(items);
            ref var refItem = ref MemoryMarshal.GetReference(span);
            for (var i = 0; i < span.Length; i++)
                vItem = Unsafe.Add(ref refItem, i);
        }

        [Benchmark]
        public void UnsafeForRefEnd()
        {
            ReadOnlySpan<int> span = CollectionsMarshal.AsSpan(items);
            for (ref int refItem = ref MemoryMarshal.GetReference(span),
                refEnd = ref Unsafe.Add(ref refItem, span.Length);
                Unsafe.IsAddressLessThan(ref refItem, ref refEnd);
                refItem = ref Unsafe.Add(ref refItem, 1))
            {
                vItem = refItem;
            }
        }

        [Benchmark]
        public void UnsafeWhileRefEnd()
        {
            ReadOnlySpan<int> span = CollectionsMarshal.AsSpan(items);
            ref var refItem = ref MemoryMarshal.GetReference(span);
            ref var refEnd = ref Unsafe.Add(ref refItem, span.Length);
            while (Unsafe.IsAddressLessThan(ref refItem, ref refEnd))
            {
                vItem = refItem;
                refItem = ref Unsafe.Add(ref refItem, 1);
            }
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
}
