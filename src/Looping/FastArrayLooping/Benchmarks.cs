using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace FastArrayLooping
{
    [MemoryDiagnoser(false)]
    public class Benchmarks
    {
        [Params(100, 10_000, 1_000_000)]
        public int Size { get; set; }

        private int[] items = new int[0];
        public volatile int vItem;

        [GlobalSetup]
        public void GlobalSetup()
        {
            items = Generate(new Random(), random => random.Next(1_000)).Take(Size).ToArray();
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
            for (var i = 0; i < items.Length; i++)
                vItem = items[i];
        }

        [Benchmark]
        public void ForEarchSpan()
        {
            Span<int> span = items;
            foreach (var item in span)
                vItem = item;
        }

        [Benchmark]
        public void ForSpan()
        {
            Span<int> span = items;
            for (var i = 0; i < span.Length; i++)
                vItem = span[i];
        }

        [Benchmark]
        public void UnsafeForRef()
        {
            ref var refItem = ref MemoryMarshal.GetReference<int>(items);
            for (var i = 0; i < items.Length; i++)
                vItem = Unsafe.Add(ref refItem, i);
        }

        [Benchmark]
        public void UnsafeForRefEnd()
        {
            for (ref int refItem = ref MemoryMarshal.GetReference<int>(items),
                refEnd = ref Unsafe.Add(ref refItem, items.Length);
                Unsafe.IsAddressLessThan(ref refItem, ref refEnd);
                refItem = ref Unsafe.Add(ref refItem, 1))
            {
                vItem = refItem;
            }
        }

        [Benchmark]
        public void UnsafeWhileRefEnd()
        {
            ref var refItem = ref MemoryMarshal.GetReference<int>(items);
            ref var refEnd = ref Unsafe.Add(ref refItem, items.Length);
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
