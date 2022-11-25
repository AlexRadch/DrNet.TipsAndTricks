using BenchmarkDotNet.Running;
using FastArrayLooping;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

var summary = BenchmarkRunner.Run<Benchmarks>();

//UnsafeWhileRefEnd();
//TestRef();

//static void UnsafeWhileRefEnd()
//{
//    var items = Benchmarks.Generate(1_000).Take(5).ToArray();
//    {
//        ref var item = ref MemoryMarshal.GetReference<int>(items);
//        ref var end = ref Unsafe.Add(ref item, items.Length - 1);
//        while (!Unsafe.IsAddressGreaterThan(ref item, ref end))
//        {
//            Console.WriteLine(item);
//            item = ref Unsafe.Add(ref item, 1);
//        }
//        Console.WriteLine();
//    }

//    {
//        ref var item = ref MemoryMarshal.GetReference<int>(items);
//        ref var end = ref Unsafe.Add(ref item, items.Length);
//        while (Unsafe.IsAddressLessThan(ref item, ref end))
//        {
//            Console.WriteLine(item);
//            item = ref Unsafe.Add(ref item, 1);
//        }
//        Console.WriteLine();
//    }
//}

static void TestRef()
{
    var items = Benchmarks.Generate(1_000).Take(5).ToArray();

    ref int refItem = ref items[0];
    Console.WriteLine(refItem);

    //refItem = ref items[items.Length]; // Exception
    //Console.WriteLine(refItem);

    //items = null;
    //refItem = ref items![0]; // Exception
    //Console.WriteLine(refItem);

    //items = Array.Empty<int>();
    //refItem = ref items[0]; // Exception
    //Console.WriteLine(refItem);

    //items = new int[0];
    //refItem = ref items[0];
    //Console.WriteLine(refItem);
}
